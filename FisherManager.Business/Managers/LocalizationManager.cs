using FisherManager.Business.Models;
using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class LocalizationManager : BaseManager
    {
        public LocalizationResponseModel GetResource(LocalizationRequestModel model)
        {
            var responseModel = new LocalizationResponseModel();
            var item = db.LANGUAGE_RES.FirstOrDefault(x => x.RESOURCE_KEY == model.RESOURCE_KEY && x.LANGUAGE_CODE == model.LANGUAGE_CODE);

            if (item != null)
                responseModel.RESOURCE_VALUE = item.RESOURCE_VALUE;
            else
                responseModel.RESOURCE_VALUE = model.RESOURCE_KEY;

            return responseModel;
        }
    }
}
