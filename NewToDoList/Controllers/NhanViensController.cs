using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewToDoList.Models;
using System.Security.Cryptography;
using System.Text;
using NewToDoList.code;
using System.Text.RegularExpressions;

namespace NewToDoList.Controllers
{
    [Authorize]
    public class NhanViensController : Controller
    {
        
        private QLCVEntities db = new QLCVEntities();

        // GET: NhanViens
        public ActionResult Index()
        {
            var nhanViens = db.NhanViens.Include(n => n.PhanQuyen);
            return View(nhanViens.ToList());
        }
        public bool IsNumber(string t)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(t);
        }
        [HttpGet]
        public ActionResult Search(string words)
        {
            List<NhanVien> result = new List<NhanVien>();
            if (IsNumber(words))
            {
                 result= db.NhanViens.Where(c => c.MaNV.ToString().Contains(words)).ToList();
            }
            else
            {
                result = db.NhanViens.Where(c => c.HoTen.ToString().Contains(words)).ToList();
            }
            if (result == null)
            {
                ViewBag.SearchResult = "Không tìm thấy";
                return View(db.NhanViens.ToList());
            }
            else
            {
                ViewBag.SearchResult = "Có "+result.Count+" kết quả tìm được";
                return View(result.ToList());
            }
        }

        // GET: NhanViens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {
            ViewBag.Quyen = new SelectList(db.PhanQuyens, "MaQuyen", "Ten");
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,HoTen,NgaySinh,Email,Password,TrangThai,ChucVu,Quyen")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {

                nhanVien.Password = MD5Hash.hash(nhanVien.Password);
                db.NhanViens.Add(nhanVien);
                db.SaveLog(SessionHelper.GetSession().id, "Tạo mới 1 nhân viên", "Nhân viên");
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Quyen = new SelectList(db.PhanQuyens, "MaQuyen", "Ten", nhanVien.Quyen);
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.Quyen = new SelectList(db.PhanQuyens, "MaQuyen", "Ten", nhanVien.Quyen);
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,HoTen,NgaySinh,Email,Password,TrangThai,ChucVu,Quyen")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                db.SaveLog(SessionHelper.GetSession().id, "Sửa thông tin nhân viên mã số:"+nhanVien.MaNV, "Nhân viên");
                return RedirectToAction("Index");
            }
            ViewBag.Quyen = new SelectList(db.PhanQuyens, "MaQuyen", "Ten", nhanVien.Quyen);
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
            db.SaveChanges();
            db.SaveLog(SessionHelper.GetSession().id, "Đã xóa thông tin nhân viên mã số:" + nhanVien.MaNV, "Nhân viên");
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
        public string ChangePassword(int id,string oldPass, string newPass,string confPass)
        {
            NhanVien nv =db.NhanViens.Find(id);

            if (oldPass.Equals(""))
            {
                return "Mật khẩu cũ không được để trống";
            }
            if (newPass.Equals(""))
            {
                return "Mật khẩu mới không được để trống";
            }
            if (confPass.Equals(""))
            {
                return "Mật khẩu mới nhập không được để trống";
            }
            if (newPass != confPass)
            {
                return "Mật khẩu nhập lại không khớp với mật khẩu mới";
            }
            if (MD5Hash.hash(oldPass).Equals(nv.Password))
            {
                nv.Password = MD5Hash.hash(newPass);
                db.SaveChanges();
                db.SaveLog(SessionHelper.GetSession().id, "Đã đổi mật khẩu nhân viên mã số:" + id, "Nhân viên");
                return "Đổi mật khẩu thành công";
            }
            else
            {
                return "Mật khẩu hiện tại không chính xác";
            }

        }
       
    }
}
