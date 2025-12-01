import asyncio, ctypes, json, logging, struct
from ctypes import wintypes, Structure, c_ubyte, sizeof
from typing import Any
import websockets.asyncio.client as ws_client
from SimConnect import SimConnect, Enum
from SimConnect.Enum import SIMCONNECT_CLIENT_DATA_ID, SIMCONNECT_RECV_ID, SIMCONNECT_RECV_CLIENT_DATA

# --- Config ---
CAPTAIN_MCDU_URL = "ws://localhost:8320/winwing/cdu-captain"
FO_MCDU_URL = "ws://localhost:8320/winwing/cdu-co-pilot"
MCDU_FLAG_SMALL_FONT = 0x01

MCDUColor_Map = {0:"w",1:"c",2:"a",3:"g",4:"e",5:"r",6:"y",7:"m"}
special_chars = {'a':'←','b':'→','e':'↑','f':'↓','o':'☐','d':'°','c':'Δ'}

MCDU_COLUMNS, MCDU_ROWS = 24, 14
MCDU_CHARS = MCDU_COLUMNS * MCDU_ROWS

class MCDUChar(Structure):
    _pack_ = 1
    _fields_ = [("Symbol", c_ubyte), ("Color", c_ubyte), ("Flags", c_ubyte)]

MCDU_CHAR_SIZE = sizeof(MCDUChar)
MCDU_DATA_SIZE = MCDU_CHAR_SIZE * MCDU_CHARS
A340_MCDU_CPT_NAME, A340_CPT_MCDU_CLIENT_DATA_ID = "A340MCDU", 0
A340_MCDU_FO_NAME, A340_FO_MCDU_CLIENT_DATA_ID = "iniAirbusMCDU_2", 1
A340_MCDU_CPT_DEFINITION, A340_MCDU_FO_DEFINITION = 0, 1

# --- SimConnect Wrapper ---
class SimConnectMobiFlight(SimConnect):
    def __init__(self, auto_connect=True, library_path=None):
        self.handlers = []
        super().__init__(auto_connect, library_path) if library_path else super().__init__(auto_connect)
        self.dll.MapClientDataNameToID.argtypes = [wintypes.HANDLE, ctypes.c_char_p, SIMCONNECT_CLIENT_DATA_ID]

    def register_handler(self, h): 
        if h not in self.handlers: self.handlers.append(h)

    def unregister_handler(self, h): 
        if h in self.handlers: self.handlers.remove(h)

    def my_dispatch_proc(self, pData=None, *_):
        if not pData: return
        if pData.contents.dwID == SIMCONNECT_RECV_ID.SIMCONNECT_RECV_ID_CLIENT_DATA:
            data = ctypes.cast(pData, ctypes.POINTER(SIMCONNECT_RECV_CLIENT_DATA)).contents
            for h in self.handlers: h(data)
        else: super().my_dispatch_proc(pData, *_)

# --- MobiFlight WebSocket Client ---
class MobiFlightClient:
    def __init__(self, uri:str, max_retries=3):
        self.uri, self.max_retries, self.retries = uri, max_retries, 0
        self.connected, self.websocket = asyncio.Event(), None
        self._was_connected, self.last_data = False, None

    async def run(self):
        while self.retries < self.max_retries:
            try:
                logging.info(f"Connecting to {self.uri}")
                self.websocket = await ws_client.connect(self.uri, ping_interval=None)                
                await self.websocket.send(json.dumps({"Target":"Font","Data":"AirbusThales"}))
                logging.info(f"Setting font: AirbusThales")
                await asyncio.sleep(1) # wait a second for font to be set
                self.connected.set()
                if self._was_connected and self.last_data: await self.send(self.last_data)
                self._was_connected, self.retries = True, 0
                async for _ in self.websocket: pass
            except Exception as e:
                self.retries += 1;               
                logging.info(f"WebSocket failure: {e} ({self.retries}/{self.max_retries})")
                self.websocket = None
                self.connected.clear()
            await asyncio.sleep(5)     
        logging.info("Max retries reached. Giving up connecting to MobiFlight at %s. If you only have one CDU attached, you can ignore this message.", self.uri)
        self.connected.set(); 
        
    async def send(self, data:str):
        if self.websocket and self.connected.is_set():
            await self.websocket.send(data); self.last_data = data
            
    async def close(self):
        if self.websocket: 
            await self.websocket.close()
            self.websocket = None
            self.connected.clear()

