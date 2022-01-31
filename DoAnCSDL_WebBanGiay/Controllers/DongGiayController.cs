using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCSDL_WebBanGiay.Models;
using System.Collections;
namespace DoAnCSDL_WebBanGiay.Controllers
{
    public class DongGiayController : Controller
    {
        DoAn_WebBanHangEntities db = new DoAn_WebBanHangEntities();
        // GET: DongGiay
        public ActionResult Index()
        {
            var loaiSP = db.DongGiays.ToList();
            Hashtable arrLoaiSP = new Hashtable();
            foreach (var item in loaiSP)
            {
                arrLoaiSP.Add(item.MaDongGiay, item.TenDongGiay);
            }
            ViewBag.LoaiSP = arrLoaiSP;
            return PartialView("Index");
        }
        public ActionResult HangGiay()
        {
            var NCC = db.NhaCungCaps.ToList();
            Hashtable arrLoaiSP = new Hashtable();
            foreach (var item in NCC)
            {
                arrLoaiSP.Add(item.MaNCC, item.TenNCC);
            }
            ViewBag.HangGiay = arrLoaiSP;
            return PartialView("HangGiay");
        }
    }
}