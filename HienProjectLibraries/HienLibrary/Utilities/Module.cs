using System;
using System.Web;
using System.Web.SessionState;

namespace Utilities
{
    public class Module : IHttpModule
    {
        #region IHttpModule Members

        private SessionStateModule _SessionModule;

        public void Dispose()
        {
//throw new Exception("The method or operation is not implemented.");
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
            for (var i = 0; i < context.Modules.Count; i++)
                if (context.Modules[i] is SessionStateModule)
                {
                    _SessionModule = (SessionStateModule) context.Modules[i];
                    break;
                }

            if (_SessionModule != null) _SessionModule.Start += SessionModule_Start;
        }

        private void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            OnlineUsersInstance.OnlineUsers.UpldateUserActivity();
        }

        private void SessionModule_End(object sender, EventArgs e)
        {
//OnlineUsersInstance.OnlineUsers.UpdateForUserLeave();
        }

        private void SessionModule_Start(object sender, EventArgs e)
        {
            OnlineUsersInstance.OnlineUsers.UpdateForNewUser();
        }

        #endregion
    }
}