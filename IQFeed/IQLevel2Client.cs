using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace IQFeed
{
    public class Level2UpdateEventArgs : System.EventArgs
    {
        public Level2UpdateEventArgs(string line)
        {
            string[] fields = line.Split(',');
             _symbol = fields[0];
            _mmid = fields[1];
            if (!double.TryParse(fields[2], out _bid)) _bid = 0;
            if (!double.TryParse(fields[3], out _ask)) _ask = 0;
            if (!int.TryParse(fields[4], out _bidSize)) _bidSize = 0;
            if (!int.TryParse(fields[5], out _askSize)) _askSize = 0;
            _bidTime = new Time(fields[6]);
            if (!DateTime.TryParseExact(fields[7], "yyyy-MM-dd", _enUS, DateTimeStyles.None, out _date)) _date = DateTime.MinValue;
            _reasonCode = fields[8];
            _conditionCode = fields[9];
            _sourceId = fields[10];
            _askTime = new Time(fields[11]);
            _bidInfoValid = false;
            if (fields[12] == "T") { _bidInfoValid = true; }
            _askInfoValid = false;
            if (fields[13] == "T") { _askInfoValid = true; } 
        }
        public string Symbol { get { return _symbol; } }
        public string Mmid { get { return _mmid; } }
        public double Bid { get { return _bid; } }
        public double Ask { get { return _ask; } }
        public int BidSize { get { return _bidSize; } }
        public int AskSize { get { return _askSize; } }
        public Time BidTime { get { return _bidTime; } }
        public DateTime Date { get { return _date; } }
        public string ReasonCode { get { return _reasonCode; } }
        public string ConditionCode { get { return _conditionCode; } }
        public string SourceId { get { return _sourceId; } }
        public Time AskTime { get { return _askTime; } }
        public bool BidInfoValid { get { return _bidInfoValid; } }
        public bool AskInfoValid { get { return _askInfoValid; } }
        #region private
        private string _symbol;
        private string _mmid;
        private double _bid;
        private double _ask;
        private int _bidSize;
        private int _askSize;
        private Time _bidTime;
        private DateTime _date;
        private string _reasonCode;
        private string _conditionCode;
        private string _sourceId;
        private Time _askTime;
        private bool _bidInfoValid;
        private bool _askInfoValid;
        private CultureInfo _enUS = new CultureInfo("en-US");
        #endregion
    }


    public class IQLevel2Client : SocketClient 
    {
        public IQLevel2Client(int bufferSize)
            : base(IQSocket.GetEndPoint(PortType.Level2), bufferSize)
        {
        }
    }
}
