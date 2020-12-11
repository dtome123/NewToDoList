using NewToDoList.code;
using NewToDoList.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewToDoList.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        QLCVEntities db = new QLCVEntities();
        private bool isAttached(string nameFile,int cv)
        {
            var result = from f in db.TapTins
                         where f.DuongDan.Equals(nameFile) && f.MaCV == cv
                         select f;
            if (result.FirstOrDefault() != null)
                return true;
            return false;
        }
        
        public ActionResult Index(int id)
        {
            List<ObjFile> ObjFiles = new List<ObjFile>();
            ViewBag.cv = id;
            //foreach (string strfile in Directory.GetFiles(Server.MapPath("~/Files")))
            //{
            //    FileInfo fi = new FileInfo(strfile);
            //    ObjFile obj = new ObjFile();
            //    obj.File = fi.Name;
            //    obj.Size = fi.Length;
            //    obj.Type = GetFileTypeByExtension(fi.Extension);
            //    if(isAttached(fi.Name,id))
            //    {
            //        ObjFiles.Add(obj);
            //    }
            //}
            ViewBag.title = db.CongViecs.Find(id).TieuDe;
            List<TapTin> taptins = new List<TapTin>();
            taptins = (from t in db.TapTins
                       where t.MaCV == id
                       select t).ToList();
            foreach(TapTin t in taptins)
            {
                var path = Server.MapPath("~/Files");
                var fullpath = Path.Combine(path, t.DuongDan);
                FileInfo fi = new FileInfo(fullpath);
                ObjFile obj = new ObjFile();
                obj.File = fi.Name;
                obj.Size = fi.Length;
                obj.Type = GetFileTypeByExtension(fi.Extension);
                obj.nv = t.NhanVienUp+"_" + t.NhanVien.HoTen;
                ObjFiles.Add(obj);
            }

            return View(ObjFiles);
        }

        public FileResult Download(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Files"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            db.SaveLog(SessionHelper.GetSession().id, "Đã down load file có tên: " + fileName + " cho công việc", "Tập tin");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private string GetFileTypeByExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".docx":
                case ".doc":
                    return "Microsoft Word Document";
                case ".xlsx":
                case ".xls":
                    return "Microsoft Excel Document";
                case ".txt":
                    return "Text Document";
                case ".jpg":
                case ".png":
                    return "Image";
                default:
                    return "Unknown";
            }
        }
        [HttpPost]
        public ActionResult Index(ObjFile doc,int cv)
        {
            CongViec c = db.CongViecs.Find(cv);
            ViewBag.title = c.TieuDe;
            foreach (var file in doc.files)
            {

                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                    TapTin t = new TapTin();
                    t.MaCV = cv;
                    t.DuongDan = file.FileName;
                    t.NhanVienUp = SessionHelper.GetSession().id;
                    db.TapTins.Add(t);
                    db.SaveLog(SessionHelper.GetSession().id, "Đã upfile có tên: " + fileName+" cho công việc "+c.TieuDe, "Tập tin");
                    db.SaveChanges();
                    file.SaveAs(filePath);
                }
            }
            TempData["Message"] = "files uploaded successfully";
            return RedirectToAction("Index/"+cv);
        }

    }
}
