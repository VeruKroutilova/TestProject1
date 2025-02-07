using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace TestProject1
{
    internal class TeamsFirefoxTests : CommonTests
    {
        [SetUp] // Metoda se provede pred kazdym testem
        public void Setup()
        {
            SetupBase();
            // Definovat FirefoxDriver
            var options = new FirefoxOptions();
            options.SetPreference("network.cookie.cookieBehavior", 0); // Povolit cookies
            driver = new FirefoxDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Prejit na Teams
            driver.Navigate().GoToUrl(teamsUrl);
            
            // Prihlasit se
            LoginToTeams();
        }

        [Test]
        public void SendTwoFilesFromOneDrive_FilesSent()
        {
            // Kliknout na plus
            driver.FindElement(By.CssSelector("button[data-tid='sendMessageCommands-message-extension-flyout-command']")).Click();

            // Kliknut na pripojit soubor
            driver.FindElement(By.CssSelector("[data-tid='flyout-list-item']")).Click();

            // Kliknut na prilozit z OneDrive
            driver.FindElement(By.CssSelector("[role='menuitem']")).Click();
            Thread.Sleep(2000);  // Čekání na načtení OneDrive

            // Najit a prepnout se do iframe
            var iframeElement = driver.FindElement(By.XPath("//iframe[@aria-label='Opening file picker']"));
            driver.SwitchTo().Frame(iframeElement);

            // Najit a vybrat prvni soubor
            var firstFile = driver.FindElement(By.CssSelector("[data-automationid='row-selection-SPO@00888abf-6921-4e17-a5c0-aeacf2545961,nI59utEJVU-RTu8IH5oRZzqtjz1-XEhFmRdcvk0FA4Tis6Ato8EDQ7cwW7607v669JaJQa1P006T4cLv_PkvrQ']"));
            firstFile.Click();

            // Najit a vybrat druhy soubor
            var secondFile = driver.FindElement(By.CssSelector("[data-automationid='row-selection-SPO@00888abf-6921-4e17-a5c0-aeacf2545961,s-xUt2nB90yg8Oiid1PNrDqtjz1-XEhFmRdcvk0FA4Tis6Ato8EDQ7cwW7607v669JaJQa1P006T4cLv_PkvrQ']"));
            secondFile.Click();

            // Potvrdit vyber souboru
            driver.FindElement(By.CssSelector("[data-automationid='picker-complete']")).Click();
            Thread.Sleep(2000);

            // Po skonceni se vratit do hlavniho okna
            driver.SwitchTo().DefaultContent();

            // Kliknout na odeslat
            driver.FindElement(By.CssSelector("button[data-tid='newMessageCommands-send']")).Click();

            // Overeni, ze soubory byly odeslany
            var fileElement = driver.FindElement(By.XPath($"//div[@aria-label='The message has 2 attachments.']"));
            Assert.That(fileElement.Displayed, Is.True, "Přílohy nenalezeny");
        }

        [Test]
        public void SendThreeMessagesToChat_MessagesSent()
        {
            // Vlozit zpravy do promenne
            var messages = new[] { "První zpráva", "Druhá zpráva", "Třetí zpráva" };

            // Najit elementy
            IWebElement messageBox = driver.FindElement(By.XPath("//div[@aria-label='Type a message']"));
            IWebElement sendButton = driver.FindElement(By.CssSelector("button[data-tid='sendMessageCommands-send']"));

            // Odeslat zpravy v cyklu
            foreach (var message in messages)
            {
                messageBox.SendKeys(message);
                sendButton.Click();
                Thread.Sleep(2000);
            }

            // Overeni, ze zpravy byly odeslany
            foreach (var message in messages)
            {
                var sentMessage = driver.FindElement(By.XPath($"//div[@aria-label='{message}']"));
                Assert.That(sentMessage.Displayed, Is.True, $"Zpráva '{message}' nenalezena");
            }
        }

        [TearDown] // Spusti na konci kazdeho testu
        public void TearDown()
        {
            TearDownBase();
        }
    }
}