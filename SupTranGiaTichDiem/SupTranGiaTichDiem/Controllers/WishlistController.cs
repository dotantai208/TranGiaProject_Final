using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Controllers
{
    public class WishlistController : BaseController
    {
        // GET: Wishlist
        public ActionResult Index()
        {
            if (Session["AccId"] == null)
            {
                return RedirectToAction("Login", "DangNhap");
            }

            int accId = (int)Session["AccId"];
            var userWishlist = db.WishlistItems.Where(w => w.AccId == accId && w.status==true).ToList();
            ViewBag.UserPoints = db.Accounts.Find(accId).Point;
            return View(userWishlist);
        }

        // POST: Wishlist/Add
        [HttpPost]
        public ActionResult Add(int rewardId)
        {
            if (Session["AccId"] == null)
            {
                return RedirectToAction("Login", "DangNhap");
            }

            int accId = (int)Session["AccId"];
            var wishlistItem = db.WishlistItems.SingleOrDefault(w => w.RewardId == rewardId && w.AccId == accId);

            if (wishlistItem == null )
            {
                db.WishlistItems.Add(new WishlistItem
                {
                    AccId = accId,
                    RewardId = rewardId,
                    Quantity = 1,
                    status=true
                   
                });
            }
            else
            {
                if (wishlistItem.status == false)
                {
                    wishlistItem.Quantity = 1;
                    wishlistItem.status = true;

                }
                wishlistItem.Quantity++;
               
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Wishlist/Remove
        [HttpPost]
        public ActionResult Remove(int rewardId)
        {
            if (Session["AccId"] == null)
            {
                return RedirectToAction("Login", "DangNhap");
            }

            int accId = (int)Session["AccId"];
            var wishlistItem = db.WishlistItems.SingleOrDefault(w => w.RewardId == rewardId && w.AccId == accId);

            if (wishlistItem != null)
            {
                db.WishlistItems.Remove(wishlistItem);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST: Wishlist/UpdateQuantity
        [HttpPost]
        public ActionResult UpdateQuantity(int rewardId, int quantity)
        {
            if (Session["AccId"] == null)
            {
                return Json(new { success = false });
            }

            int accId = (int)Session["AccId"];
            var wishlistItem = db.WishlistItems.SingleOrDefault(w => w.RewardId == rewardId && w.AccId == accId);

            if (wishlistItem != null)
            {
                wishlistItem.Quantity = quantity;
              
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult ConfirmRedemption(int totalPoints, int remainingPoints )
        {
            if (Session["AccId"] == null)
            {
                return Json(new { success = false });
            }
           
            int accId = (int)Session["AccId"];
            var account = db.Accounts.Find(accId);
            var userWishlist = db.WishlistItems.Where(w => w.AccId == accId).ToList();
            if (account == null || account.Point < totalPoints)
            {
                return Json(new { success = false });
            }
            if (account.Point >= totalPoints)
            {
                foreach (var item in userWishlist)
                {
                    db.Redemptions.Add(new Redemption
                    {
                        redemption_date = DateTime.Now,
                        reward_id = item.RewardId,
                        red_acc_id = accId,
                        red_status = false,
                        red_quantity = item.Quantity
                       
                    });

                    item.status = false;
                }
                

                account.Point = remainingPoints;
                db.SaveChanges();
            }
           

            return Json(new { success = true });
        }
    }
}
