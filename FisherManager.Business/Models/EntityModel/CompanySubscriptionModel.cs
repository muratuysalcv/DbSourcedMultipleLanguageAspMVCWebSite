using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Models.EntityModel
{
    public class CompanySubscriptionModel
    {
        public int COMPANY_ID { get; set; }
        public int SUBSCRIPTION_ID { get; set; }
        public DateTime DATE_START { get; set; }
        public DateTime DATE_END { get; set; }
        public string DATE_START_STR { get; set; }
        public string DATE_END_STR { get; set; }
        public int PERIOD { get; set; }
        public decimal PRICE { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string SYSTEM_CODE_NAME { get; set; }
        public int ID { get; set; }
        public string DAYS_LEFT { get; set; }
        public string IS_ACTIVE { get; set; }
        
    }
}
