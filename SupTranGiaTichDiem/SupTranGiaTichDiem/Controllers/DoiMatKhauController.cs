using SupTranGiaTichDiem.MaHoa;
using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Controllers
{
    public class DoiMatKhauController : Controller
    {
        // GET: DoiMatKhau
        private readonly dataTranGiaEntities db = new dataTranGiaEntities();

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
                ViewBag.ErrorMessage = "Mật khẩu hiện tại không đúng.";
                return View("Index");
            }

            if (newPassword != confirmNewPassword)
            {
                ViewBag.ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu mới không khớp.";
                return View("Index");
            }

            user.password = PasswordHashing.HashPassword(newPassword);
            db.SaveChanges();

            ViewBag.SuccessMessage = "Đổi mật khẩu thành công.";
            return View("Index");
        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            var hashedInputPassword = PasswordHashing.HashPassword(password);
            return hashedPassword == hashedInputPassword;
        }
       
    }
}