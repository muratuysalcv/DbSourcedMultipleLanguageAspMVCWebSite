using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class SubscriptionModuleManager:BaseManager
    {
        public List<SUBSCRIPTION_MODULE> GetSubscriptionModuleList()
        {
            var list = db.SUBSCRIPTION_MODULE.ToList();
            return list;
        }
        public void CreateSubscriptionModule(SUBSCRIPTION_MODULE subsModule)
        {
            if(subsModule != null)
            {
                db.SUBSCRIPTION_MODULE.Add(subsModule);
                db.SaveChanges();
            }
        }
        public void UpdateSubscriptionModule(SUBSCRIPTION_MODULE subsModule)
        {
            var updateSubs = db.SUBSCRIPTION_MODULE.FirstOrDefault(x => x.ID == subsModule.ID);
            updateSubs.IS_ACTIVE = subsModule.IS_ACTIVE;
            updateSubs.MODULE_ID = subsModule.MODULE_ID;
            updateSubs.SUBSCRIPTION_ID = subsModule.SUBSCRIPTION_ID;
            updateSubs.DATE_UPDATE = DateTime.UtcNow;
            db.SaveChanges();

        }
        public void DeleteSubscriptionModule(int id)
        {
            var deleteSubsModule = db.SUBSCRIPTION_MODULE.FirstOrDefault(x => x.ID == id);
            deleteSubsModule.IS_DELETE = true;
            deleteSubsModule.DATE_DELETE = DateTime.UtcNow;
            deleteSubsModule.IS_ACTIVE = false;
            
        }
        public List<SUBSCRIPTION_MODULE> GetSubscriptionModuleListById(int subsId)
        {
            if (subsId > 0)
            {
                var dbList = db.SUBSCRIPTION_MODULE.Where(x => x.SUBSCRIPTION_ID == subsId).ToList();
                return dbList;
            }
            else
                return null;
        }
    }
}
