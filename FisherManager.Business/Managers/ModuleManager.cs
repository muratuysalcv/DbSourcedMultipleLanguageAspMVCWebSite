using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class ModuleManager : BaseManager
    {
        public List<MODULE> GetModuleList()
        {
            var list = db.MODULE.ToList();
            return list;
        }
        public MODULE GetModuleById(int moduleId)
        {
            var module = db.MODULE.FirstOrDefault(x => x.MODULE_ID == moduleId);
            return module;
        }
        public void CreateModule(MODULE module)
        {
            if (module != null)
            {
                db.MODULE.Add(module);
                db.SaveChanges();
            }
            
        }
        public void UpdateModule(MODULE module)
        {
            var updateModule = db.MODULE.FirstOrDefault(x => x.MODULE_ID == module.MODULE_ID);

            updateModule.CURRENCY = module.CURRENCY;
            updateModule.CURRENCY_CODE = module.CURRENCY_CODE;
            updateModule.PRICE_ADDITIONAL = module.PRICE_ADDITIONAL;
            updateModule.DATE_UPDATE = DateTime.UtcNow;
            updateModule.SYSTEM_CODE_DESCRIPTION = module.SYSTEM_CODE_DESCRIPTION;
            updateModule.IS_ACTIVE = module.IS_ACTIVE;
            updateModule.SYSTEM_CODE_NAME = module.SYSTEM_CODE_NAME;
            db.SaveChanges();
        }
        public void DeleteModule(MODULE module)
        {
            var deleteModule = db.MODULE.FirstOrDefault(x => x.MODULE_ID == module.MODULE_ID);
            deleteModule.DATE_DELETE = DateTime.UtcNow;
            deleteModule.IS_DELETE = true;
            deleteModule.IS_ACTIVE = false;
            db.SaveChanges();
        }
    }
}
