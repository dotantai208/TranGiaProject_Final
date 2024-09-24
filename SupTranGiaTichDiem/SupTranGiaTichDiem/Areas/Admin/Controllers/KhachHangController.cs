using SupTranGiaTichDiem.MaHoa;
using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        public readonly dataTranGiaEntities data = new dataTranGiaEntities();
        // GET: Admin/KhachHang
        public ActionResult KhachHang()
        {
            List<Branch> branch = data.Branches.ToList();
            ViewBag.branch = branch;
            ViewBag.i = 1;
            List<Account> khachHang = data.Accounts.Where(a => a.role == "KH").ToList();

            return View(khachHang);
        }

        [HttpPost]
        public ActionResult createKH(Account account)
        {
            var userCookie = Request.Cookies["UserInfo"];
            var acc_id = userCookie.Values["UserId"];
            if (!int.TryParse(acc_id, out int accIdInt))
            {

            }


            if (ModelState.IsValid)
            {
                // Check for unique phone number and email in a single query
                var existingAccount = data.Accounts.FirstOrDefault(a => a.phone == account.phone || a.Email == account.Email);

                if (existingAccount != null)
                {
                    // Display specific error message based on duplicate field
                    if (existingAccount.phone == account.phone)
                    {

                        return RedirectToAction("KhachHang", new { message = "Số điện thoại đã tồn tại!" });

                    }
                    else
                    {

                        return RedirectToAction("KhachHang", new { message = "Email đã tồn tại!" });
                    }

                    // Re-render the form with error messages
                    // Pass the account object back to the view
                }

                account.CreateDate = DateTime.Now;
                account.CreateDate?.ToString("dd-MM-yyyy");
                account.status = true;
                account.Point = 0;
                account.password = PasswordHashing.HashPassword(account.password);
                account.CreateBy = accIdInt;

                data.Accounts.Add(account);
                data.SaveChanges();
                return RedirectToAction("KhachHang", new { message = "Thêm thành công!" });
            }
            return RedirectToAction("KhachHang");

        }

        public ActionResult GetKhachHang(int id)
        {
            var account = data.Accounts.Find(id);

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
        public ActionResult UpdateKhachHang(Account account)
        {
            var userCookie = Request.Cookies["UserInfo"];
            var acc_id = userCookie.Values["UserId"];
            if (!int.TryParse(acc_id, out int accIdInt))
            {

            }

            if (ModelState.IsValid)
            {
                var existingAccount = data.Accounts.Find(account.acc_id);
                if (existingAccount != null)
                {

                    existingAccount.phone = account.phone;
                    existingAccount.fullname = account.fullname;
                    existingAccount.Email = account.Email;
                    existingAccount.birthday = account.birthday;
                    existingAccount.role = account.role;
                    existingAccount.Branch = account.Branch;
                    existingAccount.status = account.status.GetValueOrDefault();

                    existingAccount.UpdateDate = DateTime.Now;

                    data.SaveChanges();

                    return RedirectToAction("KhachHang", new { message = "Cập nhật thành công!" });
                }

                return RedirectToAction("KhachHang", new { message = "Không tìm thấy nhân viên!" });
            }
            else
            {
                // Log the model state errors to diagnose issues
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return RedirectToAction("KhachHang", new { message = "Dữ liệu không hợp lệ! " + string.Join(", ", errors) });
            }
        }


        [HttpPost]
        public ActionResult DeleteKhachHang(Account account)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = data.Accounts.Find(account.acc_id);
                if (existingAccount != null)
                {

                    data.Accounts.Remove(existingAccount);
                    data.SaveChanges();
                    return RedirectToAction("KhachHang", new { message = "Xóa thành công!" });
                }
                return RedirectToAction("KhachHang", new { message = "Không tìm thấy khách hàng!" });
            }
            else
            {
                return RedirectToAction("KhachHang", new { message = "Dữ liệu không hợp lệ!" });
            }

        }



    }


}