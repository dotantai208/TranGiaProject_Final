using Microsoft.AspNet.Identity;
using SupTranGiaTichDiem.MaHoa;
using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;


namespace SupTranGiaTichDiem.Controllers
{
    public class HomeController : BaseController
    {
      
        public ActionResult Index()
        {
            var listSanPham = db.Rewards.OrderByDescending(r => r.reward_date).Take(4).ToList();
       
            return View(listSanPham);
        }




        public ActionResult ListSanPham(int? maloai, int page = 1, int pageSize = 6)
        {
            db.Database.ExecuteSqlCommand("EXEC UpdateTempProductCategory");

            var data = db.TempProductCategories.ToList();
            ViewBag.Category = data;

            // Lấy cookie của người dùng
            var userCookie = Request.Cookies["UserInfo"];
           

            var acc_id = userCookie.Values["UserId"];
            if (!int.TryParse(acc_id, out int accIdInt))
            {
                return RedirectToAction("Login", "DangNhap");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var user = db.Accounts.Find(accIdInt);
            if (user == null)
            {
                return RedirectToAction("Login", "DangNhap");
            }

            // Lấy điểm của người dùng
            var userPoints = user.Point;
         
         
            IQueryable<Reward> rewardsQuery = db.Rewards;

            if (maloai.HasValue)
            {
                rewardsQuery = rewardsQuery.Where(r => r.category_id == maloai.Value);
                ViewBag.CategoryId = maloai.Value;
            }

            // Lọc sản phẩm dựa trên điểm của người dùng
            rewardsQuery = rewardsQuery.Where(r => r.points_required <= userPoints);

            var listAllSanPham = rewardsQuery
                                 .OrderByDescending(r => r.reward_date)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            int totalItems = rewardsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
         
            return View(listAllSanPham);
        }

        public ActionResult ChiTietSanPham(int Reward_id)
        {
            
            var product = db.Rewards
                            .Include("Category")
                            .FirstOrDefault(r => r.reward_id == Reward_id);

            if (product == null)
            {
                return HttpNotFound();
            }
          
            return View(product);
        }
        public ActionResult Top3SanPham()
        {
            var top3SanPham = db.Rewards.OrderByDescending(r => r.reward_date).Take(3).ToList();
            return PartialView("_Top3ListSanPham", top3SanPham);
        }



    }
}