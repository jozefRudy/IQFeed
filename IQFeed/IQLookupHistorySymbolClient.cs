using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;

namespace IQFeed
{
    // Historical stock data lookup events
    public class LookupTickEventArgs : LookupEventArgs
    {
        public LookupTickEventArgs(string requestId, string line) :
            base(requestId, LookupType.REQ_HST_TCK, LookupSequence.MessageDetail)
        {            
            string[] fields = line.Split(',');
            if (!DateTime.TryParseExact(fields[1], "yyyy-MM-dd HH:mm:ss", _enUS, DateTimeStyles.None, out _dateTimeStamp)) _dateTimeStamp = DateTime.MinValue;           
            if (!double.TryParse(fields[2], out _last)) _last = 0;
            if (!int.TryParse(fields[3], out _lastSize)) _lastSize = 0;
            if (!int.TryParse(fields[4], out _totalVolume)) _totalVolume = 0;
            if (!double.TryParse(fields[5], out _bid)) _bid = 0;
            if (!double.TryParse(fields[6], out _ask)) _ask = 0;
            if (!int.TryParse(fields[7], out _tickId)) _tickId = 0;
            if (!char.TryParse(fields[10], out _basis)) _basis = ' ';
        }
        public DateTime DateTimeStamp { get { return _dateTimeStamp; } }
        public double Last { get { return _last; } }
        public int LastSize { get { return _lastSize; } }
        public int TotalVolume { get { return _totalVolume; } }
        public double Bid { get { return _bid; } }
        public double Ask { get { return _ask; } }
        public int TickId { get { return _tickId; } }
        public char Basis { get { return _basis; } }

        #region private
        private DateTime _dateTimeStamp;
        private double _last;
        private int _lastSize;
        private int _totalVolume;
        private double _bid;
        private double _ask;
        private int _tickId;
        private char _basis;
        private CultureInfo _enUS = new CultureInfo("en-US");
        #endregion
    }
    public class LookupIntervalEventArgs : LookupEventArgs
    {
        public LookupIntervalEventArgs(string requestId, string line) :
            base(requestId, LookupType.REQ_HST_INT, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            
            var dateInfo = fields[1].Split(' ')[0].Split('-');
            var timeInfo = fields[1].Split(' ')[1].Split(':');
            int year;
            int month;
            int day;
            int hour;
            int minute;
            int second;

            year = int.Parse(dateInfo[0]);
            month = int.Parse(dateInfo[1]);
            day = int.Parse(dateInfo[2]);
            hour = int.Parse(timeInfo[0]);
            minute = int.Parse(timeInfo[1]);
            second = int.Parse(timeInfo[2]);

            _dateTimeStamp = new DateTime(year, month, day, hour, minute, second);

            //if (!DateTime.TryParseExact(fields[1], "yyyy-MM-dd HH:mm:ss", _enUS, DateTimeStyles.None, out _dateTimeStamp)) _dateTimeStamp = DateTime.MinValue;
            if (!float.TryParse(fields[2], out _high)) _high = 0;
            if (!float.TryParse(fields[3], out _low)) _low = 0;
            if (!float.TryParse(fields[4], out _open)) _open = 0;
            if (!float.TryParse(fields[5], out _close)) _close = 0;
            if (!int.TryParse(fields[6], out _totalVolume)) _totalVolume = 0;
            if (!int.TryParse(fields[7], out _periodVolume)) _periodVolume = 0;
            
        }
        public DateTime DateTimeStamp { get { return _dateTimeStamp; } }
        public double High { get { return _high; } }
        public double Low { get { return _low; } }
        public double Open { get { return _open; } }
        public double Close { get { return _close; } }
        public int TotalVolume { get { return _totalVolume; } }
        public int PeriodVolume { get { return _periodVolume; } }

