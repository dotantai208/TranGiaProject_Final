using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    public class DoiThuongController : Controller
    {
        // GET: Admin/DoiThuong
       public ActionResult DoiThuong()
{
    dataTranGiaEntities db = new dataTranGiaEntities();
    var ObjectRed = (from vw in db.vw_RedemptionDetails
                     select new RedemptionList
                     {  
                         RedemptionId = vw.redemption_id,
                         Phone = vw.phone,
                         FullName = vw.fullname,
                         PointsRequired = vw.points_required,
                         RewardName = vw.reward_name,
                         Status = vw.red_status
                     })
                     
                     .ToList();

    
    return View(ObjectRed);
}


        [HttpPost]
        public ActionResult CapNhat(Redemption redemp)
        {
            dataTranGiaEntities db = new dataTranGiaEntities();
            var updateRedemp = db.Redemptions.Find(redemp.red_acc_id);

            updateRedemp.red_status = redemp.red_status;
            db.SaveChanges();
            return RedirectToAction("DoiThuong");
        }


        public ActionResult TichDiemThapCao()
        {
            //  var dsNV = new NhanVien().ListNhanVien().OrderBy(x => x.DIEM).ToList();

            return View();
        }
    }
}
 