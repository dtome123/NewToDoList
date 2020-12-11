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
    public class ChiTietCV_NVController : Controller
    {
        
        private QLCVEntities db = new QLCVEntities();

        // GET: ChiTietCV_NV
        public ActionResult Index()
        {
            
            var chiTietCV_NV = db.ChiTietCV_NV.Include(c => c.CongViec).Include(c => c.NhanVien).Include(c => c.TrangThai1);
            return View(chiTietCV_NV.ToList());
        }
        [HttpGet]
        public ActionResult Index(int id)
        {
            ViewBag.title = db.CongViecs.Find(id).TieuDe;
            ViewBag.id = id;
            var chiTietCV_NV = db.ChiTietCV_NV.Where(c=>c.MaCV==id).Include(c => c.CongViec).Include(c => c.NhanVien).Include(c => c.TrangThai1);
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
            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang");
            return View();
        }
        [HttpGet]
        public ActionResult Create(int id)
        {
            ViewBag.id = id;
            ViewBag.MaCV = new SelectList(db.CongViecs.Where(c=>c.MaCV==id), "MaCV", "TieuDe");
            List<int> temp = new List<int>();
            temp = (from t in db.ChiTietCV_NV
                    where t.MaCV == id
                    select t.MaNV).ToList();
            List<NhanVien> nhanviens = new List<NhanVien>();
            nhanviens = (from n in db.NhanViens
                        where !temp.Contains(n.MaNV)
                        select n).ToList();
                                 
            ViewBag.MaNV = new SelectList(nhanviens, "MaNV", "HoTen");
            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang");
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
                
                db.SaveLog(SessionHelper.GetSession().id, "Đã sửa thêm "+chiTietCV_NV.NhanVien.MaNV+"_"+chiTietCV_NV.NhanVien.HoTen+" vào danh sách cho công việc: " + chiTietCV_NV.MaCV + "_" + chiTietCV_NV.CongViec.TieuDe, "Công việc");
                return RedirectToAction("Index",new { id= chiTietCV_NV.MaCV});
            }

            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe", chiTietCV_NV.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", chiTietCV_NV.MaNV);
            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang", chiTietCV_NV.TrangThai);
            return View(chiTietCV_NV);
        }

        // GET: ChiTietCV_NV/Edit/5
        public ActionResult Edit(int? id,int? nv)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietCV_NV chiTietCV_NV = db.ChiTietCV_NV.Where(c=> c.MaCV==id && c.MaNV == nv).FirstOrDefault();
            if (chiTietCV_NV == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCV = new SelectList(db.CongViecs.Where(c => c.MaCV == id), "MaCV", "TieuDe", chiTietCV_NV.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens.Where(c=>c.MaNV==nv), "MaNV", "HoTen", chiTietCV_NV.MaNV);
            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang", chiTietCV_NV.TrangThai);
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
                
                db.SaveLog(SessionHelper.GetSession().id, "Đã cập nhật trạng thái công việc: " + chiTietCV_NV.MaCV + " cho công việc" + chiTietCV_NV.CongViec.TieuDe, "Công việc");
                return RedirectToAction("Index", new { id = chiTietCV_NV.MaCV });
            }
            ViewBag.MaCV = new SelectList(db.CongViecs, "MaCV", "TieuDe", chiTietCV_NV.MaCV);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", chiTietCV_NV.MaNV);
            ViewBag.TrangThai = new SelectList(db.TrangThais, "Ma", "TinhTrang", chiTietCV_NV.TrangThai);
            return View(chiTietCV_NV);
        }

        // GET: ChiTietCV_NV/Delete/5
        public ActionResult Delete(int? id, int? nv)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietCV_NV chiTietCV_NV = db.ChiTietCV_NV.Where(c => c.MaCV == id && c.MaNV == nv).FirstOrDefault();
            if (chiTietCV_NV == null)
            {
                return HttpNotFound();
            }
            return View(chiTietCV_NV);
        }

        // POST: ChiTietCV_NV/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int cv,int nv)
        {
            ChiTietCV_NV chiTietCV_NV = db.ChiTietCV_NV.Where(c => c.MaCV == cv && c.MaNV == nv).FirstOrDefault();
            db.ChiTietCV_NV.Remove(chiTietCV_NV);
            db.SaveChanges();
            db.SaveLog(SessionHelper.GetSession().id, "Đã xóa công việc của nhân viện: " + chiTietCV_NV.MaCV + " cho công việc" + chiTietCV_NV.CongViec.TieuDe, "Công việc");
            return RedirectToAction("Index", new { id = cv });
        }
        [HttpPost]
        public ActionResult DeleteOk(int cv, int nv)
        {
            ChiTietCV_NV chiTietCV_NV = db.ChiTietCV_NV.Where(c => c.MaCV == cv && c.MaNV == nv).FirstOrDefault();
            db.ChiTietCV_NV.Remove(chiTietCV_NV);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = cv });
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