        #region private
        private DateTime _dateTimeStamp;
        private float _high;
        private float _low;
        private float _open;
        private float _close;
        private int _totalVolume;
        private int _periodVolume;
        private CultureInfo _enUS = new CultureInfo("en-US");
        #endregion
    }
    public class LookupDayWeekMonthEventArgs : LookupEventArgs
    {
        public LookupDayWeekMonthEventArgs(string requestId, string line) :
            base(requestId, LookupType.REQ_HST_DWM, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            if (!DateTime.TryParseExact(fields[1].Split(' ')[0], "yyyy-MM-dd", _enUS, DateTimeStyles.None, out _dateTimeStamp)) _dateTimeStamp = DateTime.MinValue;
            if (!double.TryParse(fields[2], out _high)) _high = 0;
            if (!double.TryParse(fields[3], out _low)) _low = 0;
            if (!double.TryParse(fields[4], out _open)) _open = 0;
            if (!double.TryParse(fields[5], out _close)) _close = 0;
            if (!int.TryParse(fields[6], out _periodVolume)) _periodVolume = 0;
            if (!int.TryParse(fields[7], out _openInterest)) _openInterest = 0;
        }
        public DateTime DateTimeStamp { get { return _dateTimeStamp; } }
        public double High { get { return _high; } }
        public double Low { get { return _low; } }
        public double Open { get { return _open; } }
        public double Close { get { return _close; } }
        public int PeriodVolume { get { return _periodVolume; } }
        public int OpenInterest { get { return _openInterest; } }

        #region private
        private DateTime _dateTimeStamp;
        private double _high;
        private double _low;
        private double _open;
        private double _close;
        private int _periodVolume;
        private int _openInterest;
        private CultureInfo _enUS = new CultureInfo("en-US");
        #endregion
    }

    // Symbol search lookup events
    public class LookupSymbolEventArgs : LookupEventArgs
    {
        public LookupSymbolEventArgs(string requestId, string line) :
            base(requestId, LookupType.REQ_SYM_SYM, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            if (fields.Length < 5) throw new Exception("Error in Symbol parameter provided");
            _symbol = fields[1];
            _marketId = fields[2];
            _securityId = fields[3];
            _description = "";
            for (int i = 4; i < fields.Length; i++) _description += fields[i];
        }
        public string Symbol { get { return _symbol; } }
        public string MarketId { get { return _marketId; } }
        public string SecurityId { get { return _securityId; } }
        public string Description { get { return _description; } }

        #region private
        private string _symbol;
        private string _marketId;
        private string _securityId;
        private string _description;
        #endregion
    }
    public class LookupSicSymbolEventArgs : LookupEventArgs
    {
        public LookupSicSymbolEventArgs(string requestId, string line) :
            base(requestId, LookupType.REQ_SYM_SIC, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            if (fields.Length < 6) throw new Exception("Error in SIC parameter provided");

            _sic = fields[1];
            _symbol = fields[2];
            _marketId = fields[3];
            _securityId = fields[4];
            _description = "";
            for (int i = 5; i < fields.Length; i++) _description += fields[i];
        }

        public string Sic { get { return _sic; } }
        public string Symbol { get { return _symbol; } }
        public string MarketId { get { return _marketId; } }
        public string SecurityId { get { return _securityId; } }
        public string Description { get { return _description; } }

        #region private
        private string _sic;
        private string _symbol;
        private string _marketId;
        private string _securityId;
        private string _description;
        #endregion
    }
    public class LookupNaicSymbolEventArgs : LookupEventArgs
    {
        public LookupNaicSymbolEventArgs(string requestId, string line) :
            base(requestId, LookupType.REQ_SYM_NAC, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            if (fields.Length < 6) throw new Exception("Error in NAIC parameter provided");

            _naic = fields[1];
            _symbol = fields[2];
            _marketId = fields[3];
            _securityId = fields[4];
            _description = "";
            for (int i = 5; i < fields.Length; i++) _description += fields[i];
        }
        public string Naic { get { return _naic; } }
        public string Symbol { get { return _symbol; } }
        public string MarketId { get { return _marketId; } }
        public string SecurityId { get { return _securityId; } }
        public string Description { get { return _description; } }

        #region private
        private string _naic;
        private string _symbol;
        private string _marketId;
        private string _securityId;
        private string _description;
        #endregion
    }

    public class IQLookupHistorySymbolClient : SocketClient
    {
        // Delegates for event
        public event EventHandler<LookupEventArgs> LookupEvent;

        // Constructor
        public IQLookupHistorySymbolClient(int bufferSize)
            : base(IQSocket.GetEndPoint(PortType.Lookup), bufferSize)
        {
            _histDataPointsPerSend = 500;
            _timeMarketOpen = new Time(09, 30, 00);
            _timeMarketClose = new Time(16, 00, 00);
            _lastRequestNumber = -1;
            _histMaxDataPoints = 5000;
        }

