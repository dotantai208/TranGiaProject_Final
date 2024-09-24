using SupTranGiaTichDiem.Models;
using SupTranGiaTichDiem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Areas.Admin.Controllers
{
    public class SearchAndPageController : Controller
    {
        private dataTranGiaEntities db = new dataTranGiaEntities();
        public ActionResult AccountAd(string search, int page = 1, int pageSize = 10)
        {
            var query = db.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.fullname.Contains(search) || a.phone.Contains(search));
            }

            var totalItems = query.Count();

            var accounts = query
                .OrderByDescending(a => a.Point)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AccountPointsViewModel
                {
                    FullName = a.fullname,
                    Phone = a.phone,
                    TotalPoints = a.Point
                })
                .ToList();

            var viewModel = new PagedListViewModel<AccountPointsViewModel>
            {
                Items = accounts,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(viewModel);
        }

    }
}