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
    
    public partial class COMPANY_ACCOUNTING
    {
        public COMPANY_ACCOUNTING()
        {
            this.COMPANY_PAYMENT = new HashSet<COMPANY_PAYMENT>();
        }
    
        public int COMPANY_ACCOUNTING_ID { get; set; }
        public bool IS_ACTIVE { get; set; }
        public bool IS_DELETE { get; set; }
        public System.DateTime DATE_CREATE { get; set; }
        public Nullable<System.DateTime> DATE_UPDATE { get; set; }
        public Nullable<System.DateTime> DATE_DELETE { get; set; }
        public int COMPANY_ID { get; set; }
    
        public virtual COMPANY COMPANY { get; set; }
        public virtual ICollection<COMPANY_PAYMENT> COMPANY_PAYMENT { get; set; }
    }
}
