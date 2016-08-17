using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Globalization;

namespace IQFeed
{

    public enum LookupSequence
    {
        MessageStart,
        MessageDetail,
        MessageEnd
    }
    public enum LookupType
    {
        REQ_HST_TCK,
        REQ_HST_INT,
        REQ_HST_DWM,
        REQ_SYM_SYM,
        REQ_SYM_SIC,
        REQ_SYM_NAC,
        REQ_TAB_MKT,
        REQ_TAB_SEC,
        REQ_TAB_MKC,
        REQ_TAB_SIC,
        REQ_TAB_NAC
    }

    // Lookup super event
    public class LookupEventArgs : System.EventArgs
    {
        public LookupEventArgs(string requestId, LookupType lookupType, LookupSequence lookupSequence)
        {
            _requestId = requestId;
            _lookupType = lookupType;
            _lookupSequence = lookupSequence;
        }
        public string Id { get { return _requestId; } }
        public LookupType Type { get { return _lookupType; } }
        public LookupSequence Sequence { get { return _lookupSequence; } }
        #region private
        private string _requestId;
        private LookupType _lookupType;
        private LookupSequence _lookupSequence;
        #endregion
    }







    public enum PortType { Level1 = 1, Lookup = 3, Level2 = 2, Admin = 0 }
    public static class IQSocket
    {
        public static int GetPort(PortType portType)
        {
            int iReturn = 0;
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
            if (key != null)
            {
                string sData = "";
                switch (portType)
                {
                    case PortType.Level1:
                        // the default port for Level 1 data is 5009.
                        sData = key.GetValue("Level1Port", "5009").ToString();
                        break;
                    case PortType.Lookup:
                        // the default port for Lookup data is 9100.
                        sData = key.GetValue("LookupPort", "9100").ToString();
                        break;
                    case PortType.Level2:
                        // the default port for Level 2 data is 9200.
                        sData = key.GetValue("Level2Port", "9200").ToString();
                        break;
                    case PortType.Admin:
                        // the default port for Admin data is 9300.
                        sData = key.GetValue("AdminPort", "9300").ToString();
                        break;
                }
                iReturn = Convert.ToInt32(sData);
            }
            return iReturn;
        }
        public static IPAddress GetIp()
        {
            return IPAddress.Parse("127.0.0.1");
        }
        public static IPEndPoint GetEndPoint(PortType portType)
        {
            return new IPEndPoint(GetIp(), GetPort(portType));
        }
        public static Socket GetSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }

    public class Status
    {
        internal Status()
        {
        }
        internal void Update(string line)
        {
            string[] fields = line.Split(',');
            lock (this)
            {
                _serverIp = fields[2];
                if (!int.TryParse(fields[3], out _serverPort)) _serverPort = 0;
                if (!int.TryParse(fields[4], out _maxSymbols)) _maxSymbols = 0;
                if (!int.TryParse(fields[5], out _numberOfSymbols)) _numberOfSymbols = 0;
                if (!int.TryParse(fields[6], out _clientsConnected)) _clientsConnected = 0;
                if (!int.TryParse(fields[7], out _secondsSinceLastUpdate)) _secondsSinceLastUpdate = 0;
                if (!int.TryParse(fields[8], out _reconnections)) _reconnections = 0;
                if (!int.TryParse(fields[9], out _attemptedReconnections)) _attemptedReconnections = 0;
                if (!DateTime.TryParseExact(fields[10], "MMM dd hh':'mmtt", _enUS, DateTimeStyles.None, out _startTime)) _startTime = DateTime.MinValue;
                if (!DateTime.TryParseExact(fields[11], "MMM dd hh':'mmtt", _enUS, DateTimeStyles.None, out _marketTime)) _marketTime = DateTime.MinValue;
                _connected = false;
                if (fields[12] == "Connected") { _connected = true; }
                _iqFeedVersion = fields[13];
                _loginId = fields[14];
                if (!double.TryParse(fields[15], out _totalKbsRecv)) _totalKbsRecv = 0;
                if (!double.TryParse(fields[16], out _kbsPerSecRecv)) _kbsPerSecRecv = 0;
                if (!double.TryParse(fields[17], out _avgKbsPerSecRecv)) _avgKbsPerSecRecv = 0;
                if (!double.TryParse(fields[18], out _totalKbsSent)) _totalKbsSent = 0;
                if (!double.TryParse(fields[19], out _kbsPerSecSent)) _kbsPerSecSent = 0;
                if (!double.TryParse(fields[20], out _avgKbsPerSecSent)) _avgKbsPerSecSent = 0;
            }
        }

        public string serverIp { get { lock (this) return _serverIp; } }
        public int serverPort { get { lock (this) return _serverPort; } }
        public int maxSymbols { get { lock (this) return _maxSymbols; } }
        public int numberOfSymbols { get { lock (this) return _numberOfSymbols; } }
        public int clientsConnected { get { lock (this) return _clientsConnected; } }
        public int secondsSinceLastUpdate { get { lock (this) return _secondsSinceLastUpdate; } }
        public int reconnections { get { lock (this) return _reconnections; } }
        public int attemptedReconnections { get { lock (this) return _attemptedReconnections; } }
        public DateTime startTime { get { lock (this) return _startTime; } }
        public DateTime marketTime { get { lock (this) return _marketTime; } }
        public bool connected { get { lock (this) return _connected; } }
        public string iqFeedVersion { get { lock (this) return _iqFeedVersion; } }
        public string loginId { get { lock (this) return _loginId; } }
        public double totalKbsRecv { get { lock (this) return _totalKbsRecv; } }
        public double kbsPerSecRecv { get { lock (this) return _kbsPerSecRecv; } }
        public double avgKbsPerSecRecv { get { lock (this) return _avgKbsPerSecRecv; } }
        public double totalKbsSent { get { lock (this) return _totalKbsSent; } }
        public double kbsPerSecSent { get { lock (this) return _kbsPerSecSent; } }
        public double avgKbsPerSecSent { get { lock (this) return _avgKbsPerSecSent; } }

        #region private
        private string _serverIp;
        private int _serverPort;
        private int _maxSymbols;
        private int _numberOfSymbols;
        private int _clientsConnected;
        private int _secondsSinceLastUpdate;
        private int _reconnections;
        private int _attemptedReconnections;
        private DateTime _startTime;
        private DateTime _marketTime;
        private bool _connected;
        private string _iqFeedVersion;
        private string _loginId;
        private double _totalKbsRecv;
        private double _kbsPerSecRecv;
        private double _avgKbsPerSecRecv;
        private double _totalKbsSent;
        private double _kbsPerSecSent;
        private double _avgKbsPerSecSent;
        private CultureInfo _enUS = new CultureInfo("en-US");
        #endregion
    }
 


}
 