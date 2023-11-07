using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Models
{
    public class ApiResponseModel
    {
        public string MESSAGE { get; set; }

        public List<string> ERRORS = new List<string>();
        public bool STATUS
        {
            get
            {
                return ERRORS.Count == 0;
            }
        }

        public Exception EXCEPTION { get; set; }
        public string LANGUAGE_CODE { get; set; }
        public object RESULT_DATA { get; set; }
        public string RESOURCE_KEY { get; set; }
        public string RESOURCE_VALUE { get; set; }
    }
}
