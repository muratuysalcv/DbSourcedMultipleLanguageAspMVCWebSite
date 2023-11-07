using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class UserManager : BaseManager
    {
        //TODO: Response olarak dönecek
        public USER_ACCOUNT CreateUserAccount(USER_ACCOUNT userAccount)
        {
            if (userAccount != null)
            {
                db.USER_ACCOUNT.Add(userAccount);
                if(db.SaveChanges() > 0)
                    return userAccount;
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public List<USER_ACCOUNT> GetUserAccountList()
        {
            var list = db.USER_ACCOUNT.ToList();
            return list;
        }
        public void UpdateUserAccount(USER_ACCOUNT userAccount)
        {
            var user = db.USER_ACCOUNT.FirstOrDefault(x => x.USER_ACCOUNT_ID == userAccount.USER_ACCOUNT_ID);
            user.USERNAME = userAccount.USERNAME;
            user.ROLE_ID = userAccount.ROLE_ID;
            user.PASSWORD = userAccount.PASSWORD;
            user.LAST_NAME = userAccount.LAST_NAME;
            user.FULL_NAME = userAccount.FULL_NAME;
            user.E_MAIL = userAccount.E_MAIL;
            user.FIRST_NAME = userAccount.FIRST_NAME;
            user.DESCRIPTION = userAccount.DESCRIPTION;
            db.SaveChanges();

        }
        public void DeleteUser(int userAccountId)
        {
            var user = db.USER_ACCOUNT.FirstOrDefault(x => x.USER_ACCOUNT_ID == userAccountId);
            user.IS_DELETE = true;
            user.IS_ACTIVE = false;
        }
        public USER_ACCOUNT GetUserAccountById(int userAccountId)
        {
            if(userAccountId != 0)
            {
                var userAccount = db.USER_ACCOUNT.FirstOrDefault(x => x.USER_ACCOUNT_ID == userAccountId);
                return userAccount;
            }
            else
            {
                return null;
            }
        }
    }
}
