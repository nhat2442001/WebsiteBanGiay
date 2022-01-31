using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnCSDL_WebBanGiay.Models;

namespace DoAnCSDL_WebBanGiay.Controllers
{
    public class SoLuongSizesController : Controller
    {
        private DoAn_WebBanHangEntities db = new DoAn_WebBanHangEntities();

        // GET: SoLuongSizes
        public ActionResult Index()
        {
            var sp = db.SanPhams;
            return View(sp.ToList());
        }
        public ActionResult ChiTietSize(int MaSP)
        {           
            var soLuongSizes = db.SoLuongSizes.Where(x=>x.MaSP==MaSP);
            return View(soLuongSizes);
        }
        //GET: SoLuongSizes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoLuongSize soLuongSize = db.SoLuongSizes.Find(id);
            if (soLuongSize == null)
            {
                return HttpNotFound();
            }
            return View(soLuongSize);
        }

        // GET: SoLuongSizes/Create
        public ActionResult Create()
        {
            ViewBag.Size = db.BangSizes;
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP");
            return View();
        }

        // POST: SoLuongSizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSizeSP,MaSP,Size,SoLuong")] SoLuongSize soLuongSize)
        {
            if (ModelState.IsValid)
            {              
                db.SoLuongSizes.Add(soLuongSize);               
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Size = new SelectList(db.BangSizes, "Size", "ChuThich", soLuongSize.Size);
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", soLuongSize.MaSP);
            return View(soLuongSize);
        }

        // GET: SoLuongSizes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoLuongSize soLuongSize = db.SoLuongSizes.Find(id);
            if (soLuongSize == null)
            {
                return HttpNotFound();
            }
            var sp = db.SoLuongSizes.Find(id);
            ViewBag.Sp = sp;
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", soLuongSize.MaSP);
            return View(soLuongSize);
        }

        // POST: SoLuongSizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSizeSP,MaSP,Size,SoLuong")] SoLuongSize soLuongSize)
        {
            if (ModelState.IsValid)
            {
                db.Entry(soLuongSize).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Size = new SelectList(db.BangSizes, "Size", soLuongSize.Size);
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", soLuongSize.MaSP);
            return View(soLuongSize);
        }

        // GET: SoLuongSizes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoLuongSize soLuongSize = db.SoLuongSizes.Find(id);
            if (soLuongSize == null)
            {
                return HttpNotFound();
            }
            return View(soLuongSize);
        }

        // POST: SoLuongSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SoLuongSize soLuongSize = db.SoLuongSizes.Find(id);
            db.SoLuongSizes.Remove(soLuongSize);
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
