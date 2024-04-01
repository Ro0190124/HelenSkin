﻿using HelenSkin.Data.Access.Data;
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
				// tìm giỏ hàng của người dùng không chứa trong hóa đơn
				var gioHang = _db.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList();
				var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_db.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang)); // Trả về giỏ hàng của người dùng chưa có trong hóa đơn
				
				IEnumerable<CHI_TIET_GIO_HANG> chiTietGioHang = new List<CHI_TIET_GIO_HANG>();

                if (gioHangChuaCoTrongHoaDon != null)
				{
                    chiTietGioHang= _db.db_CHI_TIET_GIO_HANG.Include(x => x.GIO_HANG)
                                                                         .Where(x => x.GIO_HANG.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang)
                                                                         .Include(x => x.SAN_PHAM)
                                                                         .ToList();
                    Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
                    Console.WriteLine(chiTietGioHang.Count());
                    foreach (var item in chiTietGioHang)
                    {
                        Console.WriteLine("1");
                        Console.WriteLine(item.MaGioHang);
                        Console.WriteLine(item.SAN_PHAM.MaSP);
                    }

                }
				else
				{
					
                }

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
				if(gioHangChuaCoTrongHoaDon == null)
				{
					Console.WriteLine("Không có giỏ hàng");
				}
				else
				{
					Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
				}
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
        public ActionResult ThemVaoGioHang(int id, int quantity)
        {
            var cookie = Request.Cookies["ID"];
           // NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.MaND == int.Parse(cookie)); // Tìm người dùng

            // Kiểm tra cookie
            if (cookie == null)
            {
                TempData["tbGioHang"] = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng";
                return RedirectToAction("ManHinhSP", "SanPham");
            }
            else
            {
                // Kiểm tra mã giỏ hàng đã tồn tại trong hóa đơn hay chưa
                var gioHang = _db.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList(); // Trả về giỏ hàng của người dùng 

                // Lấy ra giỏ hàng của người dùng không có trong hóa đơn
                var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_db.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang));
                if (gioHangChuaCoTrongHoaDon == null) // nếu không có thì tạo mới
                {
                    // Tạo giỏ hàng mới
                    GIO_HANG newGioHang = new GIO_HANG();
                    newGioHang.MaNguoiDung = int.Parse(cookie);
                    _db.db_GIO_HANG.Add(newGioHang);
                    _db.SaveChanges();
                    // Lấy ra giỏ hàng mới tạo
					gioHangChuaCoTrongHoaDon = _db.db_GIO_HANG.FirstOrDefault(x => x.MaNguoiDung == int.Parse(cookie) && !_db.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang));
					Console.WriteLine("Mã Giỏ Hàng : ");
					Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
				}

                // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
                var chiTietGioHang = _db.db_CHI_TIET_GIO_HANG.FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang && x.MaSP == id);
                // Nếu sản phẩm đã có trong giỏ hàng thì cập nhật số lượng
                if (chiTietGioHang != null)
                {
                    chiTietGioHang.SoLuong += quantity;
                    _db.SaveChanges();
                    TempData["tbThemVaoGioHang"] = "Thêm vào giỏ hàng thành công!";
                }
                else
                {
                    // Nếu sản phẩm chưa có trong giỏ hàng thì thêm mới
                    CHI_TIET_GIO_HANG newChiTietGioHang = new CHI_TIET_GIO_HANG();
                    newChiTietGioHang.MaGioHang = gioHangChuaCoTrongHoaDon.MaGioHang;
                    newChiTietGioHang.MaSP = id;
                    newChiTietGioHang.SoLuong = quantity;
                    _db.db_CHI_TIET_GIO_HANG.Add(newChiTietGioHang);
                    _db.SaveChanges();
                    TempData["tbThemVaoGioHang"] = "Thêm vào giỏ hàng thành công!";
                }
            }
            return RedirectToAction("ManHinhSP", "SanPham"); 
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int itemId, int newQuantity)
        {
            //tìm giò hàng của người dùng đang sử dụng
			var cookie = Request.Cookies["ID"];
			var gioHang = _db.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList(); // Trả về giỏ hàng của người dùng
			var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_db.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang)); // Trả về giỏ hàng của người dùng chưa có trong hóa đơn
			Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
			// Tìm chi tiết giỏ hàng

			var chiTietGioHang = _db.db_CHI_TIET_GIO_HANG.FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang && x.MaSP == itemId);
			// Cập nhật số lượng
			chiTietGioHang.SoLuong = newQuantity;
			_db.SaveChanges();
            // Sau khi cập nhật, bạn có thể trả về một phản hồi, ví dụ:
            return Json(new { success = true, message = "Số lượng đã được cập nhật thành công." });
        }
        [HttpPost]
        public ActionResult DeleteProduct(int itemId)
        {
            // tìm giò hàng của người dùng đang sử dụng
			var cookie = Request.Cookies["ID"];
			var gioHang = _db.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList(); // Trả về giỏ hàng của người dùng
			var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_db.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang)); // Trả về giỏ hàng của người dùng chưa có trong hóa đơn
			Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
			// Tìm chi tiết giỏ hàng
			var chiTietGioHang = _db.db_CHI_TIET_GIO_HANG.FirstOrDefault(x => x.MaGioHang == gioHangChuaCoTrongHoaDon.MaGioHang && x.MaSP == itemId);
			// Xóa sản phẩm khỏi giỏ hàng
			_db.db_CHI_TIET_GIO_HANG.Remove(chiTietGioHang);
			_db.SaveChanges();
			TempData["XoaSPGH"] = "Xóa sản phẩm trong giỏ hàng thành công";
            return Json(new { success = true, message = "Sản phẩm đã được xóa khỏi giỏ hàng." });
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




		public ActionResult DatHang(string ghiChu)
		{
            var cookie = Request.Cookies["ID"];
		
            // check cookie
            Console.WriteLine(cookie);
            var gioHang = _db.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).ToList(); // Trả về giỏ hàng của người dùng
            var gioHangChuaCoTrongHoaDon = gioHang.FirstOrDefault(x => !_db.db_HOA_DON.Any(y => y.MaGioHang == x.MaGioHang)); // Trả về giỏ hàng của người dùng chưa có trong hóa đơn
           // Console.WriteLine(gioHangChuaCoTrongHoaDon.MaGioHang);
			if (gioHangChuaCoTrongHoaDon == null)
			{
                TempData["tbDatHangLoi"] = "Không có sản phẩm trong giò hàng";
                return RedirectToAction("ChiTietGioHang", "GioHang");
            }
			else
			{
				//kiểm tra gio
				//tạo hóa đơn
				HOA_DON hoaDon = new HOA_DON();
				hoaDon.MaGioHang = gioHangChuaCoTrongHoaDon.MaGioHang;
				hoaDon.NgayTao = DateTime.Now;
				hoaDon.TrangThai = 0 ;
				hoaDon.MaDonViVanChuyen = 1;
                Console.WriteLine(ghiChu);
                hoaDon.GhiChu = ghiChu;
				_db.db_HOA_DON.Add(hoaDon);
				_db.SaveChanges();
				TempData["tbDatHang"] = "Đặt hàng thành công!";
			}
			return RedirectToAction("Index", "Home");
		}
	}
}
