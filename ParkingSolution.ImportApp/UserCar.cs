//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParkingSolution.ImportApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserCar
    {
        public int Id { get; set; }
        public string CarType { get; set; }
        public int UserId { get; set; }
        public string SeriesPartOne { get; set; }
        public string SeriesPartTwo { get; set; }
        public string RegistrationCode { get; set; }
        public int RegionCode { get; set; }
        public string Country { get; set; }
    
        public virtual User User { get; set; }
    }
}
