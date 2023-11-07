using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FisherManager.Data;

namespace FisherManager.Business
{
    public class BaseManager
    {
        public fishermanager_webEntities db { get; set; }

        public string CurrentCulture
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.Name;
            }
        }
        public BaseManager()
        {
            db = new fishermanager_webEntities();
        }
    }
}

