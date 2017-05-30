using System;
using System.Collections.Generic;


namespace FuturesDownloader
{

    enum MonthlyCodes { F = 1, G, H, J, K, M, N, Q, U, V, X, Z }
    class DownloadSpec
    {
        DateTime start_date { get; set; }

        string symbol { get; set; }

        MonthlyCodes monthly_code { get; set; }

        public DownloadSpec(string symbol, DateTime start_date)
        {            
            this.start_date = new DateTime(start_date.Year, start_date.Month, 1);
            this.symbol = symbol;
        }


        public IEnumerable<string> GetMonthlyCodes()
        {
            DateTime end_date = DateTime.Today;
            List<string> contracts = new List<string>();

            int i = 0;
            while (start_date.AddMonths(i) < end_date)
            {
                contracts.Add(this.symbol + convertDateToContractDate(start_date.AddMonths(i)));
                i += 1;
            }
            return contracts;
        }

        string convertDateToContractDate(DateTime date)
        {
            string month = ((MonthlyCodes)date.Month).ToString();
            string year = date.ToString("yy");
            return month + year;
        }
    }
}
