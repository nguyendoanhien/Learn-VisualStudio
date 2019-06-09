using System;
using System.Collections;
using System.Threading;
using System.Web;

namespace Utilities
{
    public class OnlineUsersInstance
    {
        public static OnlineUsers OnlineUsers;

        static OnlineUsersInstance()
        {
            OnlineUsers = new OnlineUsers();
        }
    }

    /// <summary>
    ///     Checks users online state
    /// </summary>
    public class OnlineUsers
    {
        /// <summary>
        ///     240000 = 4 minutes
        /// </summary>
        private const int TimerPeriod = 240000;

        private readonly Hashtable _fOnlineUsers = new Hashtable();
        private readonly Timer _thTimer;

        private readonly string _unknownUser = "_'?Unknown\nUser?'_";

        //int imtoi
        public OnlineUsers()
        {
            _unknownUser += DateTime.Now.Ticks.ToString();
            // Timer will start after _TimerPeriod
            _thTimer = new Timer(_ThreadTimerCallback, this, TimerPeriod, TimerPeriod);
        }

        #region Properties

        public int RegistredUsersCount { get; private set; }
        public int GuestUsersCount { get; private set; }

        public int UsersCount
        {
            get
            {
                Monitor.Enter(_fOnlineUsers);
                try
                {
                    return _fOnlineUsers.Count;
                }
                finally
                {
                    Monitor.Exit(_fOnlineUsers);
                }
            }
        }

        #endregion

        #region public

        public void UpdateForUserLeave()
        {
            var context = HttpContext.Current;
            if (context == null) return;
            if (context.User != null && context.User.Identity.IsAuthenticated)
                SetOfflineMemberInternal(context.User.Identity.Name);
            else if (context.Session != null) SetOfflineGuestInternal(_unknownUser + context.Session.SessionID);
        }

        /// <summary>
        ///     Check user online state
        /// </summary>
        public bool IsOnline(string userName)
        {
            Monitor.Enter(_fOnlineUsers);
            try
            {
                return _fOnlineUsers[userName] != null;
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }
        }

        public DateTime GetLastActivity(string userName)
        {
            object state;
            Monitor.Enter(_fOnlineUsers);
            try
            {
                state = _fOnlineUsers[userName];
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }

            return (DateTime) state; //new DateTime((long)state);
        }

        /// <summary>
        ///     Set user state to online
        /// </summary>
        public void SetUserOnline(string userName)
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                SetOnlineMemberInternal(userName);
            }
            else
            {
                if (context.Session != null) SetOfflineGuestInternal(_unknownUser + context.Session.SessionID);
                SetOnlineMemberInternal(userName);
            }
        }

        /// <summary>
        ///     Make user offline
        /// </summary>
        public void SetUserOffline(string userName)
        {
            SetOfflineInternal(userName);
        }

        /// <summary>
        ///     Adds user to the list
        /// </summary>
        internal void UpdateForNewUser()
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                AddUnknownUser();
                return;
            }

            if (context.User != null && context.User.Identity.IsAuthenticated)
                SetOnlineMemberInternal(context.User.Identity.Name);
            else if (context.Session != null)
                SetOnlineGuestInternal(_unknownUser + context.Session.SessionID);
            else
                AddUnknownUser();
        }

        internal void UpldateUserActivity()
        {
            var context = HttpContext.Current;
            if (context == null)
                return;
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var userName = context.User.Identity.Name;
                SetOnlineMemberInternal(userName);
            }
            else if (context.Session != null)
            {
                var sessionId = _unknownUser + context.Session.SessionID;
                SetOnlineGuestInternal(sessionId);
            }
        }

        #endregion

        #region private

        /// <summary>
        ///     This callback should recheck all the uesrs list
        /// </summary>
        /// <param name="state"></param>
        private void _ThreadTimerCallback(object state)
        {
            var now = DateTime.Now;
            var expired = new ArrayList(_fOnlineUsers.Count);
            Monitor.Enter(_fOnlineUsers);
            try
            {
                // Searching for expired users
                foreach (DictionaryEntry entry in _fOnlineUsers)
                    if ((now - (DateTime) entry.Value).TotalMilliseconds > TimerPeriod)
                        expired.Add(entry.Key);
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }

            // Remove expired items
            for (var i = 0; i < expired.Count; i++)
            {
                var user = expired[i].ToString();
                SetOfflineInternal(user);
            }
        }

        private void AddUnknownUser()
        {
            Monitor.Enter(_fOnlineUsers);
            try
            {
                _fOnlineUsers[_unknownUser + Guid.NewGuid()] = DateTime.Now;
                IncreaseGuestsCount();
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }
        }

        private void SetOnlineMemberInternal(string userName)
        {
            Monitor.Enter(_fOnlineUsers);
            try
            {
                if (_fOnlineUsers.ContainsKey(userName) == false)
                    IncreaseMembersCount();
                _fOnlineUsers[userName] = DateTime.Now;
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }
        }

        private void SetOnlineGuestInternal(string identity)
        {
            Monitor.Enter(_fOnlineUsers);
            try
            {
                if (_fOnlineUsers.ContainsKey(identity) == false)
                    IncreaseGuestsCount();
                _fOnlineUsers[identity] = DateTime.Now;
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }
        }

        private void SetOfflineInternal(string identity)
        {
            Monitor.Enter(_fOnlineUsers);
            try
            {
                if (_fOnlineUsers.ContainsKey(identity))
                {
                    _fOnlineUsers.Remove(identity);
                    if (identity.StartsWith(_unknownUser))
                        DecreaseGuestsCount();
                    else
                        DecreaseMembersCount();
                }
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }
        }

        private void SetOfflineMemberInternal(string userName)
        {
            Monitor.Enter(_fOnlineUsers);
            try
            {
                if (_fOnlineUsers.ContainsKey(userName))
                {
                    _fOnlineUsers.Remove(userName);
                    DecreaseMembersCount();
                }
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }
        }

        private void SetOfflineGuestInternal(string identity)
        {
            Monitor.Enter(_fOnlineUsers);
            try
            {
                if (_fOnlineUsers.ContainsKey(identity))
                {
                    _fOnlineUsers.Remove(identity);
                    DecreaseGuestsCount();
                }
            }
            finally
            {
                Monitor.Exit(_fOnlineUsers);
            }
        }

        private void DecreaseMembersCount()
        {
            RegistredUsersCount--;
            if (RegistredUsersCount < 0)
                RegistredUsersCount = 0;
        }

        private void DecreaseGuestsCount()
        {
            GuestUsersCount--;
            if (GuestUsersCount < 0)
                GuestUsersCount = 0;
        }

        private void IncreaseMembersCount()
        {
            RegistredUsersCount++;
        }

        private void IncreaseGuestsCount()
        {
            GuestUsersCount++;
        }

        #endregion
    }
}