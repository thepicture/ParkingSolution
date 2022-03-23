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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParkingPlaceReservation()
        {
            this.PaymentHistory = new HashSet<PaymentHistory>();
        }
    
        public int Id { get; set; }
        public int ParkingPlaceId { get; set; }
        public System.DateTime FromDateTime { get; set; }
        public System.DateTime ToDateTime { get; set; }
        public int CarId { get; set; }
        public bool IsPayed { get; set; }
    
        public virtual ParkingPlace ParkingPlace { get; set; }
        public virtual UserCar UserCar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentHistory> PaymentHistory { get; set; }
    }
}
