﻿using System;
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
    public class ClientiController : ApiController 
    {
        Models.GAPinstance GAP = new Models.GAPinstance();
        Models.Model M = new Models.Model();

        string connString = "Data Source=tcp:137.204.74.181;Initial Catalog = testDb;User ID=studSSD;Password=studSSD";
        string factory = "System.Data.SqlClient";

        BasicHeu basicHeu;

        public delegate void viewEventHandler(object sender, string textToWrite);
        public event viewEventHandler FlushText;

        [HttpGet] // in esecuzione solo con un get dal client
        [ActionName("GetAllClients")] // nome del metodo esposto nella API

        public string GetAllClients()
        {
            string res;
            Models.GAPinstance GAP = new Models.GAPinstance();
            Models.Model M = new Models.Model();
            string pathjson = @"C:\Users\monyg\source\repos\WebApplication1\WebApplication1\App_Data\toy.json";
            GAP = M.readGAPInstance(pathjson);
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

        [HttpGet] 
        [ActionName("readGAPinstance")] 
        public string readGAPinstance()
        {
            string res;
            string pathjson = @"C:\Users\monyg\source\repos\WebApplication1\WebApplication1\App_Data\toy.json";
            GAP = M.readGAPInstance(pathjson);
            res = GAP.name;
            return res;
        }

        [HttpGet] // in esecuzione solo con un get dal client
        [ActionName("readSerie")] // nome del metodo esposto nella API
        public string readSerie()
        {
            string res =  M.readSerie(connString, factory);
            return res;
        }


        public string postSomething(object obj)
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

        [HttpGet]
        [ActionName("constructSolution")]
        public int constructSolution()
        {
            return M.constructSolution();
        }

        [HttpGet]
        [ActionName("optimization")]
        public int optimization()
        {
            return M.opt10();
        }

        [HttpGet]
        [ActionName("simulatedAnnealing")]
        public int simulatedAnnealing()
        {
            return M.simulatedAnnealing();
        }

    }
}