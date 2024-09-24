using Microsoft.AspNetCore.Mvc.Filters;
using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    
    public class TichDiemController : Controller
    {
        // GET: Admin/TichDiem
        public readonly dataTranGiaEntities db = new dataTranGiaEntities();
        

        public ActionResult TichDiem()
        {
           
            List<Account> account = db.Accounts.ToList();
            ViewBag.Account1 = new Account();
            return View(account);
        }


        [HttpPost]
        public ActionResult TichDiem(Account model)
        {
            var userCookie = Request.Cookies["UserInfo"];
            var acc_id = userCookie.Values["UserId"];
            if (!int.TryParse(acc_id, out int accIdInt))
            {
            }
            var existingAccount = db.Accounts.FirstOrDefault(a => a.phone == model.phone );

            if (existingAccount != null)
            {
                // Display specific error message based on duplicate field
                if (existingAccount.phone == model.phone)
                {
                    ViewData["Error"] = "Số điện thoại đã tồn tại";
                    return RedirectToAction("TichDiem", new { message = "Số điện thoại đã tồn tại!" });

                }
                 

                // Re-render the form with error messages
                // Pass the account object back to the view
            }


            model.CreateBy = accIdInt;
            model.status = true;
            db.Accounts.Add(model);
            db.SaveChanges();
            
            return RedirectToAction("TichDiem", new { message = "Thêm thành công!" });
        }



        [HttpPost]
        public ActionResult CapNhat(Account account,int Point_Bonus)
        {
            var userCookie = Request.Cookies["UserInfo"];
            var acc_id =  userCookie.Values["UserId"];
            if (!int.TryParse(acc_id, out int accIdInt))
            {
                 
            }
            ViewBag.Bonus = Point_Bonus;

            var updateAcc = db.Accounts.Find(account.acc_id);

            var diemBanDau = updateAcc.Point;
            var diemSauKhiDoi= account.Point;
            var diemThayDoi = diemBanDau - diemSauKhiDoi;

            PointHistory point = new PointHistory();
            point.his_date = DateTime.Now;
            point.his_note = null;
            point.his_pointchange = Point_Bonus;
            point.his_acc_id = accIdInt; // sửa bằng session id đăng nhập

            var PointHistory = db.PointHistories.ToList();
            PointHistory.Add(point);
            db.PointHistories.Add(point);
 
            updateAcc.UpdateDate = DateTime.Now;
            updateAcc.UpdateBy = accIdInt;
            updateAcc.Point = account.Point + Point_Bonus;
            db.SaveChanges();
            
            return RedirectToAction("TichDiem", new { message = "Cập nhật thành công!" });
        }


        public ActionResult TichDiemThapCao()
        {
          //  var dsNV = new NhanVien().ListNhanVien().OrderBy(x => x.DIEM).ToList();

            return View();
        }

        public ActionResult TichDiemCaoThap()
        {
          //  var dsNV = new NhanVien().ListNhanVien().OrderByDescending(x => x.DIEM).ToList();

            return View();
        }
        // Tích Điểm End ----------------------------------------s

        // Thêm mới 

     
 
        // Thêm mới END ------------------------------------------------
    }
}