using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Models
{
    public class LoginResponseModel : ApiResponseModel
    {
        public USER_ACCOUNT USER_ACCOUNT{ get; set; }
    }
}
