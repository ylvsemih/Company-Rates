using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace CompanyRatesTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            prepare_PrepareForTestCases();
            bool nr1Clicked = false;
            bool nr2Clicked = false;
            bool isPrepared = true;

            do
            {
                Console.WriteLine("Type 0 to prepare environment for running test cases");
                Console.WriteLine("Type 1 for AddNewCompanyAndApprove test [TEST CASE 1]");
                Console.WriteLine("Type 2 for AddReviewToCompanyAndRateCompany test [TEST CASE 2]");
                var number = Console.ReadLine();
                if (int.Parse(number) == 1)
                {
                    if (isPrepared)
                    {
                        nr1Clicked = true;
                        test1_addNewCompanyAndApprove();
                        isPrepared = false;
                    }
                    else
                    {
                        Console.WriteLine("First prepare the environment!");
                    }
                }
                else if (int.Parse(number) == 2)
                {
                    if (nr1Clicked == false || isPrepared)
                    {
                        Console.WriteLine("First run the test number 1!");
                    }
                    else if (nr2Clicked)
                        Console.WriteLine("Prepare environement and run tests again!");
                    else
                    {
                        test2_addReviewToCompanyAndRateCompany();
                        isPrepared = false;
                        nr2Clicked = true;
                    }
                }
                else if (int.Parse(number) == 0)
                {
                    if (isPrepared)
                    {
                        Console.WriteLine("Environment is already prepared. Run the 1st test!");
                    }
                    else
                    {
                        prepare_PrepareForTestCases();
                        isPrepared = true;
                        nr2Clicked = false;
                        nr1Clicked = false;
                        Console.WriteLine("Environment prepared. You can run the 1st test now!");
                    }
                }
                Console.WriteLine("______________________________________________________________________");
            } while (true);
        }

        private async static void prepare_PrepareForTestCases()
        {
            //RestClient client = new RestClient("https://companyratesapi-prod.eu-west-2.elasticbeanstalk.com/api/accounts/removeall");
            //var request = new RestRequest(Method.DELETE);
            //var response = client.Execute(request);

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("https://companyratesapi.azurewebsites.net/api/accounts/removeall")
            };
            HttpClient httpClient = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (
                object s,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors
            )
            {
                return true;
            };
            await httpClient.SendAsync(request);
        }

        private static void test1_addNewCompanyAndApprove()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://tadejrola.github.io/companyrates-client/");
            driver.FindElement(By.LinkText("Registration")).Click();
            driver.FindElement(By.LinkText("Register COMPANY account")).Click();
            driver.FindElement(By.Id("txt_email")).Clear();
            driver.FindElement(By.Id("txt_email")).SendKeys("microsoft@company.com");
            driver.FindElement(By.Id("txt_name")).Clear();
            driver.FindElement(By.Id("txt_name")).SendKeys("Microsoft");
            driver.FindElement(By.Id("txt_website")).Clear();
            driver.FindElement(By.Id("txt_website")).SendKeys("www.microsoft.com");
            driver.FindElement(By.Id("txt_address")).Clear();
            driver.FindElement(By.Id("txt_address")).SendKeys("Gedemino 21");
            driver.FindElement(By.Id("btn_register")).Click();
            driver.FindElement(By.Id("link_profile")).Click();
            driver.FindElement(By.Id("btn_logout")).Click();
            driver.FindElement(By.Id("txt_email")).Clear();
            driver.FindElement(By.Id("txt_email")).SendKeys("admin@admin.com");
            driver.FindElement(By.Id("txt_pass")).Clear();
            driver.FindElement(By.Id("txt_pass")).SendKeys("admin");
            driver.FindElement(By.Id("btn_login")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='www.microsoft.com'])[1]/following::i[1]")).Click();
            driver.FindElement(By.Id("btn_approve")).Click();
            Thread.Sleep(500);
            bool loaded = false;
            do
            {
                driver.FindElement(By.Id("link_profile")).Click();
                if (driver.Url.Contains("Profile"))
                {
                    loaded = true;
                }
            } while (loaded == false);

            driver.FindElement(By.Id("btn_logout")).Click();
            driver.Close();
        }

        private static void test2_addReviewToCompanyAndRateCompany()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://tadejrola.github.io/companyrates-client/");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='www.microsoft.com'])[1]/following::i[1]")).Click();
            driver.FindElement(By.Id("btn_openModalAddReview")).Click();

            driver.FindElement(By.Id("txt_email")).Clear();

            driver.FindElement(By.Id("txt_email")).SendKeys("user1@user.com");
            driver.FindElement(By.Id("txt_pass")).Clear();
            driver.FindElement(By.Id("txt_pass")).SendKeys("user");
            driver.FindElement(By.Id("btn_login")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='www.microsoft.com'])[1]/following::i[1]")).Click();

            driver.FindElement(By.Id("btn_openModalAddReview")).Click();

            driver.FindElement(By.Id("txt_review")).Clear();
            driver.FindElement(By.Id("txt_review")).SendKeys("Good company!");
            driver.FindElement(By.Id("DDL_category")).Click();
            new SelectElement(driver.FindElement(By.Id("DDL_category"))).SelectByText("Good opinion");
            driver.FindElement(By.Id("DDL_category")).Click();
            driver.FindElement(By.Id("btn_addReview")).Click();
            bool loaded = false;
            do
            {
                try
                {
                    driver.FindElement(By.Id("btn_compnay_thumbsup")).Click();
                    Thread.Sleep(500);
                    loaded = true;
                }
                catch
                {
                }
            } while (loaded == false);

            loaded = false;
            do
            {
                driver.FindElement(By.Id("link_profile")).Click();
                if (driver.Url.Contains("Profile"))
                {
                    loaded = true;
                }
            } while (loaded == false);

            driver.FindElement(By.Id("btn_logout")).Click();

            driver.Close();
        }
    }
}