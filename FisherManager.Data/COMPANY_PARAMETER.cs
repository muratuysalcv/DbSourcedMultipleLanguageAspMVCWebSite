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
    
    public partial class COMPANY_PARAMETER
    {
        public int COMPANY_PARAMETER_ID { get; set; }
        public int COMPANY_ID { get; set; }
        public string PARAMETER_TYPE_CODE { get; set; }
        public string PARAMETER_VALUE { get; set; }
        public string DESCRIPTION { get; set; }
    
        public virtual COMPANY COMPANY { get; set; }
        public virtual PARAMETER_TYPE PARAMETER_TYPE { get; set; }
    }
}
