using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupTranGiaTichDiem.Models.ViewModels
{
    public class TopRewardViewModel
    {
        public string RewardName { get; set; }
        public string RewardImage { get; set; }
        public double? TotalQuantityRedeemed { get; set; }
    }
}