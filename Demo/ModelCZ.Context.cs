﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Demo
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dtczEntities1 : DbContext
    {
        public dtczEntities1()
            : base("name=dtczEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<t_vehicle_msg_today> t_vehicle_msg_today { get; set; }
        public DbSet<t_vehicle_overweight_temp> t_vehicle_overweight_temp { get; set; }
        public DbSet<t_vehicle_overweight> t_vehicle_overweight { get; set; }
        public DbSet<station_site> station_site { get; set; }
        public DbSet<t_vehicle_msg> t_vehicle_msg { get; set; }
        public DbSet<t_vehicle_msg_abnormal> t_vehicle_msg_abnormal { get; set; }
    }
}
