using MobiFlightWwFcu;

namespace MobiFlightWwFcuUnitTests
{
    [TestClass]
    public class WinwingTcasDeviceTests
    {
        private MockWinwingMessageSender mockMessageSender;
        private WinwingTcasDevice device;

        // Refresh command (always identical, 17 bytes)
        private static readonly byte[] RefreshCommand = new byte[]
        {
            0x81, 0xBB, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        [TestInitialize]
        public void Setup()
        {
            mockMessageSender = new MockWinwingMessageSender();
            device = new WinwingTcasDevice(mockMessageSender);
        }

        [TestCleanup]
        public void Cleanup()
        {
            device?.Stop();
        }

        #region Basic Properties Tests

        [TestMethod]
        public void Name_ShouldReturnCorrectDeviceName()
        {
            Assert.AreEqual("WinWing TCAS", device.Name);
        }

        [TestMethod]
        public void GetDisplayNames_ShouldReturnAllDisplayNames()
        {
            var displayNames = device.GetDisplayNames();

            Assert.IsNotNull(displayNames);
            Assert.IsTrue(displayNames.Contains("Number Of Digits"));
            Assert.IsTrue(displayNames.Contains("Ident Value"));
            Assert.IsTrue(displayNames.Contains("LCD Test On/Off"));
            Assert.AreEqual(3, displayNames.Count);
        }

        [TestMethod]
        public void GetLedNames_ShouldReturnAllLedNames()
        {
            var ledNames = device.GetLedNames();

            Assert.IsNotNull(ledNames);
            Assert.IsTrue(ledNames.Contains("ATC_FAIL"));
            Assert.IsTrue(ledNames.Contains("Backlight Percentage"));
            Assert.IsTrue(ledNames.Contains("LED Percentage"));
            Assert.IsTrue(ledNames.Contains("LCD Percentage"));
            Assert.AreEqual(4, ledNames.Count);
        }

        [TestMethod]
        public void GetInternalDisplayNames_ShouldReturnEmptyList()
        {
            var internalDisplayNames = device.GetInternalDisplayNames();

            Assert.IsNotNull(internalDisplayNames);
            Assert.AreEqual(0, internalDisplayNames.Count);
        }

        #endregion

        #region Connect and Shutdown Tests

        [TestMethod]
        public void Connect_ShouldInitializeDisplay()
        {
            device.Connect();

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);
            Assert.IsTrue(mockMessageSender.BrightnessCommands.Count >= 2);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void Shutdown_ShouldEmptyDisplayAndTurnOffLights()
        {
            device.Connect();
            mockMessageSender.Reset();

            device.Shutdown();

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);
            Assert.IsTrue(mockMessageSender.BrightnessCommands.Count >= 2);

            List<byte[]> expectedEmptyDisplay = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedEmptyDisplay);
        }

        [TestMethod]
        public void Stop_ShouldTurnOffAllLEDs()
        {
            device.Connect();
            device.SetLed("ATC_FAIL", 1);
            mockMessageSender.Reset();

            device.Stop();

            Assert.IsTrue(mockMessageSender.LightControlCommands.Count >= 1);
            Assert.AreEqual(0, mockMessageSender.DisplayCommandsSent.Count);
        }

        #endregion

        #region LED Tests

        [TestMethod]
        public void SetLed_AtcFail_On_ShouldSendLightControlMessage()
        {
            device.SetLed("ATC_FAIL", 1);

            Assert.AreEqual(1, mockMessageSender.LightControlCommands.Count);
            var command = mockMessageSender.LightControlCommands[0];
            Assert.AreEqual(0x03, command.Type);
            Assert.AreEqual(1, command.Value);
        }

        [TestMethod]
        public void SetLed_AtcFail_Off_ShouldSendZeroValue()
        {
            device.SetLed("ATC_FAIL", 1);
            device.SetLed("ATC_FAIL", 0);

            Assert.AreEqual(2, mockMessageSender.LightControlCommands.Count);
            var command = mockMessageSender.LightControlCommands[1];
            Assert.AreEqual(0x03, command.Type);
            Assert.AreEqual(0, command.Value);
        }

