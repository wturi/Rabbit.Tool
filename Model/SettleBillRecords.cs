//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SettleBillRecords
    {
        public int SettleBillRecordId { get; set; }
        public string RecordNumber { get; set; }
        public string HandleEmpId { get; set; }
        public Nullable<int> AdviserId { get; set; }
        public Nullable<decimal> UseExperience { get; set; }
        public Nullable<decimal> UnUserExperience { get; set; }
        public Nullable<decimal> RestExperience { get; set; }
        public Nullable<decimal> CurAllExperience { get; set; }
        public Nullable<decimal> CumulativeUseExperience { get; set; }
        public Nullable<int> SettleType { get; set; }
        public string AccountNo { get; set; }
        public string OpeningBank { get; set; }
        public string OpeningName { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> FinishedDate { get; set; }
        public Nullable<bool> IsEnable { get; set; }
        public int SettleEumStatus { get; set; }
        public Nullable<int> SignNum { get; set; }
    }
}
