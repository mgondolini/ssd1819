using RDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ssd1819.Models
{
    public class Forecast
    {

        private static string dataDirectory = (string)AppDomain.CurrentDomain.GetData("DataDirectory");
        private static int CUSTOM_HEAP_SIZE = 25000000;
        private static string R_HOME = "C:\\Program Files\\R\\R-3.4.4";
        private static string libraries = @"library(tseries)
                                            library(forecast)";

        private int frequency; 
        private int periods;   
        private int[] results;
        private static string fileName = "serie.csv";

        public Forecast(int frequency, int periods)
        {
            this.frequency = frequency;
            this.periods = periods;
        }

        public string ArimaForecast()
        {
            Thread t = new Thread(ComputeArimaForecast, CUSTOM_HEAP_SIZE);
            t.Start();
            t.Join();
            string s = "";
            for (int i = 0; i < results.Length; i++)
            {
                s += results[i].ToString() + "\n";
            }
            return s;
        }

        public string NNForecast()
        {
            Thread t = new Thread(ComputeNNForecast, CUSTOM_HEAP_SIZE);
            t.Start();
            t.Join();
            string s = "";
            for (int i = 0; i < results.Length; i++)
            {
                s += results[i].ToString() + "\n";
            }
            return s;
        }

        private void ComputeArimaForecast()
        {
            string filePath = (dataDirectory + "\\" + fileName).Replace("\\", "/");

            REngine engine = StartEngine();

            engine.Evaluate(libraries);
            engine.Evaluate("data <- read.csv(\"" + filePath + "\")");
            engine.Evaluate("myts <- ts(data[,1], frequency = " + frequency + ")");
            engine.Evaluate("ARIMAfit1 <- auto.arima(myts, stepwise = FALSE, approximation = FALSE)");
            engine.Evaluate("myfc <- forecast(ARIMAfit1, h = " + periods + ")");
            engine.Evaluate("mean <- as.integer(myfc$mean)");
            IntegerVector v = engine.GetSymbol("mean").AsInteger();
            results = v.ToArray();
        }

        private void ComputeNNForecast()
        {
            string filePath = (dataDirectory + "\\" + fileName).Replace("\\", "/");

            REngine engine = StartEngine();

            engine.Evaluate(libraries);
            engine.Evaluate("data <- read.csv(\"" + filePath + "\")");
            engine.Evaluate("myts <- ts(data[,1], frequency = " + frequency + ")");
            engine.Evaluate("NNfit <- nnetar(myts)");
            engine.Evaluate("NNpred <- forecast(NNfit, h = " + periods + ")");
            engine.Evaluate("mean <- as.integer(NNpred$mean)");
            IntegerVector v = engine.GetSymbol("mean").AsInteger();
            results = v.ToArray();
        }

        private REngine StartEngine()
        {
            StartupParameter Rinit = new StartupParameter
            {
                Quiet = true,
                RHome = R_HOME,
                Interactive = true
            };
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance(null, true, Rinit);

            return engine;
        }

    }
}