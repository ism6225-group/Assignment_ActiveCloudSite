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

        public IActionResult Stocks()
        {
            List<Symbol> symbols = ReadSymbols();
            return View(symbols);
        }

        public IActionResult Symbols()
        {
            List<Symbol> symbols = ReadSymbols();
            return View(symbols);
        }

        public IActionResult Market()
        {
            List<Article> marketNews = ReadLatestMarketNews();
            return View(marketNews);
        }

        public IActionResult Recommendation(string symbol = "aapl44")
        {
            List<Recommendation> recommendations = ReadRecommendations(symbol);
            return View(recommendations);
        }

        public IActionResult Sectors()
        {
            List<Sector> sectors = ReadSectors();
            return View(sectors);
        }

        public IActionResult Quote(string symbol = "goog44")
        {
            Quote quote = ReadQuote(symbol);
            return View(quote);
        }

        public IActionResult About()
        {
            return View();
        }

        public List<Symbol> ReadSymbols(bool full = false)
        {
            List<Symbol> symbols = dbContext.Symbols.ToList();
            // To show the first 50 symbol, and save some loading time
            if (!full && symbols.Count() > 50)
            {
                symbols = symbols.GetRange(0, 50);
            }
            return symbols;
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
                // We return the symbol if it is enabled for trading on IEX and has a common issue type
                symbols = symbols.Where(s => s.isEnabled && s.type != "N/A").ToList();
            }

            return symbols;
        }

        public IActionResult UpdateSymbols()
        {
            List<Symbol> symbols = GetSymbols();

            foreach (Symbol aSymbol in symbols)
            {
                //Database will give PK constraint violation error when trying to insert record with existing PK.
                if (dbContext.Symbols.Where(s => s.symbol.Equals(aSymbol.symbol)).Count() == 0)
                {
                    dbContext.Symbols.Add(aSymbol);
                }
            }
            dbContext.SaveChanges();
            symbols = ReadSymbols();
            return View("Stocks", symbols);
        }

        // To clear Symbole table if we want to test
        public void DeleteSymbols()
        {
            dbContext.Symbols.RemoveRange(dbContext.Symbols);
            dbContext.SaveChanges();
        }

        public IActionResult FullSymbols()
        {
            List<Symbol> symbols = ReadSymbols(true);
            return View("Stocks", symbols);
        }

        public List<Article> ReadLatestMarketNews()
        {
            UpdateLatestMarketNews();
            List<Article> marketNews = dbContext.News.ToList();
            marketNews = marketNews.OrderByDescending(n => n.datetime).Take(6).ToList();
            return marketNews;
        }

        public List<Article> GetLatestMarketNews()
        {
            string MarketNews_API_PATH = BASE_URL_IEXT + "stock/market/news/last/10";
            string marketNewsList = "";
            List<Article> marketNews = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(MarketNews_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(MarketNews_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                marketNewsList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!marketNewsList.Equals(""))
            {
                marketNews = JsonConvert.DeserializeObject<List<Article>>(marketNewsList);
                foreach (Article article in marketNews)
                {
                    article.symbol = "market";
                }
            }
            return marketNews;
        }

        public void UpdateLatestMarketNews()
        {
            List<Article> marketNews = GetLatestMarketNews();

            foreach (Article article in marketNews)
            {
                //Database will give PK constraint violation error when trying to insert record with existing PK.
                if (dbContext.News.Where(s => s.datetime.Equals(article.datetime)).Count() == 0)
                {
                    dbContext.News.Add(article);
                }
            }
            dbContext.SaveChanges();
        }

        // To clear News table if we want to test
        public void DeleteLatestMarketNews()
        {
            dbContext.News.RemoveRange(dbContext.News);
            dbContext.SaveChanges();
        }

        public List<Recommendation> ReadRecommendations(string symbol)
        {
            UpdateRecommendations(symbol);
            List<Recommendation> recommendations = dbContext.Recommendations.Where(r => r.symbol.Equals(symbol)).ToList();
            return recommendations;
        }

        public List<Recommendation> GetRecommendations(string symbol)
        {
            string Recommendations_API_PATH = BASE_URL_IEXC + "stock/" + symbol + "/recommendation-trends?token=pk_541e757bbc83419ba3e8924016f8bce9";
            string recommendationsList = "";
            List<Recommendation> recommendations = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(Recommendations_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(Recommendations_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                recommendationsList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!recommendationsList.Equals(""))
            {
                // To return the current the consensus only
                recommendations = JsonConvert.DeserializeObject<List<Recommendation>>(recommendationsList);
                foreach (Recommendation recommendation in recommendations)
                {
                    recommendation.symbol = symbol;
                }
                recommendations = recommendations.Where(r => r.consensusEndDate == null).ToList();
            }
            return recommendations;
        }

        public void UpdateRecommendations(string symbol)
        {
            List<Recommendation> recommendations = GetRecommendations(symbol);
            if (recommendations != null)
            {

            foreach (Recommendation recommendation in recommendations)
            {
                //Database will give PK constraint violation error when trying to insert record with existing PK.
                if (dbContext.Recommendations.Where(r => r.symbol.Equals(recommendation.symbol)).Count() == 0)
                {
                    dbContext.Recommendations.Add(recommendation);
                }
                else
                {
                    // Remove old recommendations
                    dbContext.Recommendations.RemoveRange(dbContext.Recommendations.Where(r => r.symbol.Equals(recommendation.symbol)));
                    dbContext.Recommendations.Add(recommendation);
                }
            }
            }
            dbContext.SaveChanges();
        }

        public List<Sector> ReadSectors()
        {
            UpdateSectors();
            List<Sector> sectors = dbContext.Sectors.ToList();
            return sectors;
        }

        public List<Sector> GetSectors()
        {
            string Sectors_API_PATH = BASE_URL_IEXT + "stock/market/sector-performance";
            string sectorsList = "";
            List<Sector> sectors = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(Sectors_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(Sectors_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                sectorsList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!sectorsList.Equals(""))
            {
                // To return the current the consensus only
                sectors = JsonConvert.DeserializeObject<List<Sector>>(sectorsList);
            }
            return sectors;
        }

        public void UpdateSectors()
        {
            List<Sector> sectors = GetSectors();

            foreach (Sector sector in sectors)
            {
                //Database will give PK constraint violation error when trying to insert record with existing PK.
                if (dbContext.Sectors.Where(r => r.name.Equals(sector.name)).Count() == 0)
                {
                    dbContext.Sectors.Add(sector);
                }
                else
                {
                    // Remove old Sectors
                    dbContext.Sectors.RemoveRange(dbContext.Sectors.Where(r => r.name.Equals(sector.name)));
                    dbContext.Sectors.Add(sector);
                }
            }
            dbContext.SaveChanges();
        }

        public Quote ReadQuote(string symbol)
        {
            Quote quote = null;
            UpdateQuotes(symbol);
            List<Quote> quotes = dbContext.Quotes.Where(r => r.symbol.Equals(symbol)).ToList();
            if (quotes.Count() != 0)
            {
                quote = quotes.First();
            }
            //Quote quote = GetQuote(symbol);
            return quote;
        }

        public Quote GetQuote(string symbol)
        {
            string Quote_API_PATH = BASE_URL_IEXT + "stock/" + symbol + "/quote";
            string responseData = "";
            Quote quote = new Quote();

            HttpResponseMessage response = httpClient.GetAsync(Quote_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            if (!string.IsNullOrEmpty(responseData))
            {
                quote = JsonConvert.DeserializeObject<Quote>(responseData);
            }
            else
            {
                quote = null;
            }
            return quote;
        }

        public void UpdateQuotes(string symbol)
        {
            Quote quote = GetQuote(symbol);
            if (quote != null)
            {
                if (dbContext.Quotes.Where(r => r.symbol.Equals(quote.symbol)).Count() == 0)
                {
                    dbContext.Quotes.Add(quote);
                }
                else
                {
                    // Remove old Quotes
                    dbContext.Quotes.RemoveRange(dbContext.Quotes.Where(r => r.symbol.Equals(quote.symbol)));
                    dbContext.Quotes.Add(quote);
                }
                dbContext.SaveChanges();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
