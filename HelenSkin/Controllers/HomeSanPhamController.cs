using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelenSkin.Controllers
{
	public class HomeSanPhamController : Controller
	{
		// GET: HomeSanPhamController
		public ActionResult Index()
		{
			return View();
		}

		// GET: HomeSanPhamController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: HomeSanPhamController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: HomeSanPhamController/Create
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

		// GET: HomeSanPhamController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: HomeSanPhamController/Edit/5
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

		// GET: HomeSanPhamController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: HomeSanPhamController/Delete/5
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
