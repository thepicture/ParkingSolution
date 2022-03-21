﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParkingSolution.WebAPI.Models.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ParkingBaseEntities : DbContext
    {
        public ParkingBaseEntities()
            : base("name=ParkingBaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Parking> Parking { get; set; }
        public virtual DbSet<ParkingPlace> ParkingPlace { get; set; }
        public virtual DbSet<ParkingPlaceReservation> ParkingPlaceReservation { get; set; }
        public virtual DbSet<ParkingType> ParkingType { get; set; }
        public virtual DbSet<PaymentHistory> PaymentHistory { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCar> UserCar { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
    }
}
