using SupTranGiaTichDiem.MaHoa;
using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Controllers
{
    public class DangKyController : Controller
    {
        dataTranGiaEntities db = new dataTranGiaEntities();

     
        public ActionResult Register()
        {
            ViewBag.BranchList = new SelectList(db.Branches, "Branch_id", "Branch_Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKyAcc(Account model)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = db.Accounts.FirstOrDefault(a => a.phone == model.phone);
                if (existingAccount != null)
                {
                    ModelState.AddModelError("phone", "Số điện thoại đã được đăng ký.");
                    ViewBag.BranchList = new SelectList(db.Branches, "Branch_id", "Branch_Name");
                    return View("Register", model);
                }

              
                model.password = PasswordHashing.HashPassword(model.password);
                model.role = "KH";
                model.status = true;
                model.Point = 0;

                db.Accounts.Add(model);
                db.SaveChanges();
                TempData["RegisteredPhone"] = model.phone;


                return RedirectToAction("Login", "DangNhap");
            }

            TempData["Message"] = "Đăng Ký thất bại!";
            TempData["MessageType"] = "error";

            ViewBag.BranchList = new SelectList(db.Branches, "Branch_id", "Branch_Name");
            return View("Register", model);
        }


    }
}
