using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Models
{
    public class MenuResponseModel: ApiResponseModel
    {
        public List<MENU> MENU_LIST { get; set; }
    }
}
