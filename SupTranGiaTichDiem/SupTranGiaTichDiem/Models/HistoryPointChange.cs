using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupTranGiaTichDiem.Models
{
    public class HistoryPointChange
    {
        public int HisID { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public DateTime? Date { get; set; }
        public string HisNote { get; set; }
        public double? HisPointChange { get; set; }
        public int Acc_ID { get; set; }

    }
}