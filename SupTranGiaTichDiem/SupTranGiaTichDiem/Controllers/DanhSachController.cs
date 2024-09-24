using SupTranGiaTichDiem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupTranGiaTichDiem.Controllers
{
    public class DanhSachController : Controller
    {
        // GET: DanhSach
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Doithuong()
        {
            dataTranGiaEntities data = new dataTranGiaEntities();
            List<Reward> rewards = data.Rewards.ToList();

            return View(rewards);
        }

    }
}