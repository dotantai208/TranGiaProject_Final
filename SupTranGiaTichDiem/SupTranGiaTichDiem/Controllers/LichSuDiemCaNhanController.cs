using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Controllers
{
    public class LichSuDiemCaNhanController : BaseController
    {
     
        public ActionResult Index()
        {


            var userId = (int)ViewBag.UserId;


            var pointHistory = db.PointHistories.Where(ph => ph.his_acc_id == userId).ToList();

            return View(pointHistory);
        }
    }
}