using FisherManager.Business.Managers;
using FisherManager.Business.Models;
using FisherManager.Web._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;
using FisherManager.Data;
using FisherManager.Business.Models.EntityModel;

namespace FisherManager.Web.Controllers
{
    public class ManagementController : DefaultLoginController
    {
        CompanyManager companyManager = new CompanyManager();
        SubscriptionManager subsciptionManager = new SubscriptionManager();
        SubscriptionModuleManager subsciptionModuleManager = new SubscriptionModuleManager();
        CompanySubscriptionManager companySubscriptionManager = new CompanySubscriptionManager();
        ModuleManager moduleManager = new ModuleManager();
        LocalizationManager localizationManager = new LocalizationManager();
        #region Views
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult CompanyList()
        {
            return View();
        }
        public ActionResult CompanyDetail(int companyId)
        {
            ViewBag.CompanyId = companyId;
            return View();
        }
        public ActionResult SubscriptionDetail(int subscriptionId)
        {
            ViewBag.SubscriptionId = subscriptionId;
            return View();
        }
        public ActionResult Dashboard()
        {
            var user = SessionHelper.GetSession();
            ViewBag.CompanyId = user.COMPANY_ID;
            return View();
        }
        public ActionResult Subscription()
        {
            return View();
        }
        public ActionResult CompanyInformations()
        {
            var user = SessionHelper.GetSession();
            ViewBag.CompanyId = user.COMPANY_ID;
            return View();
        }
        #endregion

