using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnCSDL_WebBanGiay.Models;

namespace DoAnCSDL_WebBanGiay.Controllers
{
    public class ChinhSuaController : Controller
    {
        private DoAn_WebBanHangEntities db = new DoAn_WebBanHangEntities();

        // GET: ChinhSua
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.DongGiay).Include(s => s.NhaCungCap);
            return View(sanPhams.ToList());
        }

        // GET: ChinhSua/Details/5
        public ActionResult Details(int? id)
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
            return View(sanPham);
        }

        // GET: ChinhSua/Create
        public ActionResult Create()
        {
            ViewBag.MaDongGiay = new SelectList(db.DongGiays, "MaDongGiay", "TenDongGiay");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            return View();
        }

        // POST: ChinhSua/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,Gia,NgayCapNhatGia,MoTa,HinhAnh,LuotBinhChon,LuotBinhLuan,SoLanMua,HangMoi,MaNCC,MaDongGiay")] SanPham sanPham, HttpPostedFileBase HinhAnh,
            HttpPostedFileBase HinhAnh2, HttpPostedFileBase HinhAnh3)
        {
            var ncc = db.NhaCungCaps.Find(sanPham.MaNCC).TenNCC;   
            string direcpath = Server.MapPath("~/Content/images/" + ncc.ToString().ToLower() + "/" + sanPham.TenSP);
            DirectoryInfo directory = new DirectoryInfo(direcpath);
            directory.Create();
            if (ModelState.IsValid)
            {
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(HinhAnh.FileName);
                    sanPham.HinhAnh = "\\images\\" + ncc.ToString().ToLower() + "\\"+ sanPham.TenSP+"\\"+fileName;
                    HinhAnh.SaveAs(direcpath+"/" + fileName);
                    for(int i=0;i<100;i++)
                        HinhAnh2.SaveAs(direcpath + "/" + Path.GetFileName(HinhAnh2.FileName));
                    for (int i = 0; i < 250; i++)
                        HinhAnh3.SaveAs(direcpath + "/" + Path.GetFileName(HinhAnh3.FileName));
                }
              db.SanPhams.Add(sanPham);
              db.SaveChanges();
              return RedirectToAction("Index");
            }

            ViewBag.MaDongGiay = new SelectList(db.DongGiays, "MaDongGiay", "TenDongGiay", sanPham.MaDongGiay);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }

        // GET: ChinhSua/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.MaDongGiay = new SelectList(db.DongGiays, "MaDongGiay", "TenDongGiay", sanPham.MaDongGiay);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }

        // POST: ChinhSua/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,Gia,NgayCapNhatGia,MoTa,HinhAnh,LuotBinhChon,LuotBinhLuan,SoLanMua,HangMoi,MaNCC,MaDongGiay")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDongGiay = new SelectList(db.DongGiays, "MaDongGiay", "TenDongGiay", sanPham.MaDongGiay);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }

        // GET: ChinhSua/Delete/5
        public ActionResult Delete(int? id)
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
            return View(sanPham);
        }

        // POST: ChinhSua/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            string path = Server.MapPath("~/Content/images/" + sanPham.NhaCungCap.TenNCC.ToString().ToLower() + "/" + sanPham.TenSP);
            DirectoryInfo directory = new DirectoryInfo(path);
            directory.Delete(true);
            SoLuongSize sl = db.SoLuongSizes.Where(x => x.MaSP == id).FirstOrDefault();
            while (sl != null)
            {   
                db.SoLuongSizes.Remove(sl);
                db.SaveChanges();
                sl = db.SoLuongSizes.Where(x => x.MaSP == id).FirstOrDefault();
            }
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
