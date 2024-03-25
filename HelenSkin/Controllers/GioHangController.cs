using HelenSkin.Data.Access.Data;
using HelenSkin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HelenSkin.Controllers
{
	public class GioHangController : Controller
	{
		private readonly ApplicationDbContext _db;
		public GioHangController(ApplicationDbContext db)
		{
			
			_db = db;
		}
		// GET: GioHangController
		public ActionResult Index()
		{
			var cookie = Request.Cookies["ID"];
			// check cookie
			Console.WriteLine(cookie);
			if (cookie == null)
			{
				TempData["tbGioHang"] = "Vui lòng đăng nhập để xem giỏ hàng";

			}
			else
			{
				NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(cookie)).First();
				TempData["tbGioHang"] = "Xin chào " + nguoiDung.TenND + "!";
				IEnumerable<GIO_HANG> obj;
				/*if(id == null || id == 0)
				{
					obj = _db.db_GIO_HANG.Include(x=> x.NGUOI_DUNG).ToList();
				}
				else
				{
					obj = _db.db_GIO_HANG.Include(x => x.NGUOI_DUNG).Where(x=> x.MaNguoiDung == id).ToList();
				}*/
				obj = _db.db_GIO_HANG.Include(x => x.NGUOI_DUNG).Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList();

			}
			
			return View() ;
		}

        // GET: GioHangController/Details/5
        // GET: GioHangController/Details/5
        public ActionResult ChiTietGioHang()
        {
            var cookie = Request.Cookies["ID"];
            // check cookie
            Console.WriteLine(cookie);
			if (cookie == null)
			{
                TempData["tbGioHang"] = "Vui lòng đăng nhập để xem giỏ hàng";
                return RedirectToAction("Index", "Home");
			}
			else
			{
                IEnumerable<CHI_TIET_GIO_HANG> gioHang = _db.db_CHI_TIET_GIO_HANG.Include(x => x.GIO_HANG)
                                                                          .Where(x => x.GIO_HANG.MaNguoiDung == int.Parse(cookie))
                                                                          .Include(x => x.SAN_PHAM)
                                                                          .ToList();
              

                if (gioHang == null)
                {
                    return NotFound();
                }

                List<string> firstImages = new List<string>();
                foreach (var item in gioHang)
                {
                    var firstImage = _db.db_DS_MEDIA_HINH_ANH.FirstOrDefault(x => x.MaSP == item.SAN_PHAM.MaSP);
                    if (firstImage != null)
                    {
                        firstImages.Add(firstImage.MediaHinhAnh);
                    }
                    else
                    {
                        // If no image is found, you can add a default image URL
                        firstImages.Add("/path/to/default/image.jpg");
                    }
                }

                // Pass both gioHang and firstImages to the view
                ViewBag.GioHang = gioHang;
                ViewBag.FirstImages = firstImages;

            }
          
           
            return View();
        }



        // GET: GioHangController/Create
        public ActionResult Create()
		{
			return View();
		}

		// POST: GioHangController/Create
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

		// GET: GioHangController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: GioHangController/Edit/5
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

		// GET: GioHangController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: GioHangController/Delete/5
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
