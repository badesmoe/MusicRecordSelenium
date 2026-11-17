using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace MusicRecordSelenium
{
    [TestClass]
    public sealed class MusicRecordSeleniumTest
    {
        private IWebDriver _driver;

        [TestInitialize]
        public void Setup()
        {
            _driver = new ChromeDriver();
            Login("adminny", "Admin123!");
        }
        private void Login(string username, string password)
        {
            _driver.Navigate().GoToUrl("http://127.0.0.1:5500");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            Thread.Sleep(1000);

            wait.Until(d => d.FindElement(By.Id("Username"))).SendKeys(username);
            Thread.Sleep(1000);

            _driver.FindElement(By.Id("Password")).SendKeys(password);
            Thread.Sleep(1000);

            _driver.FindElement(By.Id("LoginButton")).Click();
        }

        [TestMethod]
        public void SearchButton_ButtonClick_ShouldNavigateToSearchResults()
        {
            // Arrange
            Thread.Sleep(1000);

            // Act
            IWebElement title = _driver.FindElement(By.Id("Title"));
            IWebElement searchButton = _driver.FindElement(By.Id("SearchButton"));
            title.SendKeys("Purp");
            Thread.Sleep(1000);

            searchButton.Click();
            Thread.Sleep(1000);

            // Assert
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            var items = wait.Until(d =>
            {
                var list = d.FindElements(By.CssSelector(".record-grid li"));
                return list.Count > 0 ? list : null;
            });

            Assert.IsTrue(items.Any(i => i.Text.Contains("Prince")));
        }

        [TestMethod]
        public void SearchButton_ButtonClick_ReturnsAllResultsFromArtist()
        {
            // Arrange
            Thread.Sleep(1000);

            // Act
            IWebElement artist = _driver.FindElement(By.Id("Artist"));
            IWebElement searchButton = _driver.FindElement(By.Id("SearchButton"));
            artist.SendKeys("Adele");
            Thread.Sleep(1000);

            searchButton.Click();
            Thread.Sleep(1000);

            // Assert
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            var items = wait.Until(d =>
            {
                var list = d.FindElements(By.CssSelector(".record-grid li"));
                return list.Count > 0 ? list : null;
            });

            Assert.IsTrue(items.Any(i => i.Text.Contains("Adele")));
            Assert.IsTrue(items.Count() > 1);
        }

        [TestMethod]
        public void LogoutButton_ButtonClick_ShouldNavigateToLoginPage()
        {
            // Arrange
            Thread.Sleep(1000);
            // Act
            IWebElement logoutButton = _driver.FindElement(By.Id("LogoutButton"));
            logoutButton.Click();
            Thread.Sleep(1000);
            // Assert
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            var loginPage = wait.Until(d => d.FindElement(By.Id("LoginButton")));
            Assert.IsNotNull(loginPage);
        }

        [TestCleanup]
        public void Cleanup() => _driver.Quit();


    }
}
