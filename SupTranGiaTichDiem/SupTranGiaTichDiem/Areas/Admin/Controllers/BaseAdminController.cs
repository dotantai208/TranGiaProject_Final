using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        protected dataTranGiaEntities database;
        public BaseAdminController()
        {
            database = new dataTranGiaEntities();   
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
            var user = database.Accounts.Find(accIdInt);
            ViewBag.acc_id = user.acc_id;


            ViewBag.UserID = accIdInt;
            
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
    }
}