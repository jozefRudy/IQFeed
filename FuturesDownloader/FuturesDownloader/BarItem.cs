using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDownloader
{
    public struct BarItem
    {
        public DateTime date;
        public double open;
        public double high;
        public double low;
        public double close;
        public int volume;

        public BarItem(DateTime date, double o, double h, double l, double c, int v)
        {
            this.date = date;
            this.open = o;
            this.high = h;
            this.low = l;
            this.close = c;
            this.volume = v;
        }
    }
}
