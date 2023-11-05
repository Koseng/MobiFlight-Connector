using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MobiFlight.InputConfig
{
    public class ButtonInputConfig : IXmlSerializable, ICloneable
    {
        public InputAction onPress;        
        public InputAction onRelease;        
        public InputAction onLongRelease;
        public InputAction onLongPress;

        private CacheCollection LastOnPressCacheCollection;
        private InputEventArgs LastOnPressEvent;
        private List<ConfigRefValue> LastOnPressConfigRefs;

        private const int DELAY_LONG_RELEASE = 350; //ms
        public int LongPressDelay = 400;
        public int RepeatDelay = 0;

        private System.Windows.Forms.Timer LongPressTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer RepeatTimer = new System.Windows.Forms.Timer();

        public ButtonInputConfig()
        {           
            LongPressTimer.Tick += LongPressDelayTimer_Tick;
            RepeatTimer.Tick += RepeatTimer_Tick;
        }

        public object Clone()
        {
            ButtonInputConfig clone = new ButtonInputConfig();
            if (onPress != null) clone.onPress = (InputAction)onPress.Clone();
            if (onRelease != null) clone.onRelease = (InputAction)onRelease.Clone();            
            if (onLongRelease != null) clone.onLongRelease = (InputAction)onLongRelease.Clone();
            if (onLongPress != null) clone.onLongPress = (InputAction)onLongPress.Clone();
            return clone;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.Read(); // this should be the opening tag "onPress"
            if (reader.LocalName == "") reader.Read();
            if (reader.LocalName == "onPress")
            {                
                onPress = InputActionFactory.CreateByType(reader["type"]);
                onPress?.ReadXml(reader);
                reader.Read(); // Closing onPress
            }

            if (reader.LocalName == "") reader.Read();
            if (reader.LocalName == "onRelease")
            {                
                onRelease = InputActionFactory.CreateByType(reader["type"]);
                onRelease?.ReadXml(reader);
                reader.Read(); // closing onRelease
            }

            if (reader.LocalName == "") reader.Read();
            if (reader.LocalName == "onLongRelease")
            {
                onLongRelease = InputActionFactory.CreateByType(reader["type"]);
                onLongRelease?.ReadXml(reader);
                reader.Read(); // closing onLongRelease
            }

            if (reader.LocalName == "") reader.Read();
            if (reader.LocalName == "onLongPress")
            {
                string longPressDelay = reader["longPressDelay"];
                string repeatDelay = reader["repeatDelay"];
                LongPressDelay = int.Parse(longPressDelay);
                RepeatDelay = int.Parse(repeatDelay);                
                onLongPress = InputActionFactory.CreateByType(reader["type"]);
                onLongPress?.ReadXml(reader);
                reader.Read(); // closing onLongRelease
            }

            if (reader.NodeType==System.Xml.XmlNodeType.EndElement)
                reader.Read();
        }

        public void SetInputActionByName(string name, InputAction inputAction)
        {
            switch (name)
            {
                case "onPress":
                    onPress = inputAction;
                    break;
                case "onRelease":
                    onRelease = inputAction;
                    break;                   
                case "onLongRelease":
                    onLongRelease = inputAction;
                    break;
                case "onLongPress":
                    onLongPress = inputAction;
                    break;
            }
        }

        public InputAction GetInputActionByName(string name)
        {
            switch (name)
            {
                case "onPress":
                    return onPress;
                case "onRelease":
                    return onRelease;
                case "onLongRelease":
                    return onLongRelease;
                case "onLongPress":
                    return onLongPress;
                default:
                    return null;
            }        
        }

        public List<InputAction> GetInputActionsByType(Type type)
        {
            List<InputAction> result = new List<InputAction>();
            if (onPress != null && onPress.GetType()==type)
                result.Add(onPress);
            if (onRelease != null && onRelease.GetType() == type)
                result.Add(onRelease);
            if (onLongRelease != null && onLongRelease.GetType() == type)
                result.Add(onLongRelease);
            if (onLongPress != null && onLongPress.GetType() == type)
                result.Add(onLongPress);
            return result;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (onPress != null)
            {
                writer.WriteStartElement("onPress");
                onPress.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (onRelease != null)
            {
                writer.WriteStartElement("onRelease");
                onRelease.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (onLongRelease != null)
            {
                writer.WriteStartElement("onLongRelease");
                onLongRelease.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (onLongPress != null)
            {
                writer.WriteStartElement("onLongPress");
                writer.WriteAttributeString("longPressDelay", LongPressDelay.ToString());
                writer.WriteAttributeString("repeatDelay", RepeatDelay.ToString());               
                onLongPress.WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        private void ExecuteOnLongPressAction()
        {
            InputEventArgs args = (InputEventArgs)LastOnPressEvent.Clone();
            args.Value = (int)MobiFlightButton.InputEvent.LONG_PRESS;
            onPress.execute(LastOnPressCacheCollection, args, LastOnPressConfigRefs);
        }

        private void RepeatTimer_Tick(object sender, EventArgs e)
        {
            ExecuteOnLongPressAction();
        }

        private void LongPressDelayTimer_Tick(object sender, EventArgs e)
        {
            ExecuteOnLongPressAction();
            LongPressTimer.Stop();

            if (RepeatDelay > 0)
            {
                RepeatTimer.Interval = RepeatDelay;
                RepeatTimer.Start();
            }
        }

        private void StartTimer()
        {
            if (LongPressDelay > 0)
            {
                LongPressTimer.Interval = LongPressDelay;
                LongPressTimer.Start();
            }
            else if (RepeatDelay > 0)
            {
                RepeatTimer.Interval = RepeatDelay;
                RepeatTimer.Start();
            }
        }

        private void CheckAndStopTimer()
        {
            if (LongPressTimer.Enabled)
                LongPressTimer.Stop();
            if (RepeatTimer.Enabled)
                RepeatTimer.Stop();
        }

        private void CheckAndAdaptForLongButtonRelease(InputEventArgs current, InputEventArgs previous)
        {
            var inputEvent = (MobiFlightButton.InputEvent)current.Value;
            TimeSpan timeSpanToPreviousInput = current.Time - previous.Time;

            if (inputEvent == MobiFlightButton.InputEvent.RELEASE &&
                onLongRelease != null &&
                previous != null &&
                timeSpanToPreviousInput > TimeSpan.FromMilliseconds(DELAY_LONG_RELEASE))
            {
                current.Value = (int)MobiFlightButton.InputEvent.LONG_RELEASE;
                Log.Instance.log($"{current.Name} => {current.DeviceLabel}  => Execute as LONG_RELEASE", LogSeverity.Info);
            }
        }

        private void ExecuteOnPressAction(CacheCollection cacheCollection,
                                          InputEventArgs args,
                                          List<ConfigRefValue> configRefs)
        {
            LastOnPressEvent = args;
            LastOnPressCacheCollection = cacheCollection;
            LastOnPressConfigRefs = configRefs;

            if (onPress != null)
            {
                onPress.execute(cacheCollection, args, configRefs);
            }
            if (onLongPress != null)
            {
                // LongPress will be executed after timer tick.
                StartTimer();
            }
        }

        private void ExecuteOnReleaseAction(CacheCollection cacheCollection,
                                            InputEventArgs args,
                                            List<ConfigRefValue> configRefs)
        {
            CheckAndStopTimer();
            if (onRelease != null)
            {
                onRelease.execute(cacheCollection, args, configRefs);
            }
        }

        private void ExecuteOnLongReleaseAction(CacheCollection cacheCollection,
                                                InputEventArgs args,
                                                List<ConfigRefValue> configRefs)
        {
            CheckAndStopTimer();
            if (onLongRelease != null)
            {
                onLongRelease.execute(cacheCollection, args, configRefs);
            }
        }

        internal void execute(CacheCollection cacheCollection, 
                              InputEventArgs args, 
                              List<ConfigRefValue> configRefs)
        {
            CheckAndAdaptForLongButtonRelease(args, LastOnPressEvent);

            switch ((MobiFlightButton.InputEvent)args.Value)
            {
                case MobiFlightButton.InputEvent.PRESS:
                    ExecuteOnPressAction(cacheCollection, args, configRefs);
                    break;
                case MobiFlightButton.InputEvent.RELEASE:
                    ExecuteOnReleaseAction(cacheCollection, args, configRefs);
                    break;
                case MobiFlightButton.InputEvent.LONG_RELEASE:
                    ExecuteOnLongReleaseAction(cacheCollection, args, configRefs);
                    break;
                default:
                    break;
            }
        }

        public Dictionary<String, int> GetStatistics()
        {
            Dictionary<String, int> result = new Dictionary<string, int>();

            result["Input.Button"] = 1;

            if (onPress != null)
            {
                result["Input.OnPress"] = 1;
                result["Input." + onPress.GetType().Name] = 1;
            }

            if (onRelease != null)
            {
                result["Input.OnRelease"] = 1;
                result["Input." + onRelease.GetType().Name] = 1;
            }

            if (onLongRelease != null)
            {
                result["Input.OnLongRelease"] = 1;
                result["Input." + onLongRelease.GetType().Name] = 1;
            }

            if (onLongPress != null)
            {
                result["Input.OnLongPress"] = 1;
                result["Input." + onLongPress.GetType().Name] = 1;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is ButtonInputConfig && 
                (
                    (onPress == null && ((obj as ButtonInputConfig).onPress == null)) || 
                    (onPress != null && onPress.Equals((obj as ButtonInputConfig).onPress))
                ) &&
                (
                    (onRelease == null && ((obj as ButtonInputConfig).onRelease == null)) ||
                    (onRelease != null && onRelease.Equals((obj as ButtonInputConfig).onRelease))
                ) &&
                (
                    (onLongRelease == null && ((obj as ButtonInputConfig).onLongRelease == null)) ||
                    (onLongRelease != null && onLongRelease.Equals((obj as ButtonInputConfig).onLongRelease))
                ) &&
                (
                    (onLongPress == null && ((obj as ButtonInputConfig).onLongPress == null)) ||
                    (onLongPress != null && onLongPress.Equals((obj as ButtonInputConfig).onLongPress))
                );
        }
    }
}
