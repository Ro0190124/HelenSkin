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
				//tìm giỏ hàng không có trong h
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
				ViewBag.MaGioHang = _db.db_GIO_HANG.Where(x => x.MaNguoiDung == int.Parse(cookie)).FirstOrDefault().MaGioHang;

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
                if (gioHangChuaCoTrongHoaDon == null)
                {
                    // Tạo giỏ hàng mới
                    GIO_HANG newGioHang = new GIO_HANG();
                    newGioHang.MaNguoiDung = int.Parse(cookie);
                    _db.db_GIO_HANG.Add(newGioHang);
                    _db.SaveChanges();
                    // Lấy ra giỏ hàng mới tạo
                    gioHangChuaCoTrongHoaDon = _db.db_GIO_HANG.FirstOrDefault(x => x.MaNguoiDung == int.Parse(cookie));
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
            return RedirectToAction("ManHinhSP", "SanPham"); ;
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




		public ActionResult DatHang(int? MaGioHang)
		{
			Console.WriteLine(MaGioHang);
			if (MaGioHang == null)
			{
				return NotFound();
			}
			else
			{
				//tạo hóa đơn
				HOA_DON hoaDon = new HOA_DON();
				hoaDon.MaGioHang = (int)MaGioHang;
				hoaDon.NgayTao = DateTime.Now;
				hoaDon.TrangThai = 0 ;
				hoaDon.MaDonViVanChuyen = 1;
				_db.db_HOA_DON.Add(hoaDon);
				_db.SaveChanges();
				TempData["tbDatHang"] = "Đặt hàng thành công!";
			}
			return RedirectToAction("Index", "Home");
		}
	}
}
