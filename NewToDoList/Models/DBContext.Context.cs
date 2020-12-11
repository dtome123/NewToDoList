﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewToDoList.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class QLCVEntities : DbContext
    {
        public QLCVEntities()
            : base("name=QLCVEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
        public virtual DbSet<ChiTietCV_NV> ChiTietCV_NV { get; set; }
        public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }
        public virtual DbSet<TapTin> TapTins { get; set; }
        public virtual DbSet<TrangThai> TrangThais { get; set; }
        public virtual DbSet<CongViec> CongViecs { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<log> logs { get; set; }
    
        public virtual int SaveLog(Nullable<int> manv, string hoatDong, string bang)
        {
            var manvParameter = manv.HasValue ?
                new ObjectParameter("manv", manv) :
                new ObjectParameter("manv", typeof(int));
    
            var hoatDongParameter = hoatDong != null ?
                new ObjectParameter("HoatDong", hoatDong) :
                new ObjectParameter("HoatDong", typeof(string));
    
            var bangParameter = bang != null ?
                new ObjectParameter("bang", bang) :
                new ObjectParameter("bang", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SaveLog", manvParameter, hoatDongParameter, bangParameter);
        }
    }
}
