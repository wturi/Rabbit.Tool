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
    
    public partial class FeedBacks
    {
        public int id { get; set; }
        public string Content { get; set; }
        public Nullable<int> CommunityUserID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool IsEnable { get; set; }
        public string Remark { get; set; }
    
        public virtual CommunityUsers CommunityUsers { get; set; }
    }
}