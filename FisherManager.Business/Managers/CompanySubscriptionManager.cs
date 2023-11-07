using FisherManager.Business.Models.EntityModel;
using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class CompanySubscriptionManager : BaseManager
    {
        public List<COMPANY_SUBSCRIPTION> GetCompanySubscriptionListById(int companyId)
        {
            var list = db.COMPANY_SUBSCRIPTION.Where(x => x.COMPANY_ID == companyId).ToList();
            return list;
        }
        public COMPANY_SUBSCRIPTION GetCompanySubscriptionDaysLeftControlByCompanyId(int companyId)
        {
            var companySubs = db.COMPANY_SUBSCRIPTION.FirstOrDefault(x => x.COMPANY_ID == companyId);
            return companySubs;
        }
        public COMPANY_SUBSCRIPTION GetCompanySubscriptionById(int id)
        {
            var companySubs = db.COMPANY_SUBSCRIPTION.FirstOrDefault(x => x.ID == id);
            return companySubs;
        }
        public void UpdateCompanySubscription(COMPANY_SUBSCRIPTION companySubs)
        {
            if (companySubs != null)
            {
                var companySubscription = db.COMPANY_SUBSCRIPTION.FirstOrDefault(x => x.COMPANY_ID == companySubs.ID);
                companySubscription.IS_ACTIVE = companySubs.IS_ACTIVE;
                companySubscription.DATE_END = companySubs.DATE_END;
                companySubscription.DATE_START = companySubs.DATE_START;
                companySubscription.DATE_UPDATE = companySubs.DATE_UPDATE;
                companySubscription.SUBSCRIPTION_ID = companySubs.SUBSCRIPTION_ID;

                db.SaveChanges();
            }

        }
        public void CreateCompanySubscription(COMPANY_SUBSCRIPTION companySubs)
        {
            if (companySubs != null)
            {
                db.COMPANY_SUBSCRIPTION.Add(companySubs);
                db.SaveChanges();
            }
        }




        
    }
}
