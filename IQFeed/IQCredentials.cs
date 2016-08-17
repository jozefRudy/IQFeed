using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQFeed
{
    public class IQCredentials
    {
        public IQCredentials(string loginId = "", string password = "", bool autoConnect = false, bool saveCredentials = true)
        {
            _loginId = loginId;
            _password = password;
            _autoConnect = autoConnect;
            _saveCredentials = saveCredentials;
        }
        public string LoginId { get { return _loginId; } set { _loginId = value; } }
        public string Password { get { return _password; } set { _password = value; } }
        public bool AutoConnect { get { return _autoConnect; } set { _autoConnect = value; } }
        public bool SaveCredentials { get { return _saveCredentials; } set { _saveCredentials = value; } }

        #region private
        private string _loginId;
        private string _password;
        private bool _autoConnect;
        private bool _saveCredentials;
        #endregion
    }

}
