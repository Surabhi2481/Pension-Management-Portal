using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pension_Management_Portal.Models;

namespace Pension_Management_Portal.Controllers
{
    public class HomeController : Controller
    {
        static string token;
        PensionDetail penDetailObj = new PensionDetail();
        private readonly DataContext _context;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(HomeController));
        private IConfiguration configuration;
        /// <summary>
        /// Dependency Injection
        /// </summary>
        public HomeController(DataContext context, IConfiguration iConfig)
        {
            _context = context;
            configuration = iConfig;

        }
        /// <summary>
        /// Redirection to login page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            _log4net.Info("Login page is Invoked!!");
            return View();
        }
        /// <summary>
        /// Getting the token a Validating the User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(User user)
        {
            string tokenValue = configuration.GetValue<string>("MyLinkValue:tokenUri");
            token = GetToken(tokenValue, user);
            if (token=="abcd")
            {
                _log4net.Error("Authenticate Microservice is Down!!");
                ViewBag.loginerror = "Error Occured";
                return View();
            }
            if (token != null)
            {
                _log4net.Info("Login is Done!!");
                return RedirectToAction("PensionerValues");
            }
            else
            {
                _log4net.Warn("Invalid Credientails are given by Admin!!");
                ViewBag.invalid = "UserName or Password invalid";
                return View();
            }
        }
        static string GetToken(string url, User user)
        {
            string token = "abcd";
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync(url, data).Result;
                    string name = response.Content.ReadAsStringAsync().Result;
                    dynamic details = JObject.Parse(name);
                    return details.token;
                }
            }
            catch(Exception e)
            {
                return token;
            }
        }

        /// <summary>
        /// Getting the values of the Pensioner
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult PensionerValues()
        {
            _log4net.Info("Admin is giving the Pensioner Details!!");
            return View();
        }

        /// <summary>
        /// Validaing the values of the Pensioner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PensionerValues(PensionerInput input)
        {

            // string status;
            string processValue = configuration.GetValue<string>("MyLinkValue:processUri");

            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

                    client.BaseAddress = new Uri(processValue);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await client.PostAsync("api/ProcessPension/ProcessPension", content))    //Get Post Mai check kar lena
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            Result res = JsonConvert.DeserializeObject<Result>(apiResponse);
                            penDetailObj = res.result;
                        }
                    }
                    catch(Exception e)
                    {
                        _log4net.Error("Some Microservice is Down!!");
                        penDetailObj = null;
                    }

                }

                if (penDetailObj == null)
                {
                    _log4net.Error("Some Microservice is Down!!");
                    ViewBag.erroroccured = "Some Error Occured";
                    return View();
                }
                if(penDetailObj.Status.Equals(20))
                {
                    _log4net.Error("Some Microservice is Down!!");
                    ViewBag.erroroccured = "Some Error Occured";
                    return View();
                }
                if (penDetailObj.Status.Equals(10))
                {
                    // Storing the Values in Database
                    _log4net.Info("Pensioner details have been matched with the Csv and data is successfully saved in local Database!!");
                    _context.pensionDetails.Add(penDetailObj);
                    _context.SaveChanges();
                    return RedirectToAction("PensionervaluesDisplayed", penDetailObj);
                }
                else
                {
                    _log4net.Error("Persioner details does not match with the Csv!!");
                    ViewBag.notmatch = "Pensioner Values not match";
                    return View();
                }

            }

            else
            {
                _log4net.Warn("Proper details are not given by the Admin!!");
                ViewBag.invalid = "Pensioner Values are Invalid";
                return View();
            }

        }
        /// <summary>
        /// Displaying the Pension Amount 
        /// </summary>
        /// <param name="penObj"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PensionervaluesDisplayed(PensionDetail penObj)
        {
            _log4net.Info("Pension Amount of Pensioner is Displayed!!");
            return View(penObj);


        }

    }
}
