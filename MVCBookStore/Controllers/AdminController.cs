using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookStore.Models;
using PagedList;
using PagedList.Mvc;

namespace MVCBookStore.Controllers
{
    public class AdminController : Controller
    {
        dbQLBanSachDataContext db = new dbQLBanSachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
           
            var tendn = collection["username"];
            var matkhau = collection["password"];
     
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";

            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải đăng nhập mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult Sach(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;

          
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }

        #region Themsach
        [HttpGet]
        public ActionResult ThemSach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fileUpload == null) 
            {
                ViewBag.ThongBao = "Chon anh bia";
                return View();
            }
            else
            {
                if (ModelState.IsValid) 
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/BookCover"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hinh anh da ton tai";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    sach.Hinhminhhoa = fileName;

                    db.SACHes.InsertOnSubmit(sach);
                    db.SubmitChanges();
                   
                }
                return RedirectToAction("Sach");
            }
        }
        #endregion

        #region Chitiet
        [HttpGet]
        public ActionResult Chitiet(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        #endregion

        #region Xoasach
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }
        #endregion

        #region Suasach
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            var s = db.SACHes.SingleOrDefault(n => n.Masach == sach.Masach);

            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Chon anh bia";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);

                    var path = Path.Combine(Server.MapPath("~/BookCover"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Anh da ton tai";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    s.Hinhminhhoa = fileName;
                    s.Tensach = sach.Tensach;
                    s.Dongia = sach.Dongia;
                    s.Donvitinh = sach.Donvitinh;
                    s.Ngaycapnhat = sach.Ngaycapnhat;
                    s.Mota = sach.Mota;
                    s.moi = sach.moi;
                    s.MaCD = sach.MaCD;
                    s.MaNXB = sach.MaNXB;
                    s.solanxem = sach.solanxem;
                    s.Soluongban = sach.Soluongban;

                    UpdateModel(sach);
                    db.SubmitChanges();                  
                }
                return RedirectToAction("Sach");
            }
        }
        #endregion
    }

}
