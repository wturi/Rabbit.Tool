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
    
    public partial class Rules
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rules()
        {
            this.CommunityToRules = new HashSet<CommunityToRules>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string StoryEnum { get; set; }
        public string RuleEnum { get; set; }
        public int ParentId { get; set; }
        public bool IsEnable { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityToRules> CommunityToRules { get; set; }
    }
}