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
    
    public partial class Student : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.DutyFloorWatches = new HashSet<DutyFloorWatch>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string Note { get; set; }
        public Nullable<int> Course { get; set; }
        public Nullable<int> Group { get; set; }
        public System.DateTime Birthday { get; set; }
        public Nullable<System.DateTime> DateOfEntry { get; set; }
        public Nullable<System.DateTime> DateOfDeparture { get; set; }
        public string Photo { get; set; }
        public string Sex { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual Faculty Faculty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DutyFloorWatch> DutyFloorWatches { get; set; }
        public virtual StudSovietMember StudSovietMember { get; set; }

        public object Clone()
        {
            return new Student
            {
                FirstName = this.FirstName,
                SecondName = this.SecondName,
                LastName = this.LastName,
                Note = this.Note,
                Course = this.Course,
                Group = this.Group,
                Birthday = this.Birthday,
                DateOfEntry = this.DateOfEntry,
                DateOfDeparture = this.DateOfDeparture,
                Room = this.Room,
                Faculty = this.Faculty,
                DutyFloorWatches = this.DutyFloorWatches,
                StudSovietMember = this.StudSovietMember
            };
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName.Substring(0,1)}.";
        }
    }
}
