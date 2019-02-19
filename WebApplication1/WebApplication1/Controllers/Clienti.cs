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
            return M.ReadSerie(connString, factory, serieName);
        }

        [HttpGet] // in esecuzione solo con un get dal client
        [Route("arimaForecast/{serieName}")] // nome del metodo esposto nella API
        public string ArimaForecast(string serieName)
        {
            string serie = M.ReadSerie(connString, factory, serieName);
            ArimaForecast arima = new ArimaForecast(serie, 0, 1);

            return arima.forecastComputation(); 
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

        /*
        
        
        [HttpGet]
        [Route("readGAPinstance/{instance}")]
        public IHttpActionResult ReadGAPinstance(string instance)
        {
            string res;
            string pathjson = GetJsonPath(instance);
            GAP = M.ReadGAPInstance(pathjson);
            res = GAP.name;
            return Ok(res);
        }
        
        [HttpGet] // in esecuzione solo con un get dal client
        [ActionName("GetAllClients")] // nome del metodo esposto nella API
        public string GetAllClients()
        {
            string res;
            Models.GAPinstance GAP = new Models.GAPinstance();
            Models.Model M = new Models.Model();
            string pathjson = @"C:\Users\monyg\source\repos\WebApplication1\WebApplication1\App_Data\toy.json";
            GAP = M.ReadGAPInstance(pathjson);
            res = GAP.name;
            return res;
        }

        [HttpGet] // in esecuzione solo con un get dal client
        [ActionName("GetCustQuantities")] // nome del metodo esposto
        public IHttpActionResult GetCustQuantities(int id)
        {
            var user = "{\"id\":" + id + "}";

            if (user == null)
                return NotFound();
            return Ok(user);
        }

        public string getItem(string id)
        {
            string res = "non trovato";

            res = "{\"id\":" + id + "}";

            return res;
        }
         
         
        public string PostSomething(object obj)
        {
            string jStr = Convert.ToString(obj);
            var obj1 = JsonConvert.DeserializeObject<dynamic>(jStr);
            int id = obj1.id;
            string dato = Convert.ToString(obj1.dato);
            return "done";
        }



        public int execNonQueryViaFactory(string connString, string queryText, string factory)
        {
            int numRows = 0;
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(factory);

            using (DbConnection connection = dbFactory.CreateConnection())
            {
                try
                {
                    connection.ConnectionString = connString;
                    connection.Open();
                    IDbCommand cmd = connection.CreateCommand();
                    cmd.CommandText = queryText;

                    numRows = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
                return numRows;
            }
        }
         
         */

    }
}