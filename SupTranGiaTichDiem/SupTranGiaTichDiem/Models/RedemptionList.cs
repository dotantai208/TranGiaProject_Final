using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupTranGiaTichDiem.Models
{
    public class RedemptionList
    {
        public int RedemptionId { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public double? PointsRequired { get; set; }
        public string RewardName { get; set; }
        public bool? Status { get; set; }


    }
}