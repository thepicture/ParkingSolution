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
    
    public partial class Parking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parking()
        {
            this.ParkingPlace = new HashSet<ParkingPlace>();
        }
    
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int ParkingTypeId { get; set; }
        public System.TimeSpan BeforePaidTime { get; set; }
        public System.TimeSpan BeforeFreeTime { get; set; }
        public decimal CostInRubles { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual ParkingType ParkingType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParkingPlace> ParkingPlace { get; set; }
    }
}