# --- Data Conversion ---
def create_mobi_json(data:bytes)->str:
    out = {"Target":"Display","Data":[[] for _ in range(MCDU_CHARS)]}
    
    for i in range(MCDU_CHARS):
        idx = (i//MCDU_ROWS*MCDU_ROWS + i%MCDU_ROWS)*MCDU_CHAR_SIZE
        if idx+2 >= len(data): continue
        try:
            sym, col, flg = chr(data[idx]), data[idx+1], data[idx+2]
            if sym in (" ","\0"): continue
            sym = special_chars.get(sym, sym)
            out["Data"][i] = [sym, MCDUColor_Map.get(col,"w"), int(bool(flg&MCDU_FLAG_SMALL_FONT))]
        except: pass
    return json.dumps(out)

# --- MCDU Client ---
class A340MCDUClient:
    def __init__(self, sc:SimConnectMobiFlight, uri:str, def_id:int, CLIENT_AREA_NAME:str, CLIENT_AREA_ID:int):
        self.sc, self.uri, self.def_id, self.CA_NAME, self.CA_ID = sc, uri, def_id, CLIENT_AREA_NAME, CLIENT_AREA_ID
        self.mobiflight, self.last_data, self.loop = MobiFlightClient(uri), None, None
        logging.info(f"Connecting to {self.uri}")

    def setup(self):
        try:
            sc = self.sc; h = sc.hSimConnect
            sc.dll.MapClientDataNameToID(h, self.CA_NAME.encode(), self.CA_ID)
            sc.dll.AddToClientDataDefinition(h, self.def_id, 0, MCDU_DATA_SIZE, 0, 0)
            sc.dll.RequestClientData(h, self.CA_ID, self.def_id, self.def_id,
                Enum.SIMCONNECT_CLIENT_DATA_PERIOD.SIMCONNECT_CLIENT_DATA_PERIOD_VISUAL_FRAME,
                Enum.SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.SIMCONNECT_CLIENT_DATA_REQUEST_FLAG_CHANGED,0,0,0)
            sc.register_handler(self.on_data)
            return True
        except Exception as e: logging.error(f"SimConnect setup failed: {e}"); return False

    def on_data(self, d:Any):
        if d.dwDefineID!=self.def_id or not hasattr(d,"dwData"): return
        count=int(MCDU_DATA_SIZE/4)
        try: data=struct.pack(f"{count}I",*d.dwData[:count])
        except: data=b"".join(struct.pack("I",x) for x in d.dwData[:count])
        if data==self.last_data: return
        self.last_data=data
        json_data=create_mobi_json(data)
        asyncio.run_coroutine_threadsafe(self.mobiflight.send(json_data), self.loop)

    async def process_sc(self):
        while True: 
            try: self.sc.my_dispatch_proc()
            except: pass
            await asyncio.sleep(0.1)

    async def run(self):
        try:
            self.loop=asyncio.get_running_loop()
            task_ws=asyncio.create_task(self.mobiflight.run())
            await self.mobiflight.connected.wait()
            if self.mobiflight.retries>=self.mobiflight.max_retries: return
            if not self.setup(): return
            await asyncio.gather(task_ws, self.process_sc())
        finally:
            await self.mobiflight.close()
            
# --- Main ---
if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO, format="%(asctime)s - %(levelname)s - %(message)s")
    sc=SimConnectMobiFlight()

    mcdu_CPT=A340MCDUClient(sc, CAPTAIN_MCDU_URL, A340_MCDU_CPT_DEFINITION, A340_MCDU_CPT_NAME, A340_CPT_MCDU_CLIENT_DATA_ID)
    mcdu_FO=A340MCDUClient(sc, FO_MCDU_URL, A340_MCDU_FO_DEFINITION, A340_MCDU_FO_NAME, A340_FO_MCDU_CLIENT_DATA_ID)

    async def main(): 
        await asyncio.gather(
            mcdu_CPT.run(),
            mcdu_FO.run()
        )
    try: asyncio.run(main())
    except KeyboardInterrupt: pass
    finally: sc.exit()
