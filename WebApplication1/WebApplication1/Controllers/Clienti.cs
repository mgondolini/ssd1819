using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json;
using System.IO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/Clienti")]
    public class ClientiController : ApiController
    {
        Models.GAPinstance GAP = new Models.GAPinstance();
        Models.Model M = new Models.Model();

        private readonly string connString = "Data Source=tcp:137.204.74.181;Initial Catalog = testDb;User ID=studSSD;Password=studSSD";
        private readonly string factory = "System.Data.SqlClient";

        [HttpGet] // in esecuzione solo con un get dal client
        [Route("readSerie/{serieName}")] // nome del metodo esposto nella API
        public string ReadSerie(string serieName)
        {
            return M.ReadSerie(connString, factory, serieName).ToString();
        }

        [HttpGet] // in esecuzione solo con un get dal client
        [Route("arimaForecast/{serieName}")] // nome del metodo esposto nella API
        public string ArimaForecast(string serieName)
        {
            string serie = M.ReadSerie(connString, factory, serieName);
            CSVwriter w = new CSVwriter(serieName, serie);
            w.createCSV();
            Forecast f = new Forecast(4, 0);
            return f.ArimaForecast();
        }

        [HttpGet]
        [Route("NNforecast/{serieName}")] 
        public string NNForecast(string serieName)
        {
            string serie = M.ReadSerie(connString, factory, serieName);
            CSVwriter w = new CSVwriter(serieName, serie);
            w.createCSV();
            Forecast f = new Forecast(4, 0);
            return f.NNForecast();
        }

        [HttpGet]
        [Route("constructSolution/{instance}")]
        public int ConstructSolution(string instance)
        {
            return M.ConstructSolution(GetGAP(instance));
        }

        [HttpGet]
        [Route("optimization/{instance}")]
        public int Optimization(string instance)
        {
            return M.Opt10(GetGAP(instance));
        }

        [HttpGet]
        [Route("simulatedAnnealing/{instance}")]
        public int SimulatedAnnealing(string instance)
        {
            return M.SimulatedAnnealing(GetGAP(instance));
        }

        [HttpGet]
        [Route("tabuSearch/{instance}")]
        public int TabuSearch(string instance)
        {
            return M.TabuSearch(GetGAP(instance));
        }

        public string GetJsonPath(string gap)
        {
            return (string)AppDomain.CurrentDomain.GetData("DataDirectory") + @"\" + gap + ".json"; ;
        }

        public GAPinstance GetGAP(string instance)
        {
            string jsonpath = GetJsonPath(instance);
            return GAP = M.ReadGAPInstance(jsonpath);
        }
    }
}