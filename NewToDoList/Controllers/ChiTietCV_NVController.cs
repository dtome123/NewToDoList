using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewToDoList.Models;

namespace NewToDoList.Controllers
{
    public class ChiTietCV_NVController : Controller
    {
        private QLCVEntities db = new QLCVEntities();

        // GET: ChiTietCV_NV
        public ActionResult Index()
        {
            var chiTietCV_NV = db.ChiTietCV_NV.Include(c => c.CongViec).Include(c => c.NhanVien);
            return View(chiTietCV_NV.ToList());
        }

        // GET: ChiTietCV_NV/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietCV_NV chiTietCV_NV = db.ChiTietCV_NV.Find(id);
            if (chiTietCV_NV == null)
            {
                return HttpNotFound();
            }
            return View(chiTietCV_NV);
        }

        // GET: ChiTietCV_NV/Create
        public ActionResult Create()
        {
            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe");
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen");
            return View();
        }

        // POST: ChiTietCV_NV/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,MaCV,TrangThai")] ChiTietCV_NV chiTietCV_NV)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietCV_NV.Add(chiTietCV_NV);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe", chiTietCV_NV.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", chiTietCV_NV.MaNV);
            return View(chiTietCV_NV);
        }

        // GET: ChiTietCV_NV/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietCV_NV chiTietCV_NV = db.ChiTietCV_NV.Find(id);
            if (chiTietCV_NV == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe", chiTietCV_NV.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", chiTietCV_NV.MaNV);
            return View(chiTietCV_NV);
        }

        // POST: ChiTietCV_NV/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,MaCV,TrangThai")] ChiTietCV_NV chiTietCV_NV)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietCV_NV).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe", chiTietCV_NV.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", chiTietCV_NV.MaNV);
            return View(chiTietCV_NV);
        }

        // GET: ChiTietCV_NV/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietCV_NV chiTietCV_NV = db.ChiTietCV_NV.Find(id);
            if (chiTietCV_NV == null)
            {
                return HttpNotFound();
            }
            return View(chiTietCV_NV);
        }

        // POST: ChiTietCV_NV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietCV_NV chiTietCV_NV = db.ChiTietCV_NV.Find(id);
            db.ChiTietCV_NV.Remove(chiTietCV_NV);
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
