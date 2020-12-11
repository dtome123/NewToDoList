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
    public class CongViecsController : Controller
    {
        private QLCVEntities db = new QLCVEntities();

        // GET: CongViecs
        public ActionResult Index()
        {
            var congViecs = db.CongViecs.Include(c => c.TrangThai1);
            return View(congViecs.ToList());
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
            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang");
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
                if (DateTime.Compare(congViec.NgayBatDau,congViec.NgayKetKhuc)>0)
                {
                    ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang", congViec.TrangThai);
                    ModelState.AddModelError("NgayBatDau", "Ngày bắt đầu hơn ngày kết thúc");
                    return View(congViec);
                }
                db.CongViecs.Add(congViec);
                db.SaveChanges();
                db.SaveLog(SessionHelper.GetSession().id, "Đã tạo một công việc mới", "Công việc");
                return RedirectToAction("Index");
            }

            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang", congViec.TrangThai);
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
            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang", congViec.TrangThai);
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
                db.SaveLog(SessionHelper.GetSession().id, "Đã sửa thông tin công việc: "+congViec.MaCV+"_"+congViec.TieuDe, "Công việc");
                return RedirectToAction("Index");
            }
            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang", congViec.TrangThai);
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
            db.SaveLog(SessionHelper.GetSession().id, "Đã xóa công việc: " + congViec.MaCV + "_" + congViec.TieuDe, "Công việc");
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
        public JsonResult AddComment(int macv, string noidung)
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
                CongViec c = db.CongViecs.Find(macv);
                db.SaveLog(SessionHelper.GetSession().id, "Đã thêm bình luận cho công việc: " +c.MaCV+"_"+c.TieuDe, "Công việc");
                db.SaveChanges();
            }
            catch (Exception e)
            {
                status = false;
            }


            return Json(new
            {
                noidung = noidung,
                macv = macv,
                nhavien = user.id,
                ten = ten,
                status = status

            });
        }

    }
}
