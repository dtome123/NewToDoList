using NewToDoList.code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewToDoList.Models
{
    public class AccountModel
    {
        public static string role;
        public static string id;
        public bool Veryfy(string id, string password)
        {
            using (QLCVEntities db = new QLCVEntities())
            {
                var result = db.NhanViens.Where(x => x.MaNV.ToString() == id);
                if (result.ToList().Count > 0)
                {
                    NhanVien nv = result.FirstOrDefault();
                    var result_pass = nv.Password;
                    
                    if (result_pass.Equals(password))
                    {
                        SessionHelper.SetSession(new UserSession() { id = Convert.ToInt16(nv.MaNV),name=nv.HoTen,role=nv.Quyen });
                        db.SaveLog(SessionHelper.GetSession().id,"đăng nhập vào website", "Login");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }

        }
    }
}