using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Models.EntityModel
{
    public class SubscriptionDetailModel
    {
        public int SUBSCRIPTION_ID { get; set; }
        public string SUBSCRIPTION_NAME { get; set; }
        public string MODULE_NAME { get; set; }
        public string SUBSCRIPTION_DESCRIPTION { get; set; }
        public string MODULE_DESCRIPTION { get; set; }
        public int MODULE_ID { get; set; }
        public decimal SUBSCRIPTION_PRICE { get; set; }
        public string SUBSCRIPTION_CURRENCY_TYPE { get; set; }
    }
}