        [TestMethod]
        public void SetLed_WithSameStateTwice_ShouldOnlySendOnce()
        {
            device.SetLed("ATC_FAIL", 1);
            device.SetLed("ATC_FAIL", 1);

            Assert.AreEqual(1, mockMessageSender.LightControlCommands.Count);
        }

        [TestMethod]
        public void SetLed_WithNullOrEmptyName_ShouldNotSendCommand()
        {
            device.SetLed(null, 1);
            device.SetLed("", 1);

            Assert.AreEqual(0, mockMessageSender.LightControlCommands.Count);
        }

        #endregion

        #region Ident Value Tests (default 4 digits)

        [TestMethod]
        public void SetDisplay_IdentValue_With1234_ShouldDisplay1234()
        {
            device.SetDisplay("Ident Value", "1234");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_IdentValue_With0_ShouldDisplay0000()
        {
            device.SetDisplay("Ident Value", "0");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_IdentValue_With11_ShouldDisplay0011()
        {
            device.SetDisplay("Ident Value", "11");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_IdentValue_With7620_ShouldDisplay7620()
        {
            device.SetDisplay("Ident Value", "7620");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_IdentValue_With9999_ShouldDisplay9999()
        {
            device.SetDisplay("Ident Value", "9999");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_IdentValue_With12345_ShouldTruncateTo1234()
        {
            device.SetDisplay("Ident Value", "12345");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_IdentValue_With123456789_ShouldTruncateTo1234()
        {
            device.SetDisplay("Ident Value", "123456789");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_IdentValue_With567_ShouldDisplay0567()
        {
            device.SetDisplay("Ident Value", "567");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        #endregion

        #region Number Of Digits + Ident Value combinations

        [TestMethod]
        public void SetDisplay_NumberOfDigits3_With1234_ShouldDisplay123Star()
        {
            device.SetDisplay("Number Of Digits", "3");
            device.SetDisplay("Ident Value", "1234");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_NumberOfDigits3_With12_ShouldDisplay012Star()
        {
            device.SetDisplay("Number Of Digits", "3");
            device.SetDisplay("Ident Value", "12");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_NumberOfDigits2_With1234_ShouldDisplay12StarStar()
        {
            device.SetDisplay("Number Of Digits", "2");
            device.SetDisplay("Ident Value", "1234");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_NumberOfDigits2_With5_ShouldDisplay05StarStar()
        {
            device.SetDisplay("Number Of Digits", "2");
            device.SetDisplay("Ident Value", "5");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_NumberOfDigits1_With1234_ShouldDisplay1StarStarStar()
        {
            device.SetDisplay("Number Of Digits", "1");
            device.SetDisplay("Ident Value", "1234");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_NumberOfDigits0_With1234_ShouldDisplayAllStars()
        {
            device.SetDisplay("Number Of Digits", "0");
            device.SetDisplay("Ident Value", "1234");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_NumberOfDigits4_With1234_ShouldDisplay1234()
        {
            device.SetDisplay("Number Of Digits", "4");
            device.SetDisplay("Ident Value", "1234");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_NumberOfDigits5_ShouldClampTo4()
        {
            device.SetDisplay("Number Of Digits", "5");
            device.SetDisplay("Ident Value", "1234");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_NumberOfDigitsNegative_ShouldClampTo0()
        {
            device.SetDisplay("Number Of Digits", "-1");
            device.SetDisplay("Ident Value", "1234");

            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_ChangeDigitsResetsCache_AllowsResendingSameValue()
        {
            device.SetDisplay("Ident Value", "1234");
            int afterFirst = mockMessageSender.DisplayCommandsSent.Count;

            device.SetDisplay("Number Of Digits", "2");
            device.SetDisplay("Ident Value", "1234");
            int afterSecond = mockMessageSender.DisplayCommandsSent.Count;

            Assert.AreEqual(1, afterFirst);
            Assert.AreEqual(2, afterSecond);

            List<byte[]> expectedFirstCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            List<byte[]> expectedSecondCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedFirstCommands);
            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[1].Commands, expectedSecondCommands);
        }

        #endregion

        #region Annunciator Light Tests

        [TestMethod]
        public void SetDisplay_AnnunciatorLight_WithOne_ShouldTurnOnAllLights()
        {
            device.SetDisplay("LCD Test On/Off", "1");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        [TestMethod]
        public void SetDisplay_AnnunciatorLight_WithZero_ShouldResetDisplay()
        {
            device.SetDisplay("LCD Test On/Off", "0");
            Assert.AreEqual(1, mockMessageSender.DisplayCommandsSent.Count);

            List<byte[]> expectedCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedCommands);
        }

        #endregion

        #region Caching Tests

        [TestMethod]
        public void SetDisplay_IdentValue_WithSameValue_ShouldNotSendCommandTwice()
        {
            device.SetDisplay("Ident Value", "1234");
            int firstCount = mockMessageSender.DisplayCommandsSent.Count;

            device.SetDisplay("Ident Value", "1234");
            int secondCount = mockMessageSender.DisplayCommandsSent.Count;

            Assert.AreEqual(firstCount, secondCount);
        }

        [TestMethod]
        public void SetDisplay_IdentValue_WithDifferentValue_ShouldSendCommandTwice()
        {
            device.SetDisplay("Ident Value", "1234");
            int firstCount = mockMessageSender.DisplayCommandsSent.Count;

            device.SetDisplay("Ident Value", "5678");
            int secondCount = mockMessageSender.DisplayCommandsSent.Count;

            Assert.IsTrue(secondCount > firstCount);

            List<byte[]> expectedFirstCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            List<byte[]> expectedSecondCommands = new List<byte[]>()
            {
                new byte[] { 0x81, 0xBB, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RefreshCommand
            };

            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[0].Commands, expectedFirstCommands);
            CompareDisplayCommands(mockMessageSender.DisplayCommandsSent[1].Commands, expectedSecondCommands);
        }

        [TestMethod]
        public void SetDisplay_WithNullOrWhiteSpace_ShouldNotSendCommand()
        {
            device.SetDisplay("Ident Value", null);
            device.SetDisplay("Ident Value", "");
            device.SetDisplay("Ident Value", "   ");

            Assert.AreEqual(0, mockMessageSender.DisplayCommandsSent.Count);
        }

        #endregion

        #region Brightness Tests

        [TestMethod]
        public void SetLed_BacklightBrightness_ShouldSetBrightness()
        {
            device.SetLed("Backlight Percentage", 50);

            Assert.IsTrue(mockMessageSender.BrightnessCommands.Count > 0);
            var cmd = mockMessageSender.BrightnessCommands[0];
            Assert.AreEqual(0x00, cmd.Type);
        }

        [TestMethod]
        public void SetLed_LcdBrightness_ShouldSetBrightness()
        {
            device.SetLed("LCD Percentage", 75);

            Assert.IsTrue(mockMessageSender.BrightnessCommands.Count > 0);
            var cmd = mockMessageSender.BrightnessCommands[0];
            Assert.AreEqual(0x01, cmd.Type);
        }

        [TestMethod]
        public void SetLed_LedBrightness_ShouldSetBrightness()
        {
            device.SetLed("LED Percentage", 100);

            Assert.IsTrue(mockMessageSender.BrightnessCommands.Count > 0);
            var cmd = mockMessageSender.BrightnessCommands[0];
            Assert.AreEqual(0x02, cmd.Type);
        }

        #endregion

        #region Helpers

        private void CompareDisplayCommands(List<byte[]> sentCommands, List<byte[]> expectedCommands)
        {
            Assert.AreEqual(expectedCommands.Count, sentCommands.Count);

            for (int i = 0; i < expectedCommands.Count; i++)
            {
                CollectionAssert.AreEqual(expectedCommands[i], sentCommands[i]);
            }
        }

        #endregion

        #region Mock Implementation

        private class MockWinwingMessageSender : IWinwingMessageSender
        {
            public List<DisplayCommandMessage> DisplayCommandsSent { get; } = new List<DisplayCommandMessage>();
            public List<LightControlMessage> LightControlCommands { get; } = new List<LightControlMessage>();
            public List<BrightnessMessage> BrightnessCommands { get; } = new List<BrightnessMessage>();
            public List<byte[]> CduDisplayBytes { get; } = new List<byte[]>();
            public int HeartBeatMessageCount { get; private set; }
            public int RequestFirmwareMessageCount { get; private set; }
            public bool IsConnectedValue { get; set; }

            public void Reset()
            {
                DisplayCommandsSent.Clear();
                LightControlCommands.Clear();
                BrightnessCommands.Clear();
                CduDisplayBytes.Clear();
                HeartBeatMessageCount = 0;
                RequestFirmwareMessageCount = 0;
            }

            public bool IsConnected()
            {
                return IsConnectedValue;
            }

            public void Connect()
            {
                IsConnectedValue = true;
            }

            public void Shutdown()
            {
                IsConnectedValue = false;
            }

            public void SendDisplayCommands(IList<byte[]> commands)
            {
                DisplayCommandsSent.Add(new DisplayCommandMessage
                {
                    Commands = commands.Select(c => (byte[])c.Clone()).ToList()
                });

                Console.WriteLine("SendDisplayCommands called with {0} command(s):", commands.Count);
                for (int i = 0; i < commands.Count; i++)
                {
                    var bytes = commands[i];
                    var hexValues = string.Join(", ", bytes.Select(b => string.Format("0x{0:X2}", b)));
                    Console.WriteLine("  Command {0}: new byte[] {{ {1} }}", i, hexValues);
                }
                Console.WriteLine();
            }

            public void SendCduDisplayBytes(byte[] byteList)
            {
                CduDisplayBytes.Add((byte[])byteList.Clone());
            }

            public void SendLightControlMessage(byte[] destination, byte type, byte value)
            {
                LightControlCommands.Add(new LightControlMessage
                {
                    Destination = (byte[])destination.Clone(),
                    Type = type,
                    Value = value
                });
            }

            public void SetBrightness(byte[] destinationAddress, byte type, string brightness)
            {
                BrightnessCommands.Add(new BrightnessMessage
                {
                    DestinationAddress = (byte[])destinationAddress.Clone(),
                    Type = type,
                    Brightness = brightness
                });
            }

            public void SetBrightness(byte[] destinationAddress, byte type, int brightness)
            {
                BrightnessCommands.Add(new BrightnessMessage
                {
                    DestinationAddress = (byte[])destinationAddress.Clone(),
                    Type = type,
                    Brightness = brightness.ToString()
                });
            }

            public void SetVibration(byte[] destinationAddress, byte type, byte level)
            {
                // Not used by TCAS device
            }

            public void SetPulseLight(byte[] destinationAddress, bool isOn)
            {
                // Not used by TCAS device
            }

            public void SendHeartBeatMessage()
            {
                HeartBeatMessageCount++;
            }

            public void SendRequestFirmwareMessage()
            {
                RequestFirmwareMessageCount++;
            }
        }

        public class DisplayCommandMessage
        {
            public List<byte[]> Commands { get; set; }
        }

        public class LightControlMessage
        {
            public byte[] Destination { get; set; }
            public byte Type { get; set; }
            public byte Value { get; set; }
        }

        public class BrightnessMessage
        {
            public byte[] DestinationAddress { get; set; }
            public byte Type { get; set; }
            public string Brightness { get; set; }
        }

        #endregion
    }
}
