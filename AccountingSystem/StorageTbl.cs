//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountingSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class StorageTbl
    {
        public int StorageID { get; set; }
        public int operatorIdFK { get; set; }
        public int articleIdFK { get; set; }
        public System.DateTime articleSetDate { get; set; }
        public int articleCount { get; set; }
        public long articlePrice { get; set; }
        public long totalPrice { get; set; }
        public System.DateTime articleExDate { get; set; }
        public string articleName { get; set; }
        public string articleCode { get; set; }
    
        public virtual ArticleTbl ArticleTbl { get; set; }
        public virtual userTbl userTbl { get; set; }
    }
}
