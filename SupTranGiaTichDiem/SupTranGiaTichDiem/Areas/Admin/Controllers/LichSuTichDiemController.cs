using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    public class LichSuTichDiemController : Controller
    {
        public readonly dataTranGiaEntities data = new dataTranGiaEntities();
        // GET: Admin/Admin
        public ActionResult LichSuTichDiem()
        {
            var ObjectRed = (from vw in data.vw_HistoryPointChange
                             select new HistoryPointChange
                             {
                                 HisID = vw.his_id,
                                 Phone = vw.phone,
                                 FullName = vw.fullname,
                                 Date = vw.his_date,
                                 HisNote = vw.his_note,
                                 HisPointChange = vw.his_pointchange,
                                 Acc_ID = vw.acc_id
                             })

                   .ToList();
           

            return View(ObjectRed);
        }
    }
}