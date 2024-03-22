using HelenSkin.Data.Access.Data;
using HelenSkin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelenSkin.Controllers
{
    public class ChiTietGioHangController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ChiTietGioHangController(ApplicationDbContext db)
        {

            _db = db;
        }
        // GET: ChiTietGioHangController
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> SP = _db.db_DS_MEDIA_HINH_ANH.Include(s => s.SAN_PHAM).Select(

               s => new SelectListItem()
               {
                   Text = s.SAN_PHAM.TenSP,
                   Value = s.MediaHinhAnh.FirstOrDefault().ToString()
               });
            ViewBag.SanPham = SP;
            IEnumerable<CHI_TIET_GIO_HANG> obj = _db.db_CHI_TIET_GIO_HANG.ToList();
            var gioHangChiTiet = _db.db_CHI_TIET_GIO_HANG
                         .Include(ctgh => ctgh.SAN_PHAM) // Bao gồm SAN_PHAM liên quan đến CHI_TIET_GIO_HANG

                         .ToList(); // trà về danh sách sản phẩm của tất cả giỏ hàng

            var dsMediaHinhAnh = _db.db_DS_MEDIA_HINH_ANH.Include(ha => ha.SAN_PHAM).ToList();
            /*foreach( var i in dsMediaHinhAnh){
              *//*  i.MediaHinhAnh = "/images/" + i.MediaHinhAnh;*//*
			  Console.WriteLine(i.MediaHinhAnh);
            }	*/


            return View(gioHangChiTiet);
           
        }

        // GET: ChiTietGioHangController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ChiTietGioHangController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChiTietGioHangController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChiTietGioHangController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChiTietGioHangController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChiTietGioHangController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChiTietGioHangController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
