using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace IQFeed
{

    public class TextLineEventArgs : System.EventArgs
    {
        public TextLineEventArgs(string textLine)
        {
            _textLine = textLine;
        }
        public string textLine { get { return _textLine; } } 
        #region private
        private string _textLine;
        #endregion
    }

    public class SocketClient
    {
        public event EventHandler<TextLineEventArgs> TextLineEvent;
        public SocketClient(IPEndPoint endPoint, int bufferSize)
        {
            _socket = null;
            _endPoint = endPoint;
            _callback = new AsyncCallback(_OnReceive);
            _buffer = new byte[bufferSize];
        }
        protected void DisconnectFromSocket(int flushSeconds = 1)
        {
            try
            {
                _socket.Close(flushSeconds);
            }
            catch (Exception ex) 
            {
                throw new Exception("Unable to close socket", ex);
            }

        }

        public void Send(string command)
        {
            byte[] szCommand = new byte[command.Length];
            szCommand = Encoding.ASCII.GetBytes(command);
            int iBytesToSend = szCommand.Length;
            try
            {
                _socket.Send(szCommand, iBytesToSend, SocketFlags.None);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in send in socket", ex);
            }
        }
        protected void ConnectToSocketAndBeginReceive(Socket socket)
        {
            try
            {
                _socket = socket;
                _socket.Connect(_endPoint);
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, _callback, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in connecting to socket and starting receive", ex);
            }

        }


        protected virtual void OnTextLineEvent(TextLineEventArgs e)
        {
            if (TextLineEvent != null) TextLineEvent(this, e);
        }

        #region private
 
        private void _OnReceive(IAsyncResult asyn)
        {
            int iReceivedBytes = 0;
             try
            {
                iReceivedBytes = _socket.EndReceive(asyn);
            }
            catch (Exception ex)
            {
                ObjectDisposedException ode = ex as ObjectDisposedException;
                if (ode == null)
                {
                    throw new Exception("Error in ending receive", ex);
                }
                return;
            }

            string sData = Encoding.ASCII.GetString(_buffer, 0, iReceivedBytes);

            sData = _incompleteRecord + sData;
            _incompleteRecord = "";

            while (sData.Length > 0)
            {
                int iNewLinePos = -1;
                iNewLinePos = sData.IndexOf("\n");
                string sLine;
                if (iNewLinePos == -1)
                {
                    _incompleteRecord = sData;
                    sData = "";
                }
                else
                {
                    sLine = sData.Substring(0, iNewLinePos);
                    OnTextLineEvent(new TextLineEventArgs(sLine));
                    sData = sData.Substring(sLine.Length + 1);
                }
            }
            if (!_socket.Connected) { return; }
            try
            {
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, _callback, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in beginning asynchronous receive", ex);
            }

        }
         
        private AsyncCallback _callback;
        private Socket _socket;
        private IPEndPoint _endPoint;
        private byte[] _buffer;
        private string _incompleteRecord;
        #endregion
    }
}
