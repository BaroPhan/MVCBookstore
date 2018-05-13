using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookStore.Models;

namespace MVCBookStore.Controllers
{
    public class UserController : Controller
    {
        dbQLBanSachDataContext data = new dbQLBanSachDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();  
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var diachi = collection["DiachiKH"];
            var email = collection["Email"];
            var dienthoai = collection["DienthoaiKH"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["LoiTenKH"] = "Ho ten khach hang khong duoc de trong";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["LoiTenDN"] = "Phai nhap ten dang nhap";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["LoiMatkhau"] = "Phai nhap mat khau";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["LoiEmail"] = "Email khong duoc de trong";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["LoiDTKH"] = "Phai nhap dien thoai";
            }
            else
            {
                kh.HoTenKH = hoten;
                kh.TenDN = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                kh.DienthoaiKH = dienthoai;

                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["LoiTenDN"] = "Phai nhap ten dang nhap";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["LoiMatkhau"] = "Phai nhap mat khau";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TenDN == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index","BookStore");
                        
                }
                else
                    ViewBag.Thongbao = "Ten dang nhap hoac mat khau khong dung";
            }
            return View();
        }
    }
}