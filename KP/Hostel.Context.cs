﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HostelContainer : DbContext
    {
        public HostelContainer()
            : base("name=HostelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<StudSovietMember> StudSovietMembers { get; set; }
        public virtual DbSet<DutyFloorWatch> DutyFloorWatches { get; set; }
    }
}
