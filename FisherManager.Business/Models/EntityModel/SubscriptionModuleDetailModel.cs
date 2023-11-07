using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Models.EntityModel
{
    public class SubscriptionModuleDetailModel
    {
        public int SUBSCRIPTION_ID { get; set; }
        public string SUBSCRIPTION_SYSTEM_CODE_NAME { get; set; }
        public int SUBSCRIPTION_PERIOD { get; set; }
        public decimal SUBSCRIPTION_PRICE { get; set; }
        public string SUBSCRIPTION_CURRENCY_CODE { get; set; }
        public int MODULE_ID { get; set; }
        public string MODULE_SYSTEM_CODE_NAME { get; set; }
        public string MODULE_DESCRIPTION { get; set; }
        public decimal MODULE_PRICE_ADDITIONAL { get; set; }
        public string MODULE_CURRENCY_CODE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public List<SUBSCRIPTION_MODULE> MODULE_SYSTEM_CODE_NAME_LIST { get; set; }
        public string SUBSCRIPTION_DESCRIPTION { get; set; }

    }
}
