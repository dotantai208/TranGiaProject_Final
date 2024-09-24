using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: Admin/SanPham
        public readonly dataTranGiaEntities data = new dataTranGiaEntities();
        public ActionResult SanPham()
        {
            List<Reward> rewards = data.Rewards.ToList();
        
            return View(rewards);
        }

        public ActionResult SanPhamDoAn()
        {
            List<Reward> rewards = data.Rewards.Where(x => x.Category.category_id == 1).ToList();

            return View(rewards);
        }

        public ActionResult SanPhamDoUong()
        {

               List<Reward> rewards = data.Rewards.Where(x => x.Category.category_id == 2).ToList();
            

            return View(rewards);
        }


        [HttpPost]
        public ActionResult createSP(Reward reward, HttpPostedFileBase fileAnh)
        {
            var userCookie = Request.Cookies["UserInfo"];
            var acc_id = userCookie?.Values["UserId"];
            if (!int.TryParse(acc_id, out int accIdInt))
            {
                // Xử lý khi không parse được acc_id
            }

            if (fileAnh != null && fileAnh.ContentLength > 0)
            {
                // Lưu File Ảnh
                string rootFolder = Server.MapPath("~/img/image-SanPham");
                string fileName = Path.GetFileName(fileAnh.FileName);
                string pathImage = Path.Combine(rootFolder, fileName);
                fileAnh.SaveAs(pathImage);

                // Lưu thuộc tính ảnh url HinhAnh vào reward
                reward.reward_image =   fileName;
            }

            if (ModelState.IsValid)
            {
                // Lưu reward vào cơ sở dữ liệu
                data.Rewards.Add(reward);
                data.SaveChanges();

                return RedirectToAction("SanPham", new { message = "Thêm thành công!" });
            }

            // Xử lý khi ModelState không hợp lệ
            return RedirectToAction("SanPham");
        }



        public ActionResult GetSanPham(int id)
        {
            var reward = data.Rewards.Find(id);

            if (reward == null)
            {
                return HttpNotFound();
            }
            return Json(new
            {
                ID_R = reward.reward_id,
                name_R = reward.reward_name,
                point_R = reward.points_required,
                date_R = reward.reward_date?.ToString("yyyy-MM-dd"),
                image_R = reward.reward_image,
                description_R = reward.description,
                quantity_R = reward.quantity,
                status_R = reward.status,
                category_id_R = reward.category_id
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
       
        public ActionResult updateSP(Reward reward,HttpPostedFileBase fileAnh_Up)
        {
          //  var userCookie = Request.Cookies["UserInfo"];
         //   var acc_id = userCookie.Values["UserId"];
        //    if (!int.TryParse(acc_id, out int accIdInt))
        //    {

         //   }
            

            if (ModelState.IsValid)
            {
                var existingAccount = data.Rewards.Find(reward.reward_id);
                if (existingAccount != null)
                {
                    if (fileAnh_Up != null && fileAnh_Up.ContentLength > 0)
                    {
                        // Lưu File Ảnh
                        string rootFolder = Server.MapPath("~/img/image-SanPham");
                        string fileName = Path.GetFileName(fileAnh_Up.FileName);
                        string pathImage = Path.Combine(rootFolder, fileName);


                        fileAnh_Up.SaveAs(pathImage);

                        // Lưu thuộc tính ảnh url HinhAnh vào reward
                        existingAccount.reward_image = pathImage;
                    }
                    existingAccount.reward_name = reward.reward_name;
                    existingAccount.points_required = reward.points_required;
                     
                    existingAccount.description = reward.description;
                    existingAccount.quantity = reward.quantity;
                    existingAccount.status = reward.status;
                    existingAccount.category_id = reward.category_id;                   
                    existingAccount.reward_date = reward.reward_date;

                    data.SaveChanges();
                    return RedirectToAction("SanPham", new { message = "Cập nhật thành công!" });
                }
                return RedirectToAction("SanPham", new { message = "Không tìm thấy sản phẩm!" });
            }
            else
            {
                // Log the model state errors to diagnose issues
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return RedirectToAction("SanPham", new { message = "Dữ liệu không hợp lệ! " + string.Join(", ", errors) });
            }


        }


        [HttpPost]
        public ActionResult DeleteSP(Reward reward)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = data.Rewards.Find(reward.reward_id);
                if (existingAccount != null)
                {

                    data.Rewards.Remove(existingAccount);
                    data.SaveChanges();
                    return RedirectToAction("SanPham", new { message = "Xóa thành công!" });
                }
                return RedirectToAction("SanPham", new { message = "Không tìm thấy nhân viên!" });
            }
            else
            {
                return RedirectToAction("SanPham", new { message = "Dữ liệu không hợp lệ!" });
            }

        }

    }
}