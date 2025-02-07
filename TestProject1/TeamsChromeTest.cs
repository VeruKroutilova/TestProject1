using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestProject1
{
    public class TeamsChromeTest : CommonTests
    {
        [SetUp] // Metoda se provede pred kazdym testem
        public void Setup()
        {
            SetupBase();
            // Definovat ChromeDriver
            var options = new ChromeOptions();
            options.AddUserProfilePreference("profile.default_content_setting_values.cookies", 1); // Povolit cookies
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Prejit na Teams
            driver.Navigate().GoToUrl(teamsUrl);

            // Prihlasit se
            LoginToTeams();
        }

        [Test]
        public void SendFileFromOneDrive_FileSent()
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

            // Najit a vybrat konkretni soubor
            var file = driver.FindElement(By.CssSelector("[data-automationid='row-selection-SPO@00888abf-6921-4e17-a5c0-aeacf2545961,nI59utEJVU-RTu8IH5oRZzqtjz1-XEhFmRdcvk0FA4Tis6Ato8EDQ7cwW7607v669JaJQa1P006T4cLv_PkvrQ']"));
            file.Click();

            // Potvrdit vyber souboru
            driver.FindElement(By.CssSelector("[data-automationid='picker-complete']")).Click(); 
            Thread.Sleep(2000);  

            // Po skonceni se vratit do hlavniho okna
            driver.SwitchTo().DefaultContent();

            // Napsat zpravu
            string actualTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var message = $"{actualTime}"; //unikatni zprava
            IWebElement messageBox = driver.FindElement(By.XPath("//div[@aria-label='Type a message']"));
            messageBox.SendKeys(message);

            // Kliknout na odeslat
            driver.FindElement(By.CssSelector("button[data-tid='newMessageCommands-send']")).Click();

            // Overeni, ze byla odeslana zprava
            var sendMessage = driver.FindElement(By.XPath($"//div[@aria-label='{message}']"));
            Assert.That(sendMessage.Displayed, Is.True, "Zpráva nenalezena");

            // Ziskani id zpravy
            var id = sendMessage.GetAttribute("id").Replace("content-", "");

            // Overeni, ze byla prilozena priloha
            var attachments = driver.FindElement(By.Id($"attachments-{id}"));
            Assert.That(attachments.Displayed, Is.True, "Příloha nenalezena");
        }

        [Test]
        public void SendMessageToChat_MessageSent()
        {
            // Napsani zpravy
            string actualTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var message = $"{actualTime}"; //unikatni zprava
            IWebElement messageBox = driver.FindElement(By.XPath("//div[@aria-label='Type a message']"));
            messageBox.SendKeys(message);

            // Odeslani zpravy
            IWebElement sendButton = driver.FindElement(By.CssSelector("button[data-tid='sendMessageCommands-send']"));
            sendButton.Click();

            // Overeni, ze byla zprava odeslana
            var sendMessage = driver.FindElement(By.XPath($"//div[@aria-label='{message}']"));
            Assert.That(sendMessage.Displayed, Is.True, "Zpráva nenalezena");
        }

        [TearDown] // Spusti na konci kazdeho testu
        public void TearDown()
        {
            TearDownBase();
        }
    }
}
