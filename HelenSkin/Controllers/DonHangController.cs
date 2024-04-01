using HelenSkin.Data.Access.Data;
using HelenSkin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HelenSkin.Controllers
{
	public class DonHangController : Controller
	{
		private readonly ApplicationDbContext _db;
		public DonHangController(ApplicationDbContext db)
		{

			_db = db;
		}
		// GET: DonHangController
		public ActionResult Index()
		{
			var cookie = Request.Cookies["ID"];
			NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(cookie)).First();
			IEnumerable<HOA_DON> obj;
		
			obj = _db.db_HOA_DON.Where(x => x.TrangThai == 0).Include(x=> x.GIO_HANG).ThenInclude(x => x.NGUOI_DUNG).ToList();


			IEnumerable<CHI_TIET_GIO_HANG> gioHang = _db.db_CHI_TIET_GIO_HANG.Include(x => x.GIO_HANG)
																		  .Where(x => x.GIO_HANG.MaNguoiDung == int.Parse(cookie))
																		  .Include(x => x.SAN_PHAM)
																		  .ToList();
		
			List<double> total = new List<double>();

			foreach (var item in obj)
			{
				double totalPrice = 0;

				
				var prices = _db.db_CHI_TIET_GIO_HANG.Include(x => x.GIO_HANG)
													  .Where(x => x.GIO_HANG.MaGioHang == item.MaGioHang)
													  .Select(x => x.SAN_PHAM.Gia)
													  .ToList();

				foreach (var price in prices)
				{
					totalPrice += price;
				}

				total.Add(totalPrice);
			}

		
			ViewBag.TotalPrices = total;


			return View(obj);
		}
        public ActionResult ChoXacNhan()
        {
            var cookie = Request.Cookies["ID"];
            NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(cookie)).First();
            IEnumerable<HOA_DON> obj;

            obj = _db.db_HOA_DON.Where(x => x.TrangThai == 0).Include(x => x.GIO_HANG).ThenInclude(x => x.NGUOI_DUNG).ToList();


            IEnumerable<CHI_TIET_GIO_HANG> gioHang = _db.db_CHI_TIET_GIO_HANG.Include(x => x.GIO_HANG)
                                                                          .Where(x => x.GIO_HANG.MaNguoiDung == int.Parse(cookie))
                                                                          .Include(x => x.SAN_PHAM)
                                                                          .ToList();

            List<double> total = new List<double>();

            foreach (var item in obj)
            {
                double totalPrice = 0;


                var prices = _db.db_CHI_TIET_GIO_HANG.Include(x => x.GIO_HANG)
                                                      .Where(x => x.GIO_HANG.MaGioHang == item.MaGioHang)
                                                      .Select(x => x.SAN_PHAM.Gia)
                                                      .ToList();

                foreach (var price in prices)
                {
                    totalPrice += price;
                }

                total.Add(totalPrice);
            }


            ViewBag.TotalPrices = total;


            return View(obj);
        }


        // GET: DonHangController/Details/5
        public ActionResult Accept(int id)
		{
			HOA_DON hoaDon = _db.db_HOA_DON.Where(x => x.MaHD == id).First();
			hoaDon.TrangThai = 1;
			_db.SaveChanges();
            return RedirectToAction("Index", "DonHang");
        }

		// GET: DonHangController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: DonHangController/Create
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

		// GET: DonHangController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: DonHangController/Edit/5
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

		// GET: DonHangController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: DonHangController/Delete/5
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
