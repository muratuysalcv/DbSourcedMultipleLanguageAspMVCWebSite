//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FisherManager.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class USER_ACCOUNT
    {
        public int USER_ACCOUNT_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string FULL_NAME { get; set; }
        public int COMPANY_ID { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<System.DateTime> DATE_CREATE { get; set; }
        public Nullable<System.DateTime> DATE_UPDATE { get; set; }
        public Nullable<System.DateTime> DATE_DELETE { get; set; }
        public string PASSWORD { get; set; }
        public string USERNAME { get; set; }
        public string E_MAIL { get; set; }
        public Nullable<System.DateTime> DATE_LAST_LOGIN { get; set; }
        public string ACTIVATION_CODE { get; set; }
        public Nullable<System.DateTime> DATE_PASSWORD_RESET_REQUEST { get; set; }
        public Nullable<bool> PASSWORD_RESET_IS_USED { get; set; }
        public string PASSWORD_RESET_CODE { get; set; }
        public int ROLE_ID { get; set; }
        public string DESCRIPTION { get; set; }
    
        public virtual COMPANY COMPANY { get; set; }
        public virtual ROLE ROLE { get; set; }
    }
}
