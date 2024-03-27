using HelenSkin.Data.Access.Data;
using HelenSkin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options; 

namespace HelenSkin.Controllers
{
	public class SanPhamController : Controller
	{
		private readonly ApplicationDbContext _db;

		public SanPhamController(ApplicationDbContext db)
		{
			_db = db;
		}

		// GET: ProductController
		public ActionResult Index()
		{
			var sanPhams = _db.db_SAN_PHAM.Include(sp => sp.DANH_MUC).ToList();		

			return View(sanPhams);
		}
        
        public ActionResult ManHinhSP()
		{
			return View();
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
		public ActionResult Create(SAN_PHAM sanpham, List<IFormFile> newImages, int? selectedDanhMuc)
		{
			if (ModelState.IsValid)
			{
				sanpham.NgayTao = DateTime.Now;
				sanpham.TrangThai = true;

				_db.db_SAN_PHAM.Add(sanpham);

				/*foreach (var image in newImages)
				{
					DS_MEDIA_HINH_ANH anh = new DS_MEDIA_HINH_ANH
					{
						MaSP = sanpham.MaSP,
						MaDSHinhAnh = image.FileName
					};

					_db.db_DS_MEDIA_HINH_ANH.Add(anh);
				}*/
				_db.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(sanpham);
		}
		// GET: SanPhamController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: SanPhamController/Edit/5
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
