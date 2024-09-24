using SupTranGiaTichDiem.Models;
using SupTranGiaTichDiem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using PagedList;
namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        private dataTranGiaEntities db = new dataTranGiaEntities();

        public ActionResult AccountAd(int page = 1, string search = "")
        {

            int pageSize = 10;
            // Trang hiện tại
           
            // Tổng số lượng tài khoản
            var totalAccounts = db.Accounts.Count();

            // Tổng số lượng tài khoản ở từng khu vực
            var accountsByBranch = db.Accounts
                .GroupBy(a => a.Branch1.Branch_Name)
                .Select(g => new BranchAccountCountViewModel
                {
                    BranchName = g.Key,
                    AccountCount = g.Count()
                })
                .ToList();

            // Tổng số lượng sản phẩm
            var totalProducts = db.Rewards.Count();

            // Danh sách phần thưởng được đổi nhiều nhất top 4
            var topRedeemedRewards = db.Redemptions
                .GroupBy(r => new { r.Reward.reward_name, r.Reward.reward_image })
                .Select(g => new RewardRedemptionsViewModel
                {
                    RewardName = g.Key.reward_name,
                    RewardImage = g.Key.reward_image,
                    TotalQuantityRedeemed = g.Sum(r => r.red_quantity)
                })
                .OrderByDescending(g => g.TotalQuantityRedeemed)
                .Take(4)
                .ToList();

            // Danh sách khách hàng tích lũy nhiều điểm nhất top
            var topCustomersByPoints = db.Accounts
                .OrderByDescending(a => a.Point)
                
                .Select(a => new AccountPointsViewModel
                {
                    FullName = a.fullname,
                    Phone = a.phone,
                    TotalPoints = a.Point
                })
                .ToList();
            var timKiem = new List<AccountPointsViewModel>();
          
            var items = topCustomersByPoints
                                 .Where(i => i.Phone.Contains(search)) // Áp dụng tìm kiếm
                                 .ToPagedList(page, pageSize); // Phân trang

            ViewBag.CurrentSearch = search;

            var topRedemptionsByAccount = db.Redemptions
                .GroupBy(r => new { r.Account.fullname, r.Account.phone })
                .Select(g => new AccountRedemptionsViewModel
                {
                    FullName = g.Key.fullname,
                    Phone = g.Key.phone,
                    TotalRedemptions = g.Count()
                })
                .OrderByDescending(g => g.TotalRedemptions)
                .Take(4)
                .ToList();



            // Đưa các kết quả thống kê vào ViewBag
            ViewBag.TotalAccounts = totalAccounts;
            ViewBag.AccountsByBranch = accountsByBranch;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TopRedeemedRewards = topRedeemedRewards;
            ViewBag.TopCustomersByPoints = topCustomersByPoints;
            ViewBag.TopRedemptionsByAccount = topRedemptionsByAccount;

            return View(items);
        }
        public ActionResult GetTopRewardsByMonth(int month, int year)
        {
            var topRewardsByMonth = db.Redemptions
                .Where(r => r.redemption_date.HasValue && r.redemption_date.Value.Month == month && r.redemption_date.Value.Year == year)
                .GroupBy(r => new { r.Reward.reward_name, r.Reward.reward_image })
                .Select(g => new TopRewardViewModel
                {
                    RewardName = g.Key.reward_name,
                    RewardImage = g.Key.reward_image,
                    TotalQuantityRedeemed = g.Sum(r => r.red_quantity)
                })
                .OrderByDescending(r => r.TotalQuantityRedeemed)
                .Take(4)
                .ToList();

            return Json(topRewardsByMonth, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopRewardsByYear(int year)
        {
            var topRewardsByYear = db.Redemptions
                .Where(r => r.redemption_date.HasValue && r.redemption_date.Value.Year == year)
                .GroupBy(r => new { r.Reward.reward_name, r.Reward.reward_image })
                .Select(g => new TopRewardViewModel
                {
                    RewardName = g.Key.reward_name,
                    RewardImage = g.Key.reward_image,
                    TotalQuantityRedeemed = g.Sum(r => r.red_quantity)
                })
                .OrderByDescending(r => r.TotalQuantityRedeemed)
                .Take(4)
                .ToList();

            return Json(topRewardsByYear, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMonthsAndYears()
        {
            var months = db.Redemptions
                           .Where(r => r.redemption_date.HasValue)
                           .Select(r => new { Month = r.redemption_date.Value.Month })
                           .Distinct()
                           .OrderBy(m => m.Month)
                           .ToList();

            var years = db.Redemptions
                          .Where(r => r.redemption_date.HasValue)
                          .Select(r => new { Year = r.redemption_date.Value.Year })
                          .Distinct()
                          .OrderByDescending(y => y.Year)
                          .ToList();

            return Json(new { Months = months, Years = years }, JsonRequestBehavior.AllowGet);
        }
      


    }
}
