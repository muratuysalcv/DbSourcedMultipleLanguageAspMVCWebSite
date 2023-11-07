using FisherManager.Business.Models;
using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class LoginManager : BaseManager
    {
        LocalizationManager LocalizationManager = new LocalizationManager();
        public LoginResponseModel Login(LoginRequestModel model, string culture)
        {
            var response = new LoginResponseModel();
            if (!String.IsNullOrEmpty(model.USERNAME) && !String.IsNullOrEmpty(model.PASSWORD))
            {

                var loginUser = db.USER_ACCOUNT.FirstOrDefault(x => x.USERNAME == model.USERNAME && x.PASSWORD == model.PASSWORD);
                if (loginUser != null)
                {
                    response.USER_ACCOUNT = (USER_ACCOUNT)loginUser;
                    response.RESOURCE_KEY = "LOGIN_SUCCESS";
                    LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                    {
                        LANGUAGE_CODE = culture,
                        RESOURCE_KEY = response.RESOURCE_KEY
                    };

                    response.RESOURCE_VALUE = LocalizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                    return response;
                }
                else
                {
                    response.RESOURCE_KEY = "LOGIN_FAIL";
                    LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                    {
                        LANGUAGE_CODE = culture,
                        RESOURCE_KEY = response.RESOURCE_KEY
                    };
                    response.RESOURCE_VALUE = LocalizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                    return response;
                }
            }
            else
            {
                response.RESOURCE_KEY = "LOGIN_INFO_CANNOT_BE_LEFT_BLANK";
                LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = response.RESOURCE_KEY,
                };
                response.USER_ACCOUNT = null;
                response.RESOURCE_VALUE = LocalizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                return response;
            }

        }


    }
}