        // Command Requests
        public void Connect()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            ConnectToSocketAndBeginReceive(IQSocket.GetSocket());
        }
        public void Disconnect(int flushSeconds = 2)
        {
            DisconnectFromSocket(flushSeconds);
        }
        public void SetClientName(string name)
        {
            Send("S,SET CLIENT NAME," + name + "\r\n");
        }

        // Historical Data Requests
        public int RequestTickData(string symbol, int dataPoints, bool oldToNew)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_TCK.ToString() + _lastRequestNumber.ToString("0000000");

            string reqString = string.Format("HTX,{0},{1},{2},{3},{4}\r\n", symbol, dataPoints.ToString("0000000"), oldToNew ? "1" : "0",
                reqNo, _histDataPointsPerSend.ToString("0000000"));
            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_TCK, LookupSequence.MessageStart));
            return _lastRequestNumber;
        }
        public int RequestTickData(string symbol, int days, bool oldToNew, Time timeStartInDay = null, Time timeEndInDay = null)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_TCK.ToString() + _lastRequestNumber.ToString("0000000");
            if (timeStartInDay == null) timeStartInDay = _timeMarketOpen;
            if (timeEndInDay == null) timeEndInDay = _timeMarketClose;

            string reqString = string.Format("HTD,{0},{1},{2},{3},{4},{5},{6},{7}\r\n", symbol, days.ToString("0000000"), _histMaxDataPoints.ToString("0000000"),
                timeStartInDay.IQFeedFormat, timeEndInDay.IQFeedFormat, oldToNew ? "1" : "0", reqNo, _histDataPointsPerSend.ToString("0000000"));
            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_TCK, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }
        public int RequestTickData(string symbol, DateTime start, DateTime end, bool oldToNew, Time timeStartInDay = null, Time timeEndInDay = null)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_TCK.ToString() + _lastRequestNumber.ToString("0000000");
            if (timeStartInDay == null) timeStartInDay = _timeMarketOpen;
            if (timeEndInDay == null) timeEndInDay = _timeMarketClose;

            string reqString = string.Format("HTT,{0},{1},{2},{3},{4},{5},{6},{7},{8}\r\n", symbol, start.ToString("yyyyMMdd HHmmss"),
                end.ToString("yyyyMMdd HHmmss"), _histMaxDataPoints.ToString("0000000"),
                timeStartInDay.IQFeedFormat, timeEndInDay.IQFeedFormat, oldToNew ? "1" : "0", reqNo, _histDataPointsPerSend.ToString("0000000"));
            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_TCK, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }
        public int RequestIntervalData(string symbol, Interval interval, int dataPoints, bool oldToNew)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_INT.ToString() + _lastRequestNumber.ToString("0000000");

            string reqString = string.Format("HIX,{0},{1},{2},{3},{4},{5}\r\n", symbol, interval.Seconds.ToString("0000000"),
                dataPoints.ToString("0000000"), oldToNew ? "1" : "0", reqNo, _histDataPointsPerSend.ToString("0000000"));
            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_INT, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }
        public int RequestIntervalData(string symbol, Interval interval, int days, bool oldToNew, Time timeStartInDay = null, Time timeEndInDay = null)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_INT.ToString() + _lastRequestNumber.ToString("0000000");
            if (timeStartInDay == null) timeStartInDay = _timeMarketOpen;
            if (timeEndInDay == null) timeEndInDay = _timeMarketClose;

            string reqString = string.Format("HID,{0},{1},{2},{3},{4},{5},{6},{7},{8}\r\n", symbol, interval.Seconds.ToString("0000000"),
                days.ToString("0000000"), _histMaxDataPoints.ToString("0000000"), timeStartInDay.IQFeedFormat, timeEndInDay.IQFeedFormat,
                oldToNew ? "1" : "0", reqNo, _histDataPointsPerSend.ToString("0000000"));

            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_INT, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }
        public int RequestIntervalData(string symbol, Interval interval, DateTime start, DateTime end, bool oldToNew, Time timeStartInDay = null, Time timeEndInDay = null)
        {
            switch (interval.PeriodType)
            {
                case PeriodType.Second:
                case PeriodType.Minute:
                case PeriodType.Hour:
                    requestIntervalData(symbol, interval, start, end, oldToNew, timeStartInDay: timeStartInDay, timeEndInDay: timeEndInDay);
                    break;
                case PeriodType.Day:
                    RequestDailyData(symbol, start, end, oldToNew);
                    break;
                default:
                    break;
            }

            return _lastRequestNumber;
        }

        public int requestIntervalData(string symbol, Interval interval, DateTime start, DateTime end, bool oldToNew, Time timeStartInDay = null, Time timeEndInDay = null)
        {
            _lastRequestNumber++;

            if (timeStartInDay == null) timeStartInDay = _timeMarketOpen;
            if (timeEndInDay == null) timeEndInDay = _timeMarketClose;

            string reqNo = LookupType.REQ_HST_INT.ToString() + _lastRequestNumber.ToString("0000000");
            string reqString = string.Format("HIT,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", symbol, interval.Seconds.ToString("0000000"), start.ToString("yyyyMMdd HHmmss"), end.ToString("yyyyMMdd HHmmss"), "", timeStartInDay.IQFeedFormat, timeEndInDay.IQFeedFormat, oldToNew ? "1" : "0", reqNo, _histDataPointsPerSend.ToString("0000000"));
            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_INT, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }


        public int RequestDailyData(string symbol, int dataPoints, bool oldToNew)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_DWM.ToString() + _lastRequestNumber.ToString("0000000");
            string reqString = string.Format("HDX,{0},{1},{2},{3},{4}\r\n", symbol, dataPoints.ToString("0000000"),
                 oldToNew ? "1" : "0", reqNo, _histDataPointsPerSend.ToString("0000000"));

            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_DWM, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }

        public int RequestDailyData(string symbol, DateTime start, DateTime end, bool oldToNew)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_DWM.ToString() + _lastRequestNumber.ToString("0000000");

            string reqString = string.Format("HDT,{0},{1},{2},{3},{4},{5},{6}\r\n", symbol,
                 start.ToString("yyyyMMdd HHmmss"), end.ToString("yyyyMMdd HHmmss"),
                  _histMaxDataPoints.ToString("0000000"), oldToNew ? "1" : "0",
                 reqNo, _histDataPointsPerSend.ToString("0000000"));

            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_DWM, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }
        public int RequestWeeklyData(string symbol, int dataPoints, bool oldToNew)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_DWM.ToString() + _lastRequestNumber.ToString("0000000");

            string reqString = string.Format("HWX,{0},{1},{2},{3},{4}\r\n", symbol, dataPoints.ToString("0000000"),
                 oldToNew ? "1" : "0", reqNo, _histDataPointsPerSend.ToString("0000000"));

            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_DWM, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }
        public int RequestMonthlyData(string symbol, int dataPoints, bool oldToNew)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_HST_DWM.ToString() + _lastRequestNumber.ToString("0000000");

            string reqString = string.Format("HMX,{0},{1},{2},{3},{4}\r\n", symbol, dataPoints.ToString("0000000"),
                 oldToNew ? "1" : "0", reqNo, _histDataPointsPerSend.ToString("0000000"));

            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_HST_DWM, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }

        // Search Symbols by filter
        public enum SearchField { Symbol, Description }
        public enum FilterType { Market, SecurityType }
        public int RequestSymbols(SearchField searchField, string searchText, FilterType filterType, string[] filterValue)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_SYM_SYM.ToString() + _lastRequestNumber.ToString("0000000");

            string reqString = string.Format("SBF,{0},{1},{2},{3},{4}\r\n", (searchField == SearchField.Symbol) ? "s" : "d",
                searchText, (filterType == FilterType.Market) ? "e" : "t", Util.ArrayToString(filterValue, ' '), reqNo);

            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_SYM_SYM, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }
        public int RequestSymbolBySic(string searchText)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_SYM_SIC.ToString() + _lastRequestNumber.ToString("0000000");

            string reqString = string.Format("SBS,{0},{1}\r\n", searchText, reqNo);

            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_SYM_SIC, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }
        public int RequestSymbolByNaic(string searchText)
        {
            _lastRequestNumber++;
            string reqNo = LookupType.REQ_SYM_NAC.ToString() + _lastRequestNumber.ToString("0000000");

            string reqString = string.Format("SBN,{0},{1}\r\n", searchText, reqNo);

            Send(reqString);
            OnLookupEvent(new LookupEventArgs(reqNo, LookupType.REQ_SYM_NAC, LookupSequence.MessageStart));

            return _lastRequestNumber;
        }


        // Events
        protected override void OnTextLineEvent(TextLineEventArgs e)
        {
            if (e.textLine.StartsWith(LookupType.REQ_HST_INT.ToString()))
            {
                string reqId = e.textLine.Substring(0, e.textLine.IndexOf(','));
                if (e.textLine.StartsWith(reqId + ",!ENDMSG!"))
                {
                    OnLookupEvent(new LookupEventArgs(reqId, LookupType.REQ_HST_INT, LookupSequence.MessageEnd));
                    return;
                }

                OnLookupEvent(new LookupIntervalEventArgs(reqId, e.textLine));
                return;
            }

            if (e.textLine.StartsWith(LookupType.REQ_HST_TCK.ToString()))
            {
                string reqId = e.textLine.Substring(0, e.textLine.IndexOf(','));
                if (e.textLine.StartsWith(reqId + ",!ENDMSG!"))
                {
                    OnLookupEvent(new LookupEventArgs(reqId, LookupType.REQ_HST_TCK, LookupSequence.MessageEnd));
                    return;
                }

                OnLookupEvent(new LookupTickEventArgs(reqId, e.textLine));
                return;
            }



            if (e.textLine.StartsWith(LookupType.REQ_HST_DWM.ToString()))
            {
                string reqId = e.textLine.Substring(0, e.textLine.IndexOf(','));
                if (e.textLine.StartsWith(reqId + ",!ENDMSG!"))
                {
                    OnLookupEvent(new LookupEventArgs(reqId, LookupType.REQ_HST_DWM, LookupSequence.MessageEnd));
                    return;
                }

                OnLookupEvent(new LookupDayWeekMonthEventArgs(reqId, e.textLine));
                return;
            }

            if (e.textLine.StartsWith(LookupType.REQ_SYM_SYM.ToString()))
            {
                string reqId = e.textLine.Substring(0, e.textLine.IndexOf(','));
                if (e.textLine.StartsWith(reqId + ",!ENDMSG!"))
                {
                    OnLookupEvent(new LookupEventArgs(reqId, LookupType.REQ_SYM_SYM, LookupSequence.MessageEnd));
                    return;
                }
                if (e.textLine.StartsWith(reqId + ",E")) { return; }

                OnLookupEvent(new LookupSymbolEventArgs(reqId, e.textLine));
                return;
            }

            if (e.textLine.StartsWith(LookupType.REQ_SYM_NAC.ToString()))
            {
                string reqId = e.textLine.Substring(0, e.textLine.IndexOf(','));
                if (e.textLine.StartsWith(reqId + ",!ENDMSG!"))
                {
                    OnLookupEvent(new LookupEventArgs(reqId, LookupType.REQ_SYM_NAC, LookupSequence.MessageEnd));
                    return;
                }
                if (e.textLine.StartsWith(reqId + ",E")) { return; }


                OnLookupEvent(new LookupNaicSymbolEventArgs(reqId, e.textLine));
                return;
            }

            if (e.textLine.StartsWith(LookupType.REQ_SYM_SIC.ToString()))
            {
                string reqId = e.textLine.Substring(0, e.textLine.IndexOf(','));
                if (e.textLine.StartsWith(reqId + ",!ENDMSG!"))
                {
                    OnLookupEvent(new LookupEventArgs(reqId, LookupType.REQ_SYM_SIC, LookupSequence.MessageEnd));
                    return;
                }
                if (e.textLine.StartsWith(reqId + ",E")) { return; }

                OnLookupEvent(new LookupSicSymbolEventArgs(reqId, e.textLine));
                return;
            }

            throw new Exception("(Lookup) NOT HANDLED:" + e.textLine);
        }
        protected virtual void OnLookupEvent(LookupEventArgs e)
        {
            if (LookupEvent != null) LookupEvent(this, e);
        }

        #region private
        private int _histDataPointsPerSend;
        private int _histMaxDataPoints;
        private Time _timeMarketOpen;
        private Time _timeMarketClose;
        private int _lastRequestNumber;
        #endregion

    }
}
