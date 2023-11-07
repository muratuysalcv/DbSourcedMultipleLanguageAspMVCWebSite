using FisherManager.Business.Models;
using FisherManager.Business.Models.EntityModel;
using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;

namespace FisherManager.Business.Managers
{
    public class CompanyManager : BaseManager
    {
        UserManager userManager = new UserManager();
        MessageManager messageManager = new MessageManager();
        public List<COMPANY> GetCompanyList()
        {
            var companyList = db.COMPANY.ToList();

            return companyList;
        }
        public COMPANY GetCompanyDetail(int companyId)
        {
            if (companyId != 0)
            {
                var company = db.COMPANY.FirstOrDefault(x => x.COMPANY_ID == companyId);
                return company;
            }
            else
            {
                return null;
            }

        }
        public List<COMPANY_PARAMETER> GetCompanyParameter(int companyId)
        {
            if (companyId != 0)
            {
                var companyParameter = db.COMPANY_PARAMETER.Where(x => x.COMPANY_ID == companyId).ToList();
                return companyParameter;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 1- Firma bilgileri sisteme kaydedilir
        /// 2- Firma bilgisine bağlı olarak e-mail parametre kaydı oluşturulur.
        /// 3- Firma için kullanıcı paneline giriş yapabilecekleri bir kullanıcı oluşturulur.
        /// 4- Oluşturulan kullanıcı bilgisi hem FisherManager ekibine hem de firma tarafına gönderilir.
        /// </summary>
        /// <param name="model"></param>
        public void CreateCompany(CompanyModel model)
        {
            COMPANY company = new COMPANY()
            {
                ADDRESS = model.ADDRESS,
                CITY = model.CITY,
                COUNTRY = model.COUNTRY,
                DATE_CREATE = DateTime.UtcNow,
                DESCRIPTION = model.DESCRIPTION,
                DISTRICT = model.DISTRICT,
                IS_ACTIVE = true,
                IS_DELETE = false,
                IS_ROOT = false,
                NAME = model.NAME,
                PRODUCTION_CAPACITY = model.PRODUCTION_CAPACITY,
                TAX_NUMBER = model.TAX_NUMBER,
                TAX_OFFICE = model.TAX_OFFICE,
            };
            db.COMPANY.Add(company);
            if (db.SaveChanges() > 0)
            {
                var companyId = company.COMPANY_ID;
                COMPANY_PARAMETER companyParameter = new COMPANY_PARAMETER()
                {
                    PARAMETER_TYPE_CODE = "E_MAIL",
                    COMPANY_ID = companyId,
                    PARAMETER_VALUE = model.E_MAIL,
                    DESCRIPTION = ""

                };
                db.COMPANY_PARAMETER.Add(companyParameter);
                if (db.SaveChanges() > 0)
                {
                    USER_ACCOUNT userAccount = new USER_ACCOUNT()
                    {
                        COMPANY_ID = companyId,
                        USERNAME = model.NAME.ToLower() + DateTime.UtcNow.Second.ToString(),
                        FIRST_NAME = model.NAME,
                        LAST_NAME = model.NAME,
                        FULL_NAME = model.NAME.ToLower() + DateTime.UtcNow.Second.ToString() + DateTime.UtcNow.Millisecond.ToString(),
                        ROLE_ID = 2, //CustomerRole
                        IS_ACTIVE = true,
                        IS_DELETE = false,
                        DATE_CREATE = DateTime.UtcNow,
                        PASSWORD = Membership.GeneratePassword(12, 1),
                        DESCRIPTION = "",
                        E_MAIL = ""
                    };

                    var savedUser = userManager.CreateUserAccount(userAccount);
                    AfterCompanyCreateMessageModel afterCompanyCreateMessageModel = new AfterCompanyCreateMessageModel()
                    {
                        COMPANY_NAME = company.NAME,
                        DATE_CREATE = company.DATE_CREATE,
                        E_MAIL = model.E_MAIL,
                        PASSWORD = userAccount.PASSWORD,
                        USERNAME = userAccount.USERNAME
                    };
                    var culture = Thread.CurrentThread.CurrentCulture.Name;
                    if(culture == "tr-TR")
                    {
                        messageManager.SendMailAfterCompanyCreate(afterCompanyCreateMessageModel, CreateCompanyUserMessageTR(afterCompanyCreateMessageModel.COMPANY_NAME, afterCompanyCreateMessageModel.USERNAME, afterCompanyCreateMessageModel.PASSWORD), culture);
                    }
                    else
                    {
                        messageManager.SendMailAfterCompanyCreate(afterCompanyCreateMessageModel, CreateCompanyUserMessageEN(afterCompanyCreateMessageModel.COMPANY_NAME, afterCompanyCreateMessageModel.USERNAME, afterCompanyCreateMessageModel.PASSWORD), culture);
                    }
                   
                    
                }
            }

        }
        public string CreateCompanyUserMessageTR(string companyName, string username, string password)
        {
            string body = "";

            body = @"<html><body>" +
                 "<p><h3>"+companyName+", firmanız için oluşturulan kullanıcı bilgileri aşağıdadır:</h3></p> " +
                 "<p>" +
                 "<ul>" +
                 "<li>Kullanıcı Adı: " + username+ "</li>" +
                 "<li>Parola: " + password+ "</li>" +
                 "</ul>" +
                 "</p>" +
                 "</body></html>";

            return body;
        }
        public string CreateCompanyUserMessageEN(string companyName, string username, string password)
        {
            string body = "";

            body = @"<html><body>" +
                 "<p><h3>" + companyName + ", the user information created for your company is as follows:</h3></p> " +
                 "<p>" +
                 "<ul>" +
                 "<li>Username: " + username + "</li>" +
                 "<li>Password: " + password + "</li>" +
                 "</ul>" +
                 "</p>" +
                 "</body></html>";

            return body;
        }


    }
}
