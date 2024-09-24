using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupTranGiaTichDiem.Models
{
    public class RedemptionViewModel
    {
        public Reward Reward { get; set; }
        public int Quantity { get; set; }
        public int TotalPointsRequired { get; set; }
        public int UserPoints { get; set; }
    }
}