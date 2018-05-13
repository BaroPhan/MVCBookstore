using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookStore.Models;
using PagedList;
using PagedList.Mvc;

namespace MVCBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        dbQLBanSachDataContext data = new dbQLBanSachDataContext();

        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        // GET: BookStore
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var sachMoi = LaySachMoi(15);
            return View(sachMoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }   
        
        public ActionResult SPTheoChuDe(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }

        public ActionResult NhaXuatBan()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);
        }

        public ActionResult SPTheoNXB (int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
            
        }

        public ActionResult Details (int id)
        {
            var sach = from s in data.SACHes
                       where s.Masach == id
                       select s;
            return View(sach); 
        }
    }
}