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
    public class logsController : Controller
    {
        private QLCVEntities db = new QLCVEntities();

        // GET: logs
        public ActionResult Index()
        {
            var logs = db.logs.OrderByDescending(c=>c.id).Include(l => l.NhanVien);
            return View(logs.ToList());
        }
        //search
        [HttpGet]
        public ActionResult Search(string NBD, string NKT)
        {
                DateTime bd = Convert.ToDateTime(NBD);
                DateTime kt = Convert.ToDateTime(NKT);
                if (bd.CompareTo(kt) > 0)
                {
                    ViewBag.error="Ngày bắt đầu phải trước ngày kết thúc";
                    return View(db.logs.ToList());
                }
                var logs = db.logs.Where(c => DateTime.Compare(c.ThoiGian, bd) >= 0 && DateTime.Compare(c.ThoiGian, kt) <= 0).OrderByDescending(c => c.id).Include(l => l.NhanVien);
                if (logs == null)
                {
                    ViewBag.error = "Không tìm thấy";
                    return View(db.logs.ToList());
                }
                return View(logs.ToList());
            

        }
        // GET: logs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // GET: logs/Create
        public ActionResult Create()
        {
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen");
            return View();
        }

        // POST: logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,MaNV,HoatDong,Bang")] log log)
        {
            if (ModelState.IsValid)
            {
                db.logs.Add(log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", log.MaNV);
            return View(log);
        }

        // GET: logs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", log.MaNV);
            return View(log);
        }

        // POST: logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,MaNV,HoatDong,Bang")] log log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", log.MaNV);
            return View(log);
        }

        // GET: logs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            log log = db.logs.Find(id);
            db.logs.Remove(log);
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
