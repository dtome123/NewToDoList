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
    public class CongViecsController : Controller
    {
        private QLCVEntities db = new QLCVEntities();

        // GET: CongViecs
        public ActionResult Index()
        {
            return View(db.CongViecs.ToList());
        }

        // GET: CongViecs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongViec congViec = db.CongViecs.Find(id);
            if (congViec == null)
            {
                return HttpNotFound();
            }
            return View(congViec);
        }

        // GET: CongViecs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CongViecs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCV,TieuDe,NgayBatDau,NgayKetKhuc,TrangThai,PhamVi")] CongViec congViec)
        {
            if (ModelState.IsValid)
            {
                db.CongViecs.Add(congViec);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(congViec);
        }

        // GET: CongViecs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongViec congViec = db.CongViecs.Find(id);
            if (congViec == null)
            {
                return HttpNotFound();
            }
            return View(congViec);
        }

        // POST: CongViecs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCV,TieuDe,NgayBatDau,NgayKetKhuc,TrangThai,PhamVi")] CongViec congViec)
        {
            if (ModelState.IsValid)
            {
                db.Entry(congViec).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(congViec);
        }

        // GET: CongViecs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongViec congViec = db.CongViecs.Find(id);
            if (congViec == null)
            {
                return HttpNotFound();
            }
            return View(congViec);
        }

        // POST: CongViecs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CongViec congViec = db.CongViecs.Find(id);
            db.CongViecs.Remove(congViec);
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
        [HttpPost]
        public JsonResult AddComment(int macv ,string noidung)
        {
            bool status = true;
            UserSession user = SessionHelper.GetSession();
            BinhLuan b = new BinhLuan();
            b.MaCV = macv;
            b.MaNV = user.id;
            b.BinhLuan1 = noidung;
            b.created_at = DateTime.Now;
            string ten = db.NhanViens.Find(b.MaNV).HoTen;
            try
            {
                db.BinhLuans.Add(b);
                db.SaveChanges();
            }catch(Exception e)
            {
                status = false;
            }


            return Json(new
            {
                noidung = noidung,
                macv = macv,
                nhavien = user.id,
                ten = ten,
                status =status

            });
        }
    }
}
