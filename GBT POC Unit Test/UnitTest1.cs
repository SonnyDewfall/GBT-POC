using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace GBT_POC_Unit_Test
{
    [TestClass]
    public class WpfTest
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        //private const string WpfAppId = @"C:\Box\Repos\GBT POC\bin\Debug\GBT POC.exe";
        private const string WpfAppId = @"5d056c6e-43f4-44cb-a1da-23544284b815_mn7m64sz1b7aa";

        protected static WindowsDriver<WindowsElement> session;
        protected static WindowsDriver<WindowsElement> DesktopSession;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            if (session == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", WpfAppId);
                appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                //session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
                try
                {
                    Console.WriteLine("Trying to Launch App");
                    DesktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
                }
                catch
                {
                    Console.WriteLine("Failed to attach to app session (expected).");
                }

                appiumOptions.AddAdditionalCapability("app", "Root");
                DesktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
                var mainWindow = DesktopSession.FindElementByAccessibilityId("WpfUITestingMainWindow");
                var mainWindowHandle = mainWindow.GetAttribute("NativeWindowHandle");
                mainWindowHandle = (int.Parse(mainWindowHandle)).ToString("x"); // Convert to Hex
                appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("appTopLevelWindow", mainWindowHandle);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
            }
        }

        [TestInitialize]
        public void Clear()
        {
            var txtName = session.FindElementByAccessibilityId("txtName");
            txtName.Clear();
        }

        [TestMethod]
        public void AddNameToTextBox()
        {
            var txtName = session.FindElementByAccessibilityId("txtName");
            txtName.SendKeys("Sonny");
            session.FindElementByAccessibilityId("sayHelloButton").Click();
            var txtResult = session.FindElementByAccessibilityId("txtResult");
            Assert.AreEqual(txtResult.Text, $"Hello {txtName.Text}");
        }

        [TestMethod]
        public void AddWrongNameToTextBox()
        {
            var txtName = session.FindElementByAccessibilityId("txtName");
            txtName.SendKeys("Matteo");
            session.FindElementByAccessibilityId("sayHelloButton").Click();
            var txtResult = session.FindElementByAccessibilityId("txtResult");
            Assert.AreEqual(txtResult.Text, $"Hello Matt");
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            if (session != null)
            {
                session.Close();
                session.Quit();
            }
        }
    }

}
