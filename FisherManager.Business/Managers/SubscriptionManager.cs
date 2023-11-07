using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class SubscriptionManager:BaseManager
    {
        public List<SUBSCRIPTION> GetSubscriptionList()
        {
            var list = db.SUBSCRIPTION.ToList();
            return list;
        }
        public void CreateSubscription(SUBSCRIPTION subscription)
        {
            if(subscription != null)
            {
                db.SUBSCRIPTION.Add(subscription);
                db.SaveChanges();
            }
            
        }
        public SUBSCRIPTION GetSubscriptionById(int subscriptionId)
        {
            var subscription = db.SUBSCRIPTION.FirstOrDefault(x => x.SUBSCRIPTION_ID == subscriptionId);
            return subscription;
        }
    }
}
