using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupTranGiaTichDiem.Models.ViewModels
{
    public class AccountPointsViewModel
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public double? TotalPoints { get; set; }
    }
}