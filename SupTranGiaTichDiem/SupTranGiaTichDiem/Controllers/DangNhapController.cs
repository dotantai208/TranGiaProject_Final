using SupTranGiaTichDiem.MaHoa;
using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SupTranGiaTichDiem.Controllers
{
    public class DangNhapController : Controller
    {
        // GET: DangNhap
        dataTranGiaEntities db=new dataTranGiaEntities();
      
     

      
        [HttpGet]
        public ActionResult Login()
        {
       
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string phone, string password)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = PasswordHashing.HashPassword(password);

                var account = db.Accounts.FirstOrDefault(a => a.phone == phone);

                if (account != null)
                {
                    if (hashedPassword==account.password)
                    {
                        // Đăng nhập thành công
                        var userCookie = new HttpCookie("UserInfo");
                        userCookie.Values["UserId"] = account.acc_id.ToString();
                        userCookie.Values["UserRole"] = account.role;
                        userCookie.Values["UserName"] = account.fullname;
                        userCookie.Expires = DateTime.Now.AddHours(1);
                        Response.Cookies.Add(userCookie);

                        if (account.role == "AD" || account.role == "NV")
                        {
                            return RedirectToAction("Index", "Admin", new { area = "Admin" });
                        }
                        else if (account.role == "KH")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        // Password sai
                        ModelState.AddModelError("", "Sai mật khẩu");
                        ViewData["Error"] = "Mật khẩu chưa đúng";
                    }
                }
                else
                {
                    // Số điện thoại không tồn tại
                    ModelState.AddModelError("", "Số điện thoại chưa đúng");
                    ViewData["Error"] = "Số điện thoại chưa đúng";
                }
            }

        
            return View();
        }




        public ActionResult Logout()
        {
            // Xóa cookie
            if (Request.Cookies["UserInfo"] != null)
            {
                var cookie = new HttpCookie("UserInfo");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}