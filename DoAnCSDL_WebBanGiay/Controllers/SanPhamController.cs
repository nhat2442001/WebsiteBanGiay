using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net;
using DoAnCSDL_WebBanGiay.Models;
using PagedList;
using System.IO;
namespace DoAnCSDL_WebBanGiay.Controllers
{
    public class SanPhamController : Controller
    {
        DoAn_WebBanHangEntities db = new DoAn_WebBanHangEntities();
        // GET: SanPham
        public ActionResult TatCaSanPham(string currentFilter, int? page,int currentNCC= 0, int currentDongGiay=0, int MaNCC = 0, int MaDongGiay = 0,string SearchString="")
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            List<SanPham> lstSP = new List<SanPham>();
            lstSP = db.SanPhams.OrderBy(x => x.TenSP).ToList();
            //lọc theo Hãng sản xuất
            if (currentNCC != 0)
                MaNCC = currentNCC;
            if (MaNCC != 0)
            {
                lstSP = db.SanPhams.Where(x => x.MaNCC == MaNCC).OrderBy(x => x.TenSP).ToList();
                currentNCC = MaNCC;
               
            }
            ViewBag.currentNCC = currentNCC;
            //lọc theo dòng giày
            if (currentDongGiay!=0)
                MaDongGiay = currentDongGiay;
            if (MaDongGiay != 0)
            {
                lstSP = db.SanPhams.Where(x => x.MaDongGiay == MaDongGiay).OrderBy(x => x.TenSP).ToList();
                currentDongGiay = MaDongGiay;
                
            }
            ViewBag.currentDongGiay = currentDongGiay;
            //lọc theo tên sản phẩm
            if (SearchString != "")
            {
                pageNumber = 1; 
                currentFilter = SearchString;
            }
            else
                SearchString = currentFilter;
            ViewBag.CurrentFilter = SearchString;
            if (currentFilter != null)
            {      
                lstSP = db.SanPhams.Where(x => x.TenSP.ToUpper().Contains(SearchString.ToUpper())).OrderBy(x => x.TenSP).ToList();
               
            }
            return View(lstSP.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChiTietSP(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            List<SanPham> sp = db.SanPhams.ToList();
            List<BangSize> bs = db.BangSizes.ToList();
            List<SoLuongSize> size = db.SoLuongSizes.ToList();
            var SLSize = from s in size
                         where s.MaSP == id
                         select s;
            db.SanPhams.Find(id).LuotBinhChon++;
            db.SaveChanges();
            ViewBag.Size =  SLSize;
            var NCC = db.NhaCungCaps.Find(sanPham.MaNCC);
            ViewBag.NCC = NCC;
            return View(sanPham);
        }
        //public ActionResult SanPhamPartial()
        //{

        //    return PartialView();

        //}
    }
}