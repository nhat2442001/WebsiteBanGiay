using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCSDL_WebBanGiay.Models;
namespace DoAnCSDL_WebBanGiay.Controllers
{
    public class HoaDonController : Controller
    {
        // GET: HoaDon
        DoAn_WebBanHangEntities db = new DoAn_WebBanHangEntities();
        public ActionResult TatCaHoaDon()
        {
            
            return View(db.DonDatHangs);
        }
        public ActionResult ChiTiet(string mahd)
        {
            return View(db.ChiTietDonDatHangs.Where(x=>x.MaDDH==mahd));
        }
    }
}