using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewToDoList.Models;
using NewToDoList.code;

namespace NewToDoList.Controllers
{
    [Authorize]
    public class BinhLuansController : Controller
    {
        
        private QLCVEntities db = new QLCVEntities();

        // GET: BinhLuans
        public ActionResult Index()
        {
            var binhLuans = db.BinhLuans.Include(b => b.CongViec).Include(b => b.NhanVien);
            return View(binhLuans.ToList());
        }

        // GET: BinhLuans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BinhLuan binhLuan = db.BinhLuans.Find(id);
            if (binhLuan == null)
            {
                return HttpNotFound();
            }
            return View(binhLuan);
        }

        // GET: BinhLuans/Create
        public ActionResult Create()
        {
            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe");
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen");
            return View();
        }

        // POST: BinhLuans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBL,MaCV,MaNV,BinhLuan1")] BinhLuan binhLuan)
        {
            if (ModelState.IsValid)
            {
                db.BinhLuans.Add(binhLuan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe", binhLuan.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", binhLuan.MaNV);
            return View(binhLuan);
        }

        // GET: BinhLuans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BinhLuan binhLuan = db.BinhLuans.Find(id);
            if (binhLuan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe", binhLuan.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", binhLuan.MaNV);
            return View(binhLuan);
        }

        // POST: BinhLuans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBL,MaCV,MaNV,BinhLuan1")] BinhLuan binhLuan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(binhLuan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe", binhLuan.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", binhLuan.MaNV);
            return View(binhLuan);
        }

        // GET: BinhLuans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BinhLuan binhLuan = db.BinhLuans.Find(id);
            if (binhLuan == null)
            {
                return HttpNotFound();
            }
            return View(binhLuan);
        }

        // POST: BinhLuans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BinhLuan binhLuan = db.BinhLuans.Find(id);
            db.BinhLuans.Remove(binhLuan);
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
        [HttpGet]
        public JsonResult addComment(string NoiDung)
        {
            BinhLuan b = new BinhLuan();
            //UserSession session = SessionHelper.GetSession();

            //b.MaNV = MaNV;
            //b.BinhLuan1 = NoiDung;
            //b.MaCV = MaCV;
            //try {
            //    db.BinhLuans.Add(b);
            //    db.SaveChanges();
            //    return true;
            //}
            //catch (Exception e)
            //{
            //    return false;
            //}
            return Json(new {
                data = NoiDung
            });
            
        }
    }
}
