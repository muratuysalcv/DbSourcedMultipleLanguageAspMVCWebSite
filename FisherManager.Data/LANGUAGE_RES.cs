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
    
    public partial class LANGUAGE_RES
    {
        public int RESOURCE_ID { get; set; }
        public string RESOURCE_KEY { get; set; }
        public string RESOURCE_VALUE { get; set; }
        public string LANGUAGE_CODE { get; set; }
    
        public virtual LANGUAGE LANGUAGE { get; set; }
    }
}
