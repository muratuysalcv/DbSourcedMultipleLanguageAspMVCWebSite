using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Models
{
    public class CompanyModel 
    {
        public int COMPANY_ID { get; set; }
        public bool IS_ACTIVE { get; set; }
        public bool IS_DELETE { get; set; }
        public DateTime DATE_CREATE { get; set; }
        public string DATE_CREATE_STR { get; set; }

        public DateTime? DATE_UPDATE { get; set; }
        public string DATE_UPDATE_STR { get; set; }
        public DateTime? DATE_DELETE { get; set; }
        public string DATE_DELETE_STR { get; set; }

        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public bool? IS_ROOT { get; set; }
        public string COUNTRY { get; set; }
        public string CITY { get; set; }
        public string DISTRICT { get; set; }
        public string TAX_NUMBER { get; set; }
        public string ADDRESS { get; set; }
        public string TAX_OFFICE { get; set; }
        public string PRODUCTION_CAPACITY { get; set; }
        public string E_MAIL { get; set; }
    }
}
