using HelenSkin.Data.Access.Data;
using HelenSkin.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using System.Linq;

namespace HelenSkin.Controllers
{
	public class SanPhamController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public SanPhamController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
		{
			_db = db;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: ProductController
		[HttpGet]
        public IActionResult GetProductDetails(int id)
        {
            var product = _db.db_SAN_PHAM.Include(p => p.db_DS_MEDIA_HINH_ANH).FirstOrDefault(p => p.MaSP == id);

            if (product == null)
            {
                return NotFound();
            }

            var images = product.db_DS_MEDIA_HINH_ANH.Select(m => m.MediaHinhAnh).ToList();

            return Json(new
            {
                TenSP = product.TenSP,
                Gia = product.Gia,
                MoTa = product.MoTa,
                HinhAnhDaiDien = images.FirstOrDefault(),
                HinhAnh = images.Skip(1).ToList()
            });
        }
		public ActionResult Index(string searchString, string category, bool? status)
		{
			IQueryable<SAN_PHAM> sanPhams = _db.db_SAN_PHAM.Include(sp => sp.DANH_MUC).Include(sp => sp.db_DS_MEDIA_HINH_ANH);

			// Tìm kiếm theo tên sản phẩm
			if (!string.IsNullOrEmpty(searchString))
			{
				sanPhams = sanPhams.Where(sp => sp.TenSP.Contains(searchString));
			}

			// Tìm kiếm theo danh mục
			if (!string.IsNullOrEmpty(category))
			{
				sanPhams = sanPhams.Where(sp => sp.DANH_MUC.TenDanhMuc.Contains(category));
			}

			// Tìm kiếm theo trạng thái
			if (status.HasValue)
			{
				sanPhams = sanPhams.Where(sp => sp.TrangThai == status.Value);
			}

			// Kết thúc câu truy vấn và trả về kết quả
			return View(sanPhams.ToList());
		}


		public ActionResult ManHinhSP()
		{
            var sanPhams = _db.db_SAN_PHAM.Include(sp => sp.DANH_MUC).ToList();

            return View(sanPhams);
        }

		// GET: SanPhamController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: SanPhamController/Create
		public ActionResult Create()
		{
            HamGoiDanhM();

            return View();
		}
		public void HamGoiDanhM()
		{
			IEnumerable<SelectListItem> NCC = _db.db_DANH_MUC.Select(
				u => new SelectListItem()
				{
					Text = u.TenDanhMuc,
					Value = u.MaDanhMuc.ToString()
				}
			).ToList();
			ViewBag.DanhMuc = NCC;
		}

		// POST: SanPhamController/Create
		[HttpPost]
		public async Task<IActionResult> Create(SAN_PHAM sanpham, List<IFormFile> hinhAnhTaiLen)
		{
			HamGoiDanhM();
			if (ModelState.IsValid)
			{
				sanpham.NgayTao = DateTime.Now;
				sanpham.TrangThai = true;

				_db.db_SAN_PHAM.Add(sanpham);
				_db.SaveChanges();

				// Sau khi đã lưu sản phẩm, gán MaSP cho các hình ảnh và lưu chúng vào cơ sở dữ liệu
				foreach (var file in hinhAnhTaiLen)
				{
					if (file != null && file.Length > 0)
					{
						string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/product");
						string imageName = Guid.NewGuid().ToString() + "_" + file.FileName;
						string filePath = Path.Combine(uploadDir, imageName);

						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await file.CopyToAsync(stream);
						}

						// Tạo một đối tượng HinhAnh mới và gán MaSP của sản phẩm vừa được thêm vào
						DS_MEDIA_HINH_ANH anh = new DS_MEDIA_HINH_ANH();
						anh.MediaHinhAnh = imageName;
						anh.MaSP = sanpham.MaSP;
						_db.db_DS_MEDIA_HINH_ANH.Add(anh);
						_db.SaveChanges();
					}
				}


				TempData["ThanhCong"] = "Sửa sản phẩm thành công.";
				return RedirectToAction("Index");
			}

			TempData["ThatBai"] = "Sửa sản phẩm thất bại.";
			return View(sanpham);
		}

		public ActionResult Edit(int id)
		{
			HamGoiDanhM();

			var sanPham = _db.db_SAN_PHAM
				.Include(sp => sp.DANH_MUC)
				.Include(sp => sp.db_DS_MEDIA_HINH_ANH)
				.FirstOrDefault(sp => sp.MaSP == id);

			return View(sanPham);
		}

		// GET: SanPhamController/Edit/5
		[HttpPost]
		public async Task<IActionResult> Edit(int id, SAN_PHAM sanpham, List<IFormFile> hinhAnhTaiLen)
		{
			HamGoiDanhM(); 

			if (ModelState.IsValid)
			{
				// Lấy sản phẩm cần chỉnh sửa từ cơ sở dữ liệu
				var existingProduct = _db.db_SAN_PHAM.Include(sp => sp.db_DS_MEDIA_HINH_ANH).FirstOrDefault(sp => sp.MaSP == id);

				// Cập nhật thông tin của sản phẩm
				existingProduct.TenSP= sanpham.TenSP;
				existingProduct.Gia = sanpham.Gia;
				existingProduct.MoTa = sanpham.MoTa;
				existingProduct.MaDanhMuc = sanpham.MaDanhMuc;

				// Xử lý ảnh sản phẩm nếu có
				foreach (var file in hinhAnhTaiLen)
				{
					if (file != null && file.Length > 0)
					{
						string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/product");
						string imageName = Guid.NewGuid().ToString() + "_" + file.FileName;
						string filePath = Path.Combine(uploadDir, imageName);

						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await file.CopyToAsync(stream);
						}

						// Tạo một đối tượng HinhAnh mới và gán MaSP của sản phẩm vừa được thêm vào
						DS_MEDIA_HINH_ANH anh = new DS_MEDIA_HINH_ANH();
						anh.MediaHinhAnh = imageName;
						anh.MaSP = existingProduct.MaSP;
						existingProduct.db_DS_MEDIA_HINH_ANH.Add(anh);
					}
				}

				// Lưu thay đổi vào cơ sở dữ liệu
				_db.SaveChanges();

				TempData["ThanhCong"] = "Sửa sản phẩm thành công.";
				return RedirectToAction("Index");
			}

			TempData["ThatBai"] = "Sửa sản phẩm thất bại.";
			return View(sanpham);
		}

		// GET: SanPhamController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: SanPhamController/Delete/5
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
