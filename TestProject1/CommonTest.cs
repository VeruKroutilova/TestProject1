namespace TestProject1
{
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using OpenQA.Selenium;
    using System;

    public class CommonTests
    {
        protected WebDriver driver;
        protected const string teamsUrl = "https://teams.microsoft.com/v2/";
        protected const string userName = "qa@safeticaservices.onmicrosoft.com";
        protected const string password = "automation.Safetica2004";

        public void SetupBase()
        {
            // Test se spustil
            Logger.Log("Test start: " + TestContext.CurrentContext.Test.Name);
        }

        public void TearDownBase()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var failedMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                // Test selhal, zprava o selhani
                Logger.LogError("Test failed: " + TestContext.CurrentContext.Test.Name);
                Logger.LogError($"Failed message: {failedMessage}");
            }
            else
            {
                // Test probehl uspesne
                Logger.LogSuccess("Test passed: " + TestContext.CurrentContext.Test.Name);
            }

            // Zavrit prohlizec po kazdym
            driver.Quit();
        }

        // Metoda pro prihlaseni do teams
        protected void LoginToTeams()
        {
            try
            {
                // Vlozit validni uzivatelske jmeno
                driver.FindElement(By.Id("i0116")).SendKeys(userName);
                driver.FindElement(By.Id("idSIButton9")).Click();
                Thread.Sleep(2000);  // Cekani na nacteni stranky

                // Vlozit validni heslo
                driver.FindElement(By.Id("i0118")).SendKeys(password);
                driver.FindElement(By.Id("idSIButton9")).Click();
                Thread.Sleep(2000);  // Cekani na nacteni stranky

                // Zustat prihlaseny
                driver.FindElement(By.Id("idSIButton9")).Click();
                Thread.Sleep(10000);  // Cekani na nacteni teams
            }
            catch (Exception ex)
            {
                // Prihlaseni selhalo
                Logger.LogError($"Login failed: {ex.Message}");
                throw;
            }
        }
    }
}