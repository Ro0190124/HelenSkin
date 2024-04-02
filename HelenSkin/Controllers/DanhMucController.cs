using HelenSkin.Data.Access.Data;
using HelenSkin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelenSkin.Controllers
{
	public class DanhMucController : Controller
	{
		private readonly ApplicationDbContext _db;

		public DanhMucController(ApplicationDbContext db)
		{
			_db = db;
		}

		// GET: DanhMucController
		public ActionResult Index(string searchString)
		{
			var danhMuc = _db.db_DANH_MUC.ToList();
			if (!string.IsNullOrEmpty(searchString))
			{
				danhMuc = danhMuc.Where(x => x.TenDanhMuc.Contains(searchString) || x.MaDanhMuc.ToString().Contains(searchString)).ToList();
			}
			return View(danhMuc);
		}

		// GET: DanhMucController1/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: DanhMucController1/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: DanhMucController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(DANH_MUC danhMuc)
		{
			var existingDanhMuc = _db.db_DANH_MUC.FirstOrDefault(x => x.TenDanhMuc == danhMuc.TenDanhMuc);
			if (existingDanhMuc != null)
			{
				ModelState.AddModelError("TenDanhMuc", "Tên danh mục đã tồn tại");
				return View(danhMuc);
			}

			if (ModelState.IsValid)
			{
				_db.db_DANH_MUC.Add(danhMuc);
				_db.SaveChanges();
				TempData["ThanhCong"] = "Thêm danh mục thành công";
				return RedirectToAction("Index");
			}
			else
			{
				return View(danhMuc);
			}

		}

		// GET: DanhMucController1/Edit/5
		public ActionResult Edit(int id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			DANH_MUC danhMuc = _db.db_DANH_MUC.FirstOrDefault(x => x.MaDanhMuc == id);

			if (danhMuc == null)
			{
				return NotFound();
			}

			return View(danhMuc);
		}

		// POST: DanhMucController1/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				if (id == null || id == 0)
				{
					return NotFound();
				}

				DANH_MUC danhMuc = _db.db_DANH_MUC.FirstOrDefault(x => x.MaDanhMuc == id);

				if (danhMuc == null)
				{
					return NotFound();
				}

				// Cập nhật thuộc tính của danh mục dựa trên dữ liệu được gửi từ form
				danhMuc.TenDanhMuc = collection["TenDanhMuc"];

				_db.SaveChanges();

				TempData["ThanhCong"] = "Chỉnh sửa danh mục thành công";

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: DanhMucController1/Delete/5
		public ActionResult Delete(int id)
		{
			DANH_MUC danhMuc = _db.db_DANH_MUC.FirstOrDefault(x => x.MaDanhMuc == id);

			if (danhMuc == null)
			{
				return NotFound();
			}

			// Kiểm tra xem danh mục có chứa sản phẩm hay không
			bool containsProducts = _db.db_SAN_PHAM.Any(x => x.MaDanhMuc == id);

			if (containsProducts)
			{
				TempData["ThatBai"] = "Không thể xóa danh mục này vì nó chứa sản phẩm.";
				return RedirectToAction("Index");
			}

			_db.db_DANH_MUC.Remove(danhMuc);
			TempData["ThanhCong"] = "Xóa danh mục thành công";
			_db.SaveChanges();

			return RedirectToAction("Index");
		}


		// POST: DanhMucController1/Delete/5
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