        [HttpPost]
        public JsonResult CreateCompany(CompanyModel model)
        {
            companyManager.CreateCompany(model);
            return Json("SUCCESS", JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetCompanyList()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            var companyList = companyManager.GetCompanyList();
            List<CompanyModel> list = new List<CompanyModel>();

            foreach (var item in companyList)
            {
                CompanyModel companyResponseModel = new CompanyModel()
                {
                    COMPANY_ID = item.COMPANY_ID,
                    DATE_CREATE_STR = item.DATE_CREATE.ToString("dd.MM.yyyy"),
                    DESCRIPTION = item.DESCRIPTION,
                    IS_ACTIVE = item.IS_ACTIVE,
                    IS_DELETE = item.IS_DELETE,
                    NAME = item.NAME,
                    IS_ROOT = item.IS_ROOT,
                    DATE_CREATE = item.DATE_CREATE,
                    DATE_DELETE = item.DATE_DELETE,
                    DATE_UPDATE = item.DATE_UPDATE

                };
                list.Add(companyResponseModel);
            }
            int totalRows = list.Count();
            if (!string.IsNullOrEmpty(searchValue))
            {
                list = list.Where(x => x.NAME.ToLower().Contains(searchValue.ToLower()) || x.DESCRIPTION.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            list = list
                .AsQueryable()
                .OrderBy(sortColumnName + " " + sortDirection).ToList();
            list = list.Skip(start).Take(length).ToList();
            int totalRowsAfterFiltering = list.Count();
            return Json(new
            {
                data = list,
                draw = Request["draw"],
                recordsTotal = companyList.Count(),
                recordsFiltered = companyList.Count()
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCompanyDetail(int companyId)
        {
            var company = companyManager.GetCompanyDetail(companyId);
            CompanyModel companyModel = new CompanyModel()
            {
                COMPANY_ID = company.COMPANY_ID,
                DATE_CREATE_STR = company.DATE_CREATE.ToString("dd.MM.yyyy"),
                DESCRIPTION = company.DESCRIPTION,
                IS_ACTIVE = company.IS_ACTIVE,
                IS_DELETE = company.IS_DELETE,
                NAME = company.NAME,
                IS_ROOT = company.IS_ROOT,
                DATE_CREATE = company.DATE_CREATE,
                DATE_DELETE = company.DATE_DELETE,
                DATE_UPDATE = company.DATE_UPDATE,
                ADDRESS = company.ADDRESS,
                CITY = company.CITY,
                COUNTRY = company.COUNTRY,
                TAX_NUMBER = company.TAX_NUMBER,
                DISTRICT = company.DISTRICT,
                TAX_OFFICE = company.TAX_OFFICE,
                PRODUCTION_CAPACITY = company.PRODUCTION_CAPACITY

            };
            return Json(companyModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCompanyParameters(int companyId)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            var companyParameterList = companyManager.GetCompanyParameter(companyId);
            List<COMPANY_PARAMETER> list = new List<COMPANY_PARAMETER>();

            foreach (var item in companyParameterList)
            {
                LocalizationRequestModel parameterTypeRequest = new LocalizationRequestModel()
                {
                    LANGUAGE_CODE = this.LANGUAGE_CODE(),
                    RESOURCE_KEY = item.PARAMETER_TYPE_CODE
                };
                LocalizationResponseModel parameterTypeResponse = localizationManager.GetResource(parameterTypeRequest);


                COMPANY_PARAMETER companyResponseModel = new COMPANY_PARAMETER()
                {
                    COMPANY_ID = item.COMPANY_ID,
                    PARAMETER_TYPE_CODE = parameterTypeResponse.RESOURCE_VALUE,
                    PARAMETER_VALUE = item.PARAMETER_VALUE,
                    DESCRIPTION = item.DESCRIPTION,
                    COMPANY_PARAMETER_ID = item.COMPANY_PARAMETER_ID
                };
                list.Add(companyResponseModel);
            }
            int totalRows = list.Count();
            if (!string.IsNullOrEmpty(searchValue))
            {
                list = list.Where(x => x.PARAMETER_VALUE.ToLower().Contains(searchValue.ToLower()) || x.DESCRIPTION.ToLower().Contains(searchValue.ToLower())).ToList();
            }
            list = list
                .AsQueryable()
                .OrderBy(sortColumnName + " " + sortDirection).ToList();
            list = list.Skip(start).Take(length).ToList();
            int totalRowsAfterFiltering = list.Count();
            return Json(new
            {
                data = list,

                recordsTotal = companyParameterList.Count(),
                recordsFiltered = companyParameterList.Count()
            }, JsonRequestBehavior.AllowGet);




        }
        [HttpPost]
        public JsonResult GetSubscriptionForCompany(int companyId)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            var companySubscriptionList = companySubscriptionManager.GetCompanySubscriptionListById(companyId);

            List<CompanySubscriptionModel> list = new List<CompanySubscriptionModel>();
            CompanySubscriptionModel companySubscriptionModel = new CompanySubscriptionModel();

            foreach (var item in companySubscriptionList)
            {
                companySubscriptionModel.COMPANY_ID = item.COMPANY_ID;
                companySubscriptionModel.DATE_END = item.DATE_END;
                companySubscriptionModel.DATE_START = item.DATE_START;
                companySubscriptionModel.DATE_END_STR = item.DATE_END.ToString("dd.MM.yyyy");
                companySubscriptionModel.DATE_START_STR = item.DATE_START.ToString("dd.MM.yyyy");
                companySubscriptionModel.ID = item.ID;
                companySubscriptionModel.SUBSCRIPTION_ID = item.SUBSCRIPTION_ID;

                var subscription = subsciptionManager.GetSubscriptionById(companySubscriptionModel.SUBSCRIPTION_ID);
                companySubscriptionModel.SYSTEM_CODE_NAME = subscription.SYSTEM_CODE_NAME;
                companySubscriptionModel.PERIOD = subscription.PERIOD;
                companySubscriptionModel.CURRENCY_CODE = subscription.CURRENCY_CODE;
                companySubscriptionModel.PRICE = subscription.PRICE;
                if ((item.DATE_END - DateTime.Now).Days <= 0)
                {
                    string RESOURCE_KEY = "DAYS_HAVE_EXPIRED";
                    LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                    {
                        RESOURCE_KEY = RESOURCE_KEY,
                        LANGUAGE_CODE = this.LANGUAGE_CODE()
                    };
                    LocalizationResponseModel responseModel = localizationManager.GetResource(localizationRequestModel);
                    companySubscriptionModel.DAYS_LEFT = responseModel.RESOURCE_VALUE;
                    LocalizationRequestModel localizationIsActiveModel = new LocalizationRequestModel()
                    {
                        RESOURCE_KEY = "PASSIVE",
                        LANGUAGE_CODE = this.LANGUAGE_CODE()
                    };
                    LocalizationResponseModel responseIsActivModel = localizationManager.GetResource(localizationIsActiveModel);
                    companySubscriptionModel.IS_ACTIVE = responseIsActivModel.RESOURCE_VALUE;
                }
                else
                {
                    LocalizationRequestModel localizationIsActiveModel = new LocalizationRequestModel()
                    {
                        RESOURCE_KEY = "ACTIVE",
                        LANGUAGE_CODE = this.LANGUAGE_CODE()
                    };
                    LocalizationResponseModel responseIsActivModel = localizationManager.GetResource(localizationIsActiveModel);
                    companySubscriptionModel.IS_ACTIVE = responseIsActivModel.RESOURCE_VALUE;
                    companySubscriptionModel.DAYS_LEFT = (item.DATE_END - DateTime.Now).Days.ToString();
                }

                list.Add(companySubscriptionModel);
            }
            int totalRows = list.Count();
            if (!string.IsNullOrEmpty(searchValue))
            {
                list = list.Where(x => x.SYSTEM_CODE_NAME.ToLower().Contains(searchValue.ToLower())).ToList();
            }
            list = list
                .AsQueryable()
                .OrderBy(sortColumnName + " " + sortDirection).ToList();
            list = list.Skip(start).Take(length).ToList();
            int totalRowsAfterFiltering = list.Count();
            return Json(new
            {
                data = list,
                recordsTotal = companySubscriptionList.Count(),
                recordsFiltered = companySubscriptionList.Count()
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetCompanySubscriptionDaysLeft(int companyId)
        {
            var companySubscription = companySubscriptionManager.GetCompanySubscriptionDaysLeftControlByCompanyId(companyId);
            var dateDiff = (companySubscription.DATE_END - DateTime.Now).Days;
            if (dateDiff <= 0)
            {
                companySubscription.IS_ACTIVE = false;
            }
            else
            {
                companySubscription.IS_ACTIVE = true;
            }
            return Json(companySubscription.IS_ACTIVE, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubscriptionInfo()
        {
            List<SubscriptionModuleDetailModel> list = new List<SubscriptionModuleDetailModel>();
            var subscriptionModuleList = subsciptionModuleManager.GetSubscriptionModuleList().Where(x => x.IS_ACTIVE && !x.IS_DELETE);
            foreach (var item in subscriptionModuleList)
            {
                SUBSCRIPTION subscription = subsciptionManager.GetSubscriptionById(item.SUBSCRIPTION_ID);
                MODULE module = moduleManager.GetModuleById(item.MODULE_ID);
                LocalizationRequestModel subscriptionNameLocalization = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = subscription.SYSTEM_CODE_NAME,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationResponseModel subscriptionResponseNameLocalization = localizationManager.GetResource(subscriptionNameLocalization);
                LocalizationRequestModel moduleNameLocalization = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = module.SYSTEM_CODE_NAME,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationResponseModel moduleResponseNameLocalization = localizationManager.GetResource(moduleNameLocalization);

                SubscriptionModuleDetailModel model = new SubscriptionModuleDetailModel()
                {
                    SUBSCRIPTION_SYSTEM_CODE_NAME = subscriptionResponseNameLocalization.RESOURCE_VALUE,
                    MODULE_SYSTEM_CODE_NAME = moduleResponseNameLocalization.RESOURCE_VALUE,
                    MODULE_CURRENCY_CODE = module.CURRENCY_CODE,
                    MODULE_DESCRIPTION = module.SYSTEM_CODE_DESCRIPTION,
                    MODULE_ID = module.MODULE_ID,
                    SUBSCRIPTION_CURRENCY_CODE = subscription.CURRENCY_CODE,
                    SUBSCRIPTION_PRICE = subscription.PRICE,
                    SUBSCRIPTION_PERIOD = subscription.PERIOD,
                    SUBSCRIPTION_ID = subscription.SUBSCRIPTION_ID,
                    MODULE_PRICE_ADDITIONAL = module.PRICE_ADDITIONAL
                };
                list.Add(model);
            }


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubsList()
        {
            var list = subsciptionManager.GetSubscriptionList();
            List<SubscriptionModuleDetailModel> subscriptionModuleDetailModelList = new List<SubscriptionModuleDetailModel>();
            foreach (var item in list)
            {
                LocalizationRequestModel subscriptionNameLocalization = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = item.SYSTEM_CODE_NAME,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationRequestModel subscriptionDescriptionLocalization = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = item.SYSTEM_CODE_NAME,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationResponseModel subscriptionResponseNameLocalization = localizationManager.GetResource(subscriptionNameLocalization);
                LocalizationResponseModel subscriptionResponseDescriptionLocalization = localizationManager.GetResource(subscriptionNameLocalization);

                SubscriptionModuleDetailModel model = new SubscriptionModuleDetailModel()
                {
                    SUBSCRIPTION_PERIOD = item.PERIOD,
                    SUBSCRIPTION_SYSTEM_CODE_NAME = subscriptionResponseNameLocalization.RESOURCE_VALUE,
                    SUBSCRIPTION_PRICE = item.PRICE,
                    SUBSCRIPTION_DESCRIPTION = subscriptionResponseDescriptionLocalization.RESOURCE_VALUE,
                    SUBSCRIPTION_CURRENCY_CODE = item.CURRENCY_CODE,
                    SUBSCRIPTION_ID = item.SUBSCRIPTION_ID
                };
                subscriptionModuleDetailModelList.Add(model);
            }
            return Json(subscriptionModuleDetailModelList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubscriptionDetail(int subscriptionId)
        {
            SUBSCRIPTION subscription = subsciptionManager.GetSubscriptionById(subscriptionId);
            var subsModuleList = subsciptionModuleManager.GetSubscriptionModuleListById(subscriptionId);
            List<SubscriptionDetailModel> list = new List<SubscriptionDetailModel>();
            foreach (var item in subsModuleList)
            {
                var module = moduleManager.GetModuleById(item.MODULE_ID);

                #region Dil Desteği İşlemleri
                //Dil desteği ile çeviri sağlıyoruz
                LocalizationRequestModel subscriptionNameLocalization = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = subscription.SYSTEM_CODE_NAME,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationResponseModel subscriptionResponseNameLocalization = localizationManager.GetResource(subscriptionNameLocalization);
                LocalizationRequestModel subscriptionDescriptionLocalization = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = subscription.SYSTEM_CODE_DESCRIPTION,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationResponseModel subscriptionResponseDescriptionLocalization = localizationManager.GetResource(subscriptionDescriptionLocalization);

                LocalizationRequestModel moduleNameLocalization = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = module.SYSTEM_CODE_NAME,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationResponseModel moduleResponseNameLocalization = localizationManager.GetResource(moduleNameLocalization);
                LocalizationRequestModel moduleDescriptionLocalization = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = module.SYSTEM_CODE_DESCRIPTION,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationResponseModel moduleResponseDescriptionLocalization = localizationManager.GetResource(moduleDescriptionLocalization);
                #endregion

                SubscriptionDetailModel model = new SubscriptionDetailModel()
                {
                    MODULE_NAME = moduleResponseNameLocalization.RESOURCE_VALUE,
                    MODULE_DESCRIPTION = moduleResponseDescriptionLocalization.RESOURCE_VALUE,
                    MODULE_ID = module.MODULE_ID,
                    SUBSCRIPTION_DESCRIPTION = subscriptionResponseDescriptionLocalization.RESOURCE_VALUE,
                    SUBSCRIPTION_ID = subscription.SUBSCRIPTION_ID,
                    SUBSCRIPTION_NAME = subscriptionResponseNameLocalization.RESOURCE_VALUE,
                    SUBSCRIPTION_PRICE = subscription.PRICE,
                    SUBSCRIPTION_CURRENCY_TYPE = subscription.CURRENCY_CODE

                };
                list.Add(model);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCompanySubscriptionDetail(int id)
        {
            var compSubscription = companySubscriptionManager.GetCompanySubscriptionById(id);

            #region DaysLeftControl
            var daysLeft = "";
            if ((compSubscription.DATE_END - DateTime.Now).Days <= 0)
            {
                string RESOURCE_KEY = "DAYS_HAVE_EXPIRED";
                LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = RESOURCE_KEY,
                    LANGUAGE_CODE = this.LANGUAGE_CODE()
                };
                LocalizationResponseModel responseModel = localizationManager.GetResource(localizationRequestModel);
                daysLeft = responseModel.RESOURCE_VALUE;

            }
            else
            {
                daysLeft = (compSubscription.DATE_END - DateTime.Now).Days.ToString();
            }
            #endregion
            #region Dil ayarları
            SUBSCRIPTION subscription = subsciptionManager.GetSubscriptionById(compSubscription.SUBSCRIPTION_ID);
            LocalizationRequestModel subscriptionNameLocalization = new LocalizationRequestModel()
            {
                RESOURCE_KEY = subscription.SYSTEM_CODE_NAME,
                LANGUAGE_CODE = this.LANGUAGE_CODE()
            };
            LocalizationResponseModel subscriptionResponseNameLocalization = localizationManager.GetResource(subscriptionNameLocalization);

            #endregion

            CompanySubscriptionModel companySubscriptionModel = new CompanySubscriptionModel()
            {
                DATE_END_STR = compSubscription.DATE_END.ToString("dd.MM.yyyy"),
                DATE_START_STR = compSubscription.DATE_START.ToString("dd.MM.yyyy"),
                DAYS_LEFT = daysLeft,
                SYSTEM_CODE_NAME = subscriptionResponseNameLocalization.RESOURCE_VALUE,
                SUBSCRIPTION_ID = compSubscription.SUBSCRIPTION_ID

            };
            return Json(companySubscriptionModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ExtendTimeWithTransfer(int id, string description)
        {
            var companySubscription = companySubscriptionManager.GetCompanySubscriptionById(id);
            var company = companyManager.GetCompanyDetail(companySubscription.COMPANY_ID);
            var subscription = subsciptionManager.GetSubscriptionById(companySubscription.SUBSCRIPTION_ID);
            var subscriptionPricePerMonth = Math.Ceiling(subscription.PRICE / 12); //Paketin aylık olarak  şu anki fiyatını hesaplıyoruz.
            var customerSubsPricePerMonth = Math.Ceiling(companySubscription.PRICE / 12); // Firmanın paketi aldığı dönemki fiyatının aylık bedelini hesaplıyoruz.
            var daysLeft = (companySubscription.DATE_END - DateTime.Now).Days;
            var monthCount = 0;
            decimal companyRemaining = 0;
            decimal subsWillPay = 0;
            if (daysLeft > 0)
            {
                monthCount = daysLeft / 30; //Kaç aylık paket kullanım hakkı kaldığını hesaplıyoruz.
                if(monthCount > 1)
                {
                    companyRemaining = Convert.ToDecimal(monthCount) * Convert.ToDecimal(customerSubsPricePerMonth);
                    subsWillPay = Convert.ToDecimal(subscription.PRICE) - Convert.ToDecimal(companyRemaining);

                }
            }
            else
            {
                subsWillPay = subscription.PRICE;
            }
            
            LocalizationRequestModel subscriptionNameLocalization = new LocalizationRequestModel()
            {
                RESOURCE_KEY = subscription.SYSTEM_CODE_NAME,
                LANGUAGE_CODE = this.LANGUAGE_CODE()
            };
            LocalizationResponseModel subscriptionResponseNameLocalization = localizationManager.GetResource(subscriptionNameLocalization);

            AfterExtendTimeWithTransferModel model = new AfterExtendTimeWithTransferModel()
            {
                COMPANY_NAME = company.NAME,
                NOTIFY_DATE = DateTime.UtcNow.ToString(),
                SUBSCRIPTION_NAME = subscriptionResponseNameLocalization.RESOURCE_VALUE,
                PRICE = subsWillPay,
                DESCRIPTION= description

            };
            var response =  messageManager.SendMailExtendMailWithTransfer(model);

            LocalizationRequestModel messageRequest = new LocalizationRequestModel()
            {
                RESOURCE_KEY = response.RESOURCE_KEY,
                LANGUAGE_CODE = this.LANGUAGE_CODE()
            };
            LocalizationResponseModel messageResponse = localizationManager.GetResource(messageRequest);
            return Json(messageResponse.RESOURCE_VALUE, JsonRequestBehavior.AllowGet);

        }
    }
}