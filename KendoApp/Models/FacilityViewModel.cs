using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KendoApp.Models
{
    public class FacilityViewModel
    { 
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public Nullable<int> PharmacyId { get; set; }
        public String PharmacyName { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}