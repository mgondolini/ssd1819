using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using Newtonsoft.Json;
using System.IO;
using System.Data;

namespace WebApplication1.Models
{
    public class Model
    {
        public delegate void viewEventHandler(object sender, string textToWrite);
        
        private GAPinstance GAP;
        private BasicHeu basicHeu;

        public GAPinstance readGAPInstance(string path)
        {
            StreamReader fin;

            try
            {
                fin = new StreamReader(path);
                string jstring = fin.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(jstring);
                GAP = JsonConvert.DeserializeObject<GAPinstance>(jstring);
                fin.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error " + ex.Message);
            }
            return GAP;
        }

        public string readSerie(string connString, string factory)
        {
            System.Diagnostics.Debug.WriteLine("connstring " + connString + " factory" + factory);
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(factory);

            string res = "";

            using (DbConnection conn = dbFactory.CreateConnection())
            {
                try
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    DbCommand com = conn.CreateCommand();
                    com.CommandText = "select time from serie";

                    DbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        System.Diagnostics.Debug.WriteLine(reader["time"] + "");
                        res = reader["time"] + "";
                        goto l0;
                    }
                        

                    reader.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine( "[dataReader] Error: " + ex.Message);
                    res = "[dataReader] Error: " + ex.Message;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            l0:
            return res;
        }

        public int constructSolution(GAPinstance GAP)
        {
            basicHeu = new BasicHeu(GAP);
            return basicHeu.constructFirtsSol();
        }

        public int opt10()
        {
            //GAP = readGAPInstance(pathjson);
            basicHeu = new BasicHeu(GAP);
            basicHeu.constructFirtsSol();
            return basicHeu.opt10(GAP.cost);
        }

        public int simulatedAnnealing()
        {
            //GAP = readGAPInstance(pathjson);
            basicHeu = new BasicHeu(GAP);
            return basicHeu.simulatedAnnealing();
        }

    }
}