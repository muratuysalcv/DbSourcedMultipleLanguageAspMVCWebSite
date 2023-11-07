using FisherManager.Business.Models;
using FisherManager.Business.Models.EntityModel;
using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class MessageManager : BaseManager
    {
        MessageResponseModel messageResponse = new MessageResponseModel();
        LocalizationManager localizationManager = new LocalizationManager();
        //Gönderilecek olan mailin hangi mail adresi tarafından gönderileceği bilgisinin db tarafından çekilmiş ayarları.
        public SmtpClient MailAddressForSend(MESSAGE_SETTING mailAddress)
        {
            SmtpClient sc = new SmtpClient();
            sc.Port = Convert.ToInt32(mailAddress.PORT);
            sc.Host = mailAddress.HOST;
            sc.EnableSsl = Convert.ToBoolean(mailAddress.ENABLE_SSL);
            sc.Credentials = new NetworkCredential(mailAddress.MESSAGE_SETTING_VALUE, mailAddress.PASSWORD);
            return sc;
        }

        //Taleplerin FisherManager tarafında düşeceği mail adresi.
        public string GetRequestMailAddress()
        {
            var requestMailAddress = db.MESSAGE_SETTING.FirstOrDefault(x => x.MESSAGE_SETTING_KEY == "REQUEST");
            return requestMailAddress.MESSAGE_SETTING_VALUE;
        }
        public string PaymentNotificationsMailAddress()
        {
            var requestMailAddress = db.MESSAGE_SETTING.FirstOrDefault(x => x.MESSAGE_SETTING_KEY == "PAYM_NOTI");
            return requestMailAddress.MESSAGE_SETTING_VALUE;
        }

        //Müşteri tarafından oluşturulan taleplerin DB'ye kaydının, hem müşteri hem de FisherManager tarafına gönderilecek maillerin ayarlarının yapıldığı metot.
        public MessageResponseModel SendGetOfferOrInfoToSystem(MESSAGE message, string customerBody, string culture)
        {
            try
            {
                message.CREATED_TIME = DateTime.UtcNow;
                //Oluşturulan talebin kaydını sistemde tutuyoruz.
                db.MESSAGE.Add(message);
                db.SaveChanges();

                //Taleplerin hem müşteriye hem de FisherManager tarafına hangi mail adresiyle gönderileceğini seçiyoruz.
                var noReplyMailAddress = db.MESSAGE_SETTING.FirstOrDefault(x => x.MESSAGE_SETTING_KEY == "NO_REPLY");

                //Müşteri talebinde FisherManager tarafına gönderilecek olan mail işlemi.
                var mailToFisherManager = new MailMessage()
                {
                    Subject = "Offer Request - "+message.E_MAIL,
                    Body = ToFisherManagerForRequestBody(message),
                    IsBodyHtml = true,
                    From = new MailAddress(noReplyMailAddress.MESSAGE_SETTING_VALUE,"Fisher Manager Software"),
                    ReplyTo = new MailAddress("info@fishermanager.com")
                };
                mailToFisherManager.To.Add(new MailAddress(GetRequestMailAddress()));
                MailAddressForSend(noReplyMailAddress).Send(mailToFisherManager);


                //TODO: Bu alan iyileştirilebilir. Dil seçenekleri çoğalırsa DB'den çekilebilir.
                if (culture == "tr-TR")
                    message.SUBJECT = "FisherManager - Talebiniz Alındı";
                else if (culture == "en-US")
                    message.SUBJECT = "FisherManager - Request Submitted";

                //Müşteri talebinde müşteri tarafına gidecek olan mail işlemleri.

                var mailToCustomer = new MailMessage()
                {
                    Subject = message.SUBJECT,
                    Body = customerBody,
                    IsBodyHtml = true,
                    From = new MailAddress(noReplyMailAddress.MESSAGE_SETTING_VALUE,"Fisher Manager Software"),
                    ReplyTo=new MailAddress("info@fishermanager.com")
                };
                mailToCustomer.To.Add(new MailAddress(message.E_MAIL,"Fisher Manager"));
                MailAddressForSend(noReplyMailAddress).Send(mailToCustomer);


                //Yukarıdaki işlemlerin başarıyla tamamlanması durumuna göre dönecek olan response.
                messageResponse = new MessageResponseModel()
                {
                    LANGUAGE_CODE = "MESSAGE_SENT_SUCCESS",
                };
                LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = messageResponse.LANGUAGE_CODE,
                    LANGUAGE_CODE = this.CurrentCulture
                };
                messageResponse.MESSAGE = localizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                return messageResponse;
            }
            catch (Exception ex)
            {
                //Yukarıdaki işlemlerin hatayla tamamlanması durumuna göre dönecek olan response.
                messageResponse = new MessageResponseModel()
                {
                    LANGUAGE_CODE = "MESSAGE_SENT_SUCCESS"
                };
                messageResponse.ERRORS.Add("MESSAGE_SENT_SUCCESS");
                messageResponse.ERRORS.Add(ex.Message);

                LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                {
                    LANGUAGE_CODE = this.CurrentCulture,
                    RESOURCE_KEY = messageResponse.LANGUAGE_CODE
                };
                messageResponse.MESSAGE = localizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                return messageResponse;
            }
        }

        //Sisteme kaydolan firma bilgisinden sonra oluşturulan kullanıcı bilgisinin hem FisherManager tarafına hem de firmaya gidecek olan mail işlemleri.
        public MessageResponseModel SendMailAfterCompanyCreate(AfterCompanyCreateMessageModel message, string customerBody, string culture)
        {
            try
            {
                //TODO: DB'de kayıt olacak olan mesaj içeriği için tablo oluşturulmalı.

                //Taleplerin hem müşteriye hem de FisherManager tarafına hangi mail adresiyle gönderileceğini seçiyoruz.
                var noReplyMailAddress = db.MESSAGE_SETTING.FirstOrDefault(x => x.MESSAGE_SETTING_KEY == "NO_REPLY");

                //Müşteri talebinde FisherManager tarafına gönderilecek olan mail işlemi.
                var mailToFisherManager = new MailMessage()
                {
                    Subject = message.SUBJECT,
                    Body = ToFisherManagerAfterCompanyCreate(message),
                    IsBodyHtml = true,
                    From = new MailAddress(noReplyMailAddress.MESSAGE_SETTING_VALUE)
                };
                mailToFisherManager.To.Add(new MailAddress(GetRequestMailAddress()));
                MailAddressForSend(noReplyMailAddress).Send(mailToFisherManager);


                //TODO: Bu alan iyileştirilebilir. Dil seçenekleri çoğalırsa DB'den çekilebilir.
                if (culture == "tr-TR")
                    message.SUBJECT = "FisherManager - Üyeliğiniz Oluşturulmuştur";
                else if (culture == "en-US")
                    message.SUBJECT = "FisherManager - Your Membership Has Been Established";

                //Müşteri talebinde müşteri tarafına gidecek olan mail işlemleri.

                var mailToCustomer = new MailMessage()
                {
                    Subject = message.SUBJECT,
                    Body = customerBody,
                    IsBodyHtml = true,
                    From = new MailAddress(noReplyMailAddress.MESSAGE_SETTING_VALUE)
                };
                mailToCustomer.To.Add(new MailAddress(message.E_MAIL));
                MailAddressForSend(noReplyMailAddress).Send(mailToCustomer);


                //Yukarıdaki işlemlerin başarıyla tamamlanması durumuna göre dönecek olan response.
                messageResponse = new MessageResponseModel()
                {
                    LANGUAGE_CODE = "MESSAGE_SENT_SUCCESS",
                };
                LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = messageResponse.LANGUAGE_CODE,
                    LANGUAGE_CODE = this.CurrentCulture
                };
                messageResponse.MESSAGE = localizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                return messageResponse;
            }
            catch
            {
                //Yukarıdaki işlemlerin hatayla tamamlanması durumuna göre dönecek olan response.
                messageResponse = new MessageResponseModel()
                {
                    LANGUAGE_CODE = "MESSAGE_SENT_ERROR",
                };
                LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                {
                    LANGUAGE_CODE = this.CurrentCulture,
                    RESOURCE_KEY = messageResponse.LANGUAGE_CODE
                };
                messageResponse.MESSAGE = localizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                return messageResponse;
            }
        }

        //Havale bildiriminden sonra FisherManager'a düşecek olan mail bildirimi
        public MessageResponseModel SendMailExtendMailWithTransfer(AfterExtendTimeWithTransferModel message)
        {
            try
            {
                //TODO: DB'de kayıt olacak olan mesaj içeriği için tablo oluşturulmalı.

                //Taleplerin FisherManager tarafına hangi mail adresiyle gönderileceğini seçiyoruz.
                var noReplyMailAddress = db.MESSAGE_SETTING.FirstOrDefault(x => x.MESSAGE_SETTING_KEY == "NO_REPLY");

                //Havale bildiriminde FisherManager tarafına gönderilecek olan mail işlemi.
                var mailToFisherManager = new MailMessage()
                {
                    Subject = "HAVALE İLE ÖDEME BİLDİRİMİ - " + message.COMPANY_NAME,
                    Body = ToFisherManagerForExtendTimeWithTransfer(message),
                    IsBodyHtml = true,
                    From = new MailAddress(noReplyMailAddress.MESSAGE_SETTING_VALUE)
                };
                mailToFisherManager.To.Add(new MailAddress(PaymentNotificationsMailAddress()));
                MailAddressForSend(noReplyMailAddress).Send(mailToFisherManager);
                //Yukarıdaki işlemlerin başarıyla tamamlanması durumuna göre dönecek olan response.
                messageResponse = new MessageResponseModel()
                {
                    LANGUAGE_CODE = "MESSAGE_SENT_SUCCESS",
                };
                LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                {
                    RESOURCE_KEY = messageResponse.LANGUAGE_CODE,
                    LANGUAGE_CODE = this.CurrentCulture
                };
                messageResponse.MESSAGE = localizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                return messageResponse;
            }
            catch
            {
                //Yukarıdaki işlemlerin hatayla tamamlanması durumuna göre dönecek olan response.
                messageResponse = new MessageResponseModel()
                {
                    LANGUAGE_CODE = "MESSAGE_SENT_ERROR",
                };
                LocalizationRequestModel localizationRequestModel = new LocalizationRequestModel()
                {
                    LANGUAGE_CODE = this.CurrentCulture,
                    RESOURCE_KEY = messageResponse.LANGUAGE_CODE
                };
                messageResponse.MESSAGE = localizationManager.GetResource(localizationRequestModel).RESOURCE_VALUE;
                return messageResponse;
            }
        }


        #region FisherManager'a Mesaj içerikleri
        //Sisteme kaydolan firma bilgisinden sonra oluşturulan kullanıcı bilgisinin FisherManager tarafına gidecek olan mail içeriği.
        public string ToFisherManagerAfterCompanyCreate(AfterCompanyCreateMessageModel message)
        {
            string body = "";

            body = @"<html><body>" +
                 "<p><h3>ÜYELİK OLUŞTURMA İŞLEMİ</h3> - Oluşturulma zamanı: " + message.DATE_CREATE + "</p> " +
                 "<p>" +
                 "<ul>" +
                 "<li>FİRMA ADI: " + message.COMPANY_NAME + "</li>" +
                 "<li>KULLANICI ADI: " + message.USERNAME + "</li>" +
                 "<li>PAROLA: " + message.PASSWORD + "</li>" +
                 "</ul>" +
                 "</p>" +
                 "</body></html>";

            return body;
        }
        //Havale bildirimi yapan müşterinin bilgisinin FisherManager'a iletileceği mail içeriği.
        public string ToFisherManagerForExtendTimeWithTransfer(AfterExtendTimeWithTransferModel message)
        {
            string body = "";

            body = @"<html><body>" +
                 "<p><h3>PAKET SÜRESİ UZATMA BİLDİRİMİ</h3> - Oluşturulma zamanı: " + message.NOTIFY_DATE + "</p> " +
                 "<p>" +
                 "<ul>" +
                 "<li>FİRMA ADI: " + message.COMPANY_NAME + "</li>" +
                 "<li>TALEP EDİLEN PAKET: " + message.SUBSCRIPTION_NAME + "</li>" +
                 "<li>TUTAR: " + message.PRICE + "</li>" +
                 "<li>AÇIKLAMA: " + message.DESCRIPTION + "</li>" +
                 "</ul>" +
                 "</p>" +
                 "</body></html>";

            return body;

        }
        //TODO - Başlıkların değiştirilmesi gerekebilir. Ör: Gelen SUBJECT değerine göre "Teklif talebi" yazması.
        //Müşteri tarafından oluşturulan taleplerin FisherManager tarafına gidecek olan mailin body'si.
        public string ToFisherManagerForRequestBody(MESSAGE message)
        {
            string body = "";
            if (message.SUBJECT == "GET_OFFER")
            {
                body = @"<html><body>" +
                 "<p><h3>TEKLİF TALEBİ</h3> Oluşturulma zamanı: " + message.CREATED_TIME + "</p> " +
                 "<p>" +
                 "<ul>" +
                 "<li>ÇİFTLİK TİPİ: " + message.FARM_TYPE + "</li>" +
                 //"<li>İL: " + message.COUNTRY + "</li>" +
                 //"<li>İLÇE: " + message.CITY + "</li>" +
                 "<li>FİRMA: " + message.COMPANY_NAME + "</li>" +
                 "<li>E-POSTA: " + message.E_MAIL + "</li>" +
                 //"<li>TELEFON: " + message.PHONE_NUMBER + "</li>" +
                 "<li>MESAJ: " + message.MESSAGE_TEXT+ "</li>" +
                 "</ul>" +
                 "</p>" +
                 "</body></html>";
            }
            else
            {
                body = @"<html><body>" +
                  "<p><h3>BİLGİ TALEBİ</h3> Oluşturulma zamanı: " + message.CREATED_TIME + "</p> " +
                  "<p>" +
                  "<ul>" +
                  "<li>FİRMA: " + message.COMPANY_NAME + "</li>" +
                  "<li>E-POSTA: " + message.E_MAIL + "</li>" +
                  //"<li>TELEFON: " + message.PHONE_NUMBER + "</li>" +
                  "<li>MESAJ: " + message.MESSAGE_TEXT + "</li>" +
                  "</ul>" +
                  "</p>" +
                  "</body></html>";
            }
            return body;
        }
        #endregion
    }
}
