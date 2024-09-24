using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly dataTranGiaEntities db = new dataTranGiaEntities();
        // GET: Admin/NhanVien
        public ActionResult NhanVien()
        {
            List<Branch> branch = db.Branches.ToList();
            ViewBag.branch = branch;
             
            List<Account> ListNV = db.Accounts.Where(a => a.role == "nv" || a.role == "ad").ToList();

            return View(ListNV);

        }


        [HttpPost]
        public ActionResult createNV(Account account) 
        {
            var userCookie = Request.Cookies["UserInfo"];
            var acc_id = userCookie.Values["UserId"];
            if (!int.TryParse(acc_id, out int accIdInt))
            {

            }


            if (ModelState.IsValid)
            {
                // Check for unique phone number and email in a single query
                var existingAccount = db.Accounts.FirstOrDefault(a => a.phone == account.phone || a.Email == account.Email);

                if (existingAccount != null)
                {
                    // Display specific error message based on duplicate field
                    if (existingAccount.phone == account.phone)
                    {
                        ViewData["Error"] = "Số điện thoại đã tồn tại";
                        return RedirectToAction("NhanVien", new { message = "Số điện thoại đã tồn tại!" });

                    }
                    else
                    {
                        ViewData["Error"] = "Email đã tồn tại";
                        return RedirectToAction("NhanVien", new { message = "Email đã tồn tại!" });
                    }

                    // Re-render the form with error messages
                    // Pass the account object back to the view
                }
             



            account.CreateDate = DateTime.Now;
            account.status = true;
            account.CreateBy = accIdInt;
            account.Point = 0;
           
            db.Accounts.Add(account);
            db.SaveChanges();
             
                return RedirectToAction("NhanVien", new { message = "Thêm thành công!" });
            }

            return RedirectToAction("NhanVien");
        }



        public ActionResult GetNhanVien(int id)
        {
            var account = db.Accounts.Find(id);

            if (account == null)
            {
                return HttpNotFound();
            }
            return Json(new
            {
                ID = account.acc_id,
                Phone = account.phone,
                FullName = account.fullname,
                EmailAcc = account.Email,
                Birthday = account.birthday?.ToString("yyyy-MM-dd"),
                Role = account.role,
                BranchAcc = account.Branch,
                Status = account.status
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateNhanVien(Account account)
        {
            var userCookie = Request.Cookies["UserInfo"];
            var acc_id = userCookie.Values["UserId"];
            if (!int.TryParse(acc_id, out int accIdInt))
            {

            }

            if (ModelState.IsValid)
            {
                var existingAccount = db.Accounts.Find(account.acc_id);
                if (existingAccount != null)
                {
                     
                    existingAccount.phone = account.phone;
                    existingAccount.fullname = account.fullname;
                    existingAccount.Email = account.Email;
                    existingAccount.birthday = account.birthday;
                    existingAccount.role = account.role;
                    existingAccount.Branch = account.Branch;
                    existingAccount.status = account.status.GetValueOrDefault();
                    existingAccount.UpdateBy = accIdInt;
                    existingAccount.UpdateDate = DateTime.Now;

                    db.SaveChanges();
                    return RedirectToAction("NhanVien", new { message = "Cập nhật thành công!" });
                }
                return RedirectToAction("NhanVien", new { message = "Không tìm thấy nhân viên!" });
            }
            else
            {
                // Log the model state errors to diagnose issues
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return RedirectToAction("NhanVien", new { message = "Dữ liệu không hợp lệ! " + string.Join(", ", errors) });
            }


        }
        
        [HttpPost]
        public ActionResult DeleteNhanVien(Account account)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = db.Accounts.Find(account.acc_id);
                if (existingAccount != null)
                {

                    db.Accounts.Remove(existingAccount);
                    db.SaveChanges();
                    return RedirectToAction("NhanVien", new { message = "Xóa thành công!" });
                }
                return RedirectToAction("NhanVien", new { message = "Không tìm thấy nhân viên!" });
            }
            else
            {
                return RedirectToAction("NhanVien", new { message = "Dữ liệu không hợp lệ!" });
            }

        }

    }
}
    