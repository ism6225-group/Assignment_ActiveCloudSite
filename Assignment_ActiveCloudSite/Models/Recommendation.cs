using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_ActiveCloudSite.Models
{
    public class Recommendation
    {
        
        public float? consensusEndDate { get; set; }
        public float? consensusStartDate { get; set; }
        public float? corporateActionsAppliedDate { get; set; }
        public float? ratingBuy { get; set; }
        public float? ratingHold { get; set; }
        public float? ratingNone { get; set; }
        public float? ratingOverweight { get; set; }
        public float? ratingScaleMark { get; set; }
        public float? ratingSell { get; set; }
        public float? ratingUnderweight { get; set; }

    }
}
