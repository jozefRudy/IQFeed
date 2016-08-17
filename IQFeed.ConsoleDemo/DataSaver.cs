using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQFeed.ConsoleDemo
{
    public class DataSaver
    {
        public List<LookupEventArgs> data = new List<LookupEventArgs>();

        public DataSaver()
        {
            // Initialise lookup socket            
            var lookup = new IQLookupHistorySymbolClient(4096);
            lookup.Connect();
            lookup.LookupEvent += Lookup_LookupEvent;

            lookup.RequestIntervalData("@ES#", new Interval(PeriodType.Minute, 1), new DateTime(2005, 1, 1), DateTime.Today, true, timeStartInDay: new Time(hour: 8, minute: 30, second: 0), timeEndInDay: new Time(hour: 16, minute: 15, second: 0));

            //lookup.RequestIntervalData("@ES#", new Interval(PeriodType.Day, 1), new DateTime(2005, 1, 1), DateTime.Today, true);


            Console.ReadKey();

            lookup.Disconnect();
        }

        private void Lookup_LookupEvent(object sender, LookupEventArgs e)
        {
            var bar = e as LookupIntervalEventArgs;
            var daily = e as LookupDayWeekMonthEventArgs;

            if (bar != null)
            {
                Debug.WriteLine(bar.DateTimeStamp);
            }
        }
    }
}
