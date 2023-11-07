using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FisherManager.Web._Base
{
    public class SessionHelper
    {
        public static USER_ACCOUNT GetSession()
        {

            var sessionObject = HttpContext.Current.Session["USER_ACCOUNT"];
            HttpContext.Current.Session.Timeout = 60;
            if (sessionObject != null)
                return (USER_ACCOUNT)sessionObject;
            else
                return null;
        }
        public static void SetSession(USER_ACCOUNT USER_ACCOUNT)
        {
            HttpContext.Current.Session["USER_ACCOUNT"] = USER_ACCOUNT;
        }
        public static void KillSession()
        {
            HttpContext.Current.Session["USER_ACCOUNT"] = null;
        }
        public static bool IsAdministrator()
        {
            var USER_ACCOUNT = SessionHelper.GetSession();
            if (USER_ACCOUNT != null)
            {
                if (USER_ACCOUNT.ROLE_ID == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsCustomer()
        {
            var USER_ACCOUNT = SessionHelper.GetSession();
            if (USER_ACCOUNT != null)
            {
                if (USER_ACCOUNT.ROLE_ID == 2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}