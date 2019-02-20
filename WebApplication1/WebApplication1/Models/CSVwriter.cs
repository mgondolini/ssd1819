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
        private string serie;
        public static string FILE_PATH = (string)AppDomain.CurrentDomain.GetData("DataDirectory") + "\\";

        public CSVwriter(string serieName, string serie)
        {
            this.serieName = serieName;
            this.serie = serie;
        }

        /*Convert data into csv file*/
        public void toCSVFile()
        {
            //Overwrite the file, if present.
            using (StreamWriter writer = new StreamWriter(FILE_PATH + serieName, false))
            {
                writer.WriteLine(serieName);
                writer.Close();
            }
            //Append to the file.
            StreamWriter appender = new StreamWriter(FILE_PATH + serieName, true);
            List<double> source = this.chooseSource(serieName);
            source.ForEach(elem => appender.WriteLine(source));
            appender.Close();

        }

        private List<double> chooseSource(string fileName)
        {
            Debug.Print(fileName);
            List<double> l = new List<double>();


            List<string> values = serie.Split(',').ToList(); //from string to list
            values.Select(double.Parse).ToList();

            //if (fileName == "esempio.csv")
            //{
            //    data.ForEach(elem =>
            //    {
            //        if (elem.esempio != null)
            //            l.Add((double)elem.esempio);
            //    });
            //}

            //if (fileName == "esempio2.csv")
            //{
            //    data.ForEach(elem =>
            //    {
            //        if (elem.esempio2 != null)
            //            l.Add((double)elem.esempio2);
            //    });
            //}

            //if (fileName == "gioiellerie.csv")
            //{
            //    data.ForEach(elem =>
            //    {
            //        if (elem.jewelry != null)
            //            l.Add((double)elem.jewelry);
            //    });
            //}

            //if (fileName == "passeggeri.csv")
            //{
            //    data.ForEach(elem =>
            //    {
            //        if (elem.Passengers != null)
            //            l.Add((double)elem.Passengers);
            //    });
            //}

            return l;
        }



    }
}