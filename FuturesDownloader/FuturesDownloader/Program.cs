using System;
using System.Collections.Generic;
using CommandLine;
using System.Threading;
using System.Globalization;

namespace FuturesDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var options = new ParseOptions();
            var isValid = Parser.Default.ParseArguments(args, options);
            
            if (!isValid)
            {
                Environment.Exit(1);
            }

            DateTime start_date = DateTime.Parse(options.start_date);
            DownloadSpec spec = new DownloadSpec(options.symbol, start_date);
            IEnumerable<string> contracts = spec.GetMonthlyCodes();

            DataDownloader downloader = new DataDownloader();
            if (!downloader.isConnectionAvailable())
            {
                Console.WriteLine("IQFEED connection not available");
                Environment.Exit(1);
            }

            foreach (string symbol in contracts)
            {                
                downloader.Download(symbol);
                downloader.Disconnect();

                if (downloader.Count > 0)
                    Console.WriteLine($"{downloader.Count} data points downloaded for {symbol}");

                CsvSaver saver = new CsvSaver(options.symbol + "/" + symbol + ".csv", downloader.historical_data);
                saver.Save();
            }
                      
            Console.WriteLine("finished");
        }
    }
}
