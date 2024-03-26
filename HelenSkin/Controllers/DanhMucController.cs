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
		public ActionResult Index()
		{
			var danhMuc = _db.db_DANH_MUC.ToList();
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
				TempData["ThongBao"] = "Thêm danh mục thành công";
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
			return View();
		}

		// POST: DanhMucController1/Edit/5
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

		// GET: DanhMucController1/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
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
