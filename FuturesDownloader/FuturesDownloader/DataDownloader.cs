using IQFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FuturesDownloader
{
    public class DataDownloader
    {
        // public List<LookupEventArgs> data = new List<LookupEventArgs>();
        public List<BarItem> historical_data = new List<BarItem>();
        IQLookupHistorySymbolClient lookup;
        DateTime start_date = new DateTime(2000, 1, 1);
        bool downloading = false;

        public int Count { get { return historical_data.Count(); } }

        public void Download(string contract)
        {
            historical_data = new List<BarItem>();
            // Initialise lookup socket            
            lookup = new IQLookupHistorySymbolClient(4096);
            lookup.Connect();
            lookup.LookupEvent += Lookup_LookupEvent;

            lookup.RequestIntervalData(contract, new Interval(PeriodType.Minute, 1), start_date, DateTime.Today, true, timeStartInDay: new Time(hour: 00, minute: 00, second: 0), timeEndInDay: new Time(hour: 23, minute: 59, second: 0));

            downloading = true;

            while (downloading)
            {
                Thread.Sleep(500);
            }
        }

        public void Disconnect()
        {
            lookup.Disconnect();
        }

        private void Lookup_LookupEvent(object sender, LookupEventArgs e)
        {
            var bar = e as LookupIntervalEventArgs;

            if (bar != null)
            {
                var bar_item = new BarItem(bar.DateTimeStamp, bar.Open, bar.High, bar.Low, bar.Close, bar.PeriodVolume);

                if (bar_item.date > start_date) 
                    historical_data.Add(bar_item);
            }
            else
            {
                if (e.Sequence == LookupSequence.MessageStart)
                    downloading = true;
                else if (e.Sequence == LookupSequence.MessageEnd)
                    downloading = false;
            }
        }
    }
}
