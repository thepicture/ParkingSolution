//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ParkingPlaceReservation
    {
        public int Id { get; set; }
        public int ParkingPlaceId { get; set; }
        public System.DateTime FromDateTime { get; set; }
        public Nullable<System.DateTime> ToDateTime { get; set; }
        public int UserId { get; set; }
        public bool IsPayed { get; set; }
    
        public virtual ParkingPlace ParkingPlace { get; set; }
        public virtual User User { get; set; }
    }
}
