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
    
    public partial class DoorOpenLogs
    {
        public int DoorOpenLogId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CommunityUserId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public bool IsSuccessed { get; set; }
        public int OpenWith { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> FinisheDate { get; set; }
        public Nullable<int> ResponseMessageId { get; set; }
        public string OpenId { get; set; }
    
        public virtual Broadcasts Broadcasts { get; set; }
        public virtual CommunityUsers CommunityUsers { get; set; }
        public virtual Machines Machines { get; set; }
    }
}