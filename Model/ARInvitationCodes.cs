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
    
    public partial class ARInvitationCodes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARInvitationCodes()
        {
            this.AdivserAudits = new HashSet<AdivserAudits>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsEnable { get; set; }
        public bool IsUse { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> FromCommunityUserId { get; set; }
        public string UsePeople { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdivserAudits> AdivserAudits { get; set; }
        public virtual CommunityUsers CommunityUsers { get; set; }
    }
}