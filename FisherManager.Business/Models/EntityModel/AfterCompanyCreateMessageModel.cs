using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Models.EntityModel
{
    public class AfterCompanyCreateMessageModel
    {
        public string SUBJECT { get; set; }
        public string E_MAIL { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public DateTime DATE_CREATE { get; set; }
        public string COMPANY_NAME { get; set; }
        
    }
}
