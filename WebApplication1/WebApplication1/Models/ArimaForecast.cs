using RDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;


namespace WebApplication1.Models
{
    public class ArimaForecast
    {

        private static string dataDirectory = (string)AppDomain.CurrentDomain.GetData("DataDirectory");
        private static int CUSTOM_HEAP_SIZE = 25000000;
        int frequency; //To be used later
        int nextValuesToCompute; //To be used later
        int[] results;
        string serie;

        public ArimaForecast(string serie, int frequency, int nextValuesToCompute)
        {
            this.frequency = frequency;
            this.nextValuesToCompute = nextValuesToCompute;
            this.serie = serie;
        }

        public string forecastComputation()
        {
            Thread t = new Thread(computeForecast, CUSTOM_HEAP_SIZE);
            t.Start();
            t.Join();
            string s = "Next Values:\n";
            for (int i = 0; i < results.Length; i++)
            {
                s += results[i].ToString() + "\n";
            }
            return s;
        }

        private void computeForecast()
        {
            string filePath = (dataDirectory + "\\" + serie).Replace("\\", "/");

            StartupParameter Rinit = new StartupParameter
            {
                Quiet = true,
                RHome = "C:\\Program Files\\R\\R-3.4.4",
                Interactive = true
            };
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance(null, true, Rinit);
            engine.Evaluate("");
            engine.Evaluate("library(tseries)");
            engine.Evaluate("library(forecast)");
            engine.Evaluate("data <- read.csv(\"" + filePath + "\")");
            engine.Evaluate("myts <- ts(data[,1], frequency = " + frequency + ")");
            engine.Evaluate("ARIMAfit1 <- auto.arima(myts, stepwise = FALSE, approximation = FALSE)");
            engine.Evaluate("myfc <- forecast(ARIMAfit1, h = " + this.nextValuesToCompute + ")");
            engine.Evaluate("intMean <- as.integer(myfc$mean)");
            IntegerVector v = engine.GetSymbol("intMean").AsInteger();
            results = v.ToArray();
        }
    }
}