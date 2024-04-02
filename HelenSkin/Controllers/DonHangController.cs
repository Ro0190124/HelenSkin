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
        public ActionResult DaXacNhan()
        {
            var cookie = Request.Cookies["ID"];
            NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(cookie)).First();
            IEnumerable<HOA_DON> obj;

            obj = _db.db_HOA_DON.Where(x => x.TrangThai == 1).Include(x => x.GIO_HANG).ThenInclude(x => x.NGUOI_DUNG).ToList();


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
			TempData["tbDatHang"] = "Đã xác nhận đơn hàng";
			return RedirectToAction("Index", "DonHang");
		}

        public ActionResult Cancel(int id)
        {
            HOA_DON hoaDon = _db.db_HOA_DON.Where(x => x.MaHD == id).First();
            hoaDon.TrangThai = 4;
            _db.SaveChanges();
            TempData["tbDonHang"] = "Đã hủy đơn hàng";
            return RedirectToAction("Index", "DonHang");
        }
		public ActionResult ChiTietDonHang(int id)
		{
			var cookie = Request.Cookies["ID"];
            NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.Where(x => x.MaND == int.Parse(cookie)).First();
            // tìm hóa đơn theo id
			HOA_DON hoaDon = _db.db_HOA_DON.Where(x => x.MaHD == id).First();
			// lấy ra chi tiết giỏ hàng theo mã giỏ hàng
			IEnumerable<CHI_TIET_GIO_HANG> chiTietGioHang = _db.db_CHI_TIET_GIO_HANG.Include(x => x.GIO_HANG)
                                                                          .Where(x => x.GIO_HANG.MaGioHang == hoaDon.MaGioHang)
                                                                          .Include(x => x.SAN_PHAM)
                                                                          .ToList();
			//  xuất ra danh sách các sản phẩm có trong giỏ hàng
			foreach(var item in chiTietGioHang)
			{
                Console.WriteLine(item.SAN_PHAM.TenSP);
            }
			Console.WriteLine();
            List<string> firstImages = new List<string>();
            foreach (var item in chiTietGioHang)
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
            ViewBag.GioHang = chiTietGioHang;
            ViewBag.FirstImages = firstImages;
            return View(chiTietGioHang);
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

		public ActionResult DonHangND()
		{

			return View();
		}
		public ActionResult ChiTietDonHangND()
		{
			return View();
		}
	}
}
