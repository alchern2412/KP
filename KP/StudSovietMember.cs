//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KP
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudSovietMember
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> DateOfEntry { get; set; }
        public string Position { get; set; }
    
        public virtual Student Student { get; set; }
    }
}
