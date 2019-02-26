using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CSVwriter
    {
        private string serieName;
        private string serieValues;
        public static string FILE_PATH = (string)AppDomain.CurrentDomain.GetData("DataDirectory") + "\\";
        private static readonly string FILE_NAME = "serie.csv";

        public CSVwriter(string serieName, string serieValues)
        {
            this.serieName = serieName;
            this.serieValues = serieValues;
        }

        public void CreateCSV()
        {
            //Overwrite the file, if present.
            using (StreamWriter writer = new StreamWriter(FILE_PATH + FILE_NAME, false))
            {
                writer.WriteLine(serieName);
                writer.Close();
            }
            //Append to the file.
            StreamWriter appender = new StreamWriter(FILE_PATH + FILE_NAME, true);
            List<string> values = this.GetValuesList();
            values.ForEach(elem => appender.WriteLine(elem));
            appender.Close();
        }

        private List<string> GetValuesList()
        {
            List<string> l = new List<string>();
            List<string> values = serieValues.Split(',').ToList(); //from string to list

            values.ForEach(elem =>
            {
                elem = elem.Replace(".", ",");
                l.Add(elem);
            });

            return l;
        }

    }
}