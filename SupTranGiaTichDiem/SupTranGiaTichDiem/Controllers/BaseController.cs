using SupTranGiaTichDiem.MaHoa;
using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Controllers
{
    public class BaseController : Controller
    {
       
        protected dataTranGiaEntities db;

        public BaseController()
        {
            db = new dataTranGiaEntities();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userCookie = Request.Cookies["UserInfo"];
            if (userCookie == null)
            {
                filterContext.Result = RedirectToAction("Login", "DangNhap");
                return;
            }

            var acc_id = userCookie.Values["UserId"];
            if (string.IsNullOrEmpty(acc_id))
            {
                filterContext.Result = RedirectToAction("Login", "DangNhap");
                return;
            }

            if (!int.TryParse(acc_id, out int accIdInt))
            {
                filterContext.Result = RedirectToAction("Login", "DangNhap");
                return;
            }

            var user = db.Accounts.Find(accIdInt);
            if (user == null)
            {
                filterContext.Result = RedirectToAction("Login", "DangNhap");
                return;
            }
            List<Branch> branch = db.Branches.ToList();

            ViewBag.branch = branch;

            System.Diagnostics.Debug.WriteLine($"Branch count: {branch.Count}");
            ViewBag.acc_id = user.acc_id;
            ViewBag.UserID = accIdInt;
            ViewBag.UserPoints = user.Point;
            Session["AccId"] = accIdInt;
            base.OnActionExecuting(filterContext);
        }

        protected bool IsAdminOrNV()
        {
            var userCookie = Request.Cookies["UserInfo"];
            if (userCookie == null)
            {
                return false;
            }
            var userRole = userCookie["UserRole"];
            return userRole == "AD" || userRole == "NV";
        }

        protected bool IsAdmin()
        {
            var userCookie = Request.Cookies["UserInfo"];
            if (userCookie == null)
            {
                return false;
            }
            var userRole = userCookie["UserRole"];
            return userRole == "AD";
        }

        // Method to get account details
        public ActionResult GetAccount(int id)
        {
            var account = db.Accounts.Find(id);
            List<Branch> branch = db.Branches.ToList();

            ViewBag.branch = branch;
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
                BranchAcc = account.Branch,
            }, JsonRequestBehavior.AllowGet);
        }

        // Method to update account details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAccount(int acc_id, string phone, string fullname, DateTime birthday, int Branch, string email)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = db.Accounts.Find(acc_id);
                if (existingAccount != null)
                {
                    existingAccount.phone = phone;
                    existingAccount.fullname = fullname;
                    existingAccount.Email = email;
                    existingAccount.birthday = birthday;
                    existingAccount.Branch = Branch;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home", new { message = "Cập nhật thành công!" });
                }
                return RedirectToAction("Index", "Home", new { message = "Không tìm thấy nhân viên!" });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return RedirectToAction("Index", "Home", new { message = "Dữ liệu không hợp lệ! " + string.Join(", ", errors) });
            }
        }

        // Method to change password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            var userCookie = Request.Cookies["UserInfo"];
            if (userCookie == null)
            {
                return RedirectToAction("Login", "DangNhap");
            }

            var acc_id = userCookie.Values["UserId"];
            if (string.IsNullOrEmpty(acc_id))
            {
                return RedirectToAction("Login", "DangNhap");
            }

            if (!int.TryParse(acc_id, out int accIdInt))
            {
                return RedirectToAction("Login", "DangNhap");
            }

            var user = db.Accounts.Find(accIdInt);
            if (user == null)
            {
                return RedirectToAction("Login", "DangNhap");
            }

            if (!VerifyHashedPassword(user.password, currentPassword))
            {
                TempData["ErrorMessage"] = "Mật khẩu hiện tại không đúng.";
                return RedirectToAction("Index", "Home");
            }

            if (newPassword != confirmNewPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới và xác nhận mật khẩu mới không khớp.";
                return RedirectToAction("Index", "Home");
            }

            user.password = PasswordHashing.HashPassword(newPassword);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công.";
            return RedirectToAction("Index", "Home");
        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            var hashedInputPassword = PasswordHashing.HashPassword(password);
            return hashedPassword == hashedInputPassword;
        }
    }
}
