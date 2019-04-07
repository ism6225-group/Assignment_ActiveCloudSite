using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment_ActiveCloudSite.Models;
using Assignment_ActiveCloudSite.DataAccess;
using Newtonsoft.Json;
using System.Net.Http;

namespace Assignment_ActiveCloudSite.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDBContext dbContext;
        string BASE_URL_IEXT = "https://api.iextrading.com/1.0/";
        string BASE_URL_IEXC = "https://cloud.iexapis.com/beta/";
        HttpClient httpClient;

        public HomeController(ApplicationDBContext context)
        {
            dbContext = context;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Symbols()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public List<Symbol> GetSymbols()
        {
            string Symbols_API_PATH = BASE_URL_IEXT + "ref-data/symbols";
            string symbolsList = "";
            List<Symbol> symbols = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(Symbols_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(Symbols_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                symbolsList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!symbolsList.Equals(""))
            {
                symbols = JsonConvert.DeserializeObject<List<Symbol>>(symbolsList);
                symbols = symbols.Where(s => s.isEnabled && s.type != "N/A").ToList();
                //symbols = symbols.GetRange(0, 20);
            }

            return symbols;
        }

        public IActionResult ReadSymbols()
        {
            List<Symbol> symbols = dbContext.Symbols.ToList();
            return View("Symbols", symbols);
        }

        public IActionResult UpdateSymbols()
        {
            List<Symbol> symbols = GetSymbols();

            foreach (Symbol aSymbol in symbols)
            {
                //Database will give PK constraint violation error when trying to insert record with existing PK.
                //So add company only if it doesnt exist, check existence using symbol (PK)
                if (dbContext.Symbols.Where(s => s.symbol.Equals(aSymbol.symbol)).Count() == 0)
                {
                    dbContext.Symbols.Add(aSymbol);
                }
            }
            dbContext.SaveChanges();
            return View("Symbols", symbols);
        }

        public IActionResult DeleteSymbols()
        {
            dbContext.Symbols.RemoveRange(dbContext.Symbols);
            dbContext.SaveChanges();
            List<Symbol> symbols = dbContext.Symbols.ToList();
            return View("Symbols", symbols);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
         public IActionResult Error()
         {
             return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
         }
    } 
}
