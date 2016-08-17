using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace IQFeed
{

    public class LookupTableMarketEventArgs : LookupEventArgs
    {
        public LookupTableMarketEventArgs(string line) :
            base(null, LookupType.REQ_TAB_MKT, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            _code = fields[0];
            _shortName = fields[1];
            _longName = fields[2];
         }
        public string Code { get { return _code; } }
        public string ShortName { get { return _shortName; } }
        public string LongName { get { return _longName; } }

        #region private
        private string _code;
        private string _shortName;
        private string _longName;
        #endregion
    }
    public class LookupTableMarketCenterEventArgs : LookupEventArgs
    {
        public LookupTableMarketCenterEventArgs(string line) :
            base(null, LookupType.REQ_TAB_MKC, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
           _code = fields[0];
            _marketEquityId = fields[1].Split(' ');
            _marketOptionId = fields[2].Split(' ');
        }
        public string Code { get { return _code; } }
        public string[] MarketEquityId { get { return _marketEquityId; } }
        public string[] MarketOptionId { get { return _marketOptionId; } }

        #region private

        private string _code;
        private string[] _marketEquityId;
        private string[] _marketOptionId;
        #endregion
    }
    public class LookupTableNaicEventArgs : LookupEventArgs
    {
        public LookupTableNaicEventArgs(string line) :
            base(null, LookupType.REQ_TAB_NAC, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            _code = fields[0];
            _description = fields[1];
        }
        public string Code { get { return _code; } }
        public string Description { get { return _description; } }
 
        #region private
        private string _code;
        private string _description;
        #endregion
    }
    public class LookupTableSecurityTypeEventArgs : LookupEventArgs
    {
        public LookupTableSecurityTypeEventArgs(string line) :
            base(null, LookupType.REQ_TAB_SEC, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            _code = fields[0];
            _shortName = fields[1];
            _longName = fields[2];
        }
        public string Code { get { return _code; } }
        public string ShortName { get { return _shortName; } }
        public string LongName { get { return _longName; } }

        #region private
        private string _code;
        private string _shortName;
        private string _longName;
        #endregion
    }
    public class LookupTableSicEventArgs : LookupEventArgs
    {
        public LookupTableSicEventArgs(string line) :
            base(null, LookupType.REQ_TAB_SIC, LookupSequence.MessageDetail)
        {
            string[] fields = line.Split(',');
            _code = fields[0];
            _description = fields[1];
        }
        public string Code { get { return _code; } }
        public string Description { get { return _description; } }

        #region private
        private string _code;
        private string _description;
        #endregion
    }

    public class IQLookupTableClient : SocketClient
    {
        // Delegates for event
        public event EventHandler<LookupEventArgs> LookupEvent;


        public IQLookupTableClient(int bufferSize) : base(IQSocket.GetEndPoint(PortType.Lookup), bufferSize)
        {
            _que = new ConcurrentQueue<LookupType>();
        }
        public void Connect()
        {
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

        public void RequestMarkets()
        {
            Send("SLM\r\n");
            _enQue(LookupType.REQ_TAB_MKT);
        }
        public void RequestMarketCenters()
        {
            Send("SMC\r\n");
            _enQue(LookupType.REQ_TAB_MKC);
        }
        public void RequestNaic()
        {
            Send("SNC\r\n");
            _enQue(LookupType.REQ_TAB_NAC);
         }
        public void RequestSecurityTypes()
        {
            Send("SST\r\n");
            _enQue(LookupType.REQ_TAB_SEC);
        }
        public void RequestSic()
        {
            Send("SSC\r\n");
            _enQue(LookupType.REQ_TAB_SIC);
        }

        protected override void OnTextLineEvent(TextLineEventArgs e)
        {
            if (e.textLine.StartsWith("!ENDMSG!"))
            {
                LookupType lut = _peekQue();
                OnLookupEvent(new LookupEventArgs(null, lut, LookupSequence.MessageEnd));
                _deQue();
                return;
            }

            LookupType lute = _peekQue();
 
            switch (lute)
            {
                case LookupType.REQ_TAB_MKC:
                    OnLookupEvent(new LookupTableMarketCenterEventArgs(e.textLine));
                    return;
                case LookupType.REQ_TAB_MKT:
                    OnLookupEvent(new LookupTableMarketEventArgs(e.textLine));
                    return;
                case LookupType.REQ_TAB_NAC:
                    OnLookupEvent(new LookupTableNaicEventArgs(e.textLine));
                    return;
                case LookupType.REQ_TAB_SEC:
                    OnLookupEvent(new LookupTableSecurityTypeEventArgs(e.textLine));
                    return;
                case LookupType.REQ_TAB_SIC:
                    OnLookupEvent(new LookupTableSicEventArgs(e.textLine));
                    return;
            }

            throw new Exception("(Lookup Table) NOT HANDLED:" + e.textLine);
        }
        protected virtual void OnLookupEvent(LookupEventArgs e)
        {
            if (LookupEvent != null) LookupEvent(this, e);
        }

 
        #region private

        void _deQue()
        {
            LookupType lut;
            if (!_que.TryDequeue(out lut))
            {
                throw new Exception("Out of sequence - Table receive");
            }
            if (_que.Count > 0)
            {
                OnLookupEvent(new LookupEventArgs(null, _peekQue(), LookupSequence.MessageStart));
            }
            return;
        }
        LookupType _peekQue()
        {
            LookupType lut;
            if (!_que.TryPeek(out lut))
            {
                throw new Exception("Out of sequence - Table receive");
            }
            return lut;
        }
        void _enQue(LookupType lut)
        {
            _que.Enqueue(lut);
            if (_que.Count == 1)
            {
                OnLookupEvent(new LookupEventArgs(null, lut, LookupSequence.MessageStart));
            }
        }


        private ConcurrentQueue<LookupType> _que;
        #endregion
    }
}
