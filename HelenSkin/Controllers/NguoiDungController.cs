using HelenSkin.Data.Access.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelenSkin.Model;
using HelenSkin.Data;


namespace HelenSkin.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NguoiDungController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: NguoiDungController
        public ActionResult Index()
        {
            var users = from u in _db.db_NGUOI_DUNG // lấy toàn bộ liên kết
                        select u;
            return View(users);
        }

        // GET: NguoiDungController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NguoiDungController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NguoiDungController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NGUOI_DUNG nguoidung)
        {
			var m = _db.db_NGUOI_DUNG.Where(x => x.SoDienThoai == nguoidung.SoDienThoai && x.TrangThai == true).FirstOrDefault();
			if (m != null)
			{
				ModelState.AddModelError("SoDienThoai", " Số điện thoại đã tồn tại");
				return View(nguoidung);
			}
			var n = _db.db_NGUOI_DUNG.Where(x => x.TenTaiKhoan == nguoidung.TenTaiKhoan && x.TrangThai == true).FirstOrDefault();
			if (n != null)
			{
				ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại");
				return View(nguoidung);
			}
			else
			{
				Console.WriteLine(nguoidung.MaND);
				if (ModelState.IsValid)
				{

					_db.db_NGUOI_DUNG.Add(nguoidung);
					_db.SaveChanges();
					TempData["ThongBao"] = "Thêm người dùng thành công";
					return RedirectToAction("Index", "NguoiDung");

				}
				else
				{
					return View(nguoidung);
				}
			}
		}

        // GET: NguoiDungController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NguoiDungController/Edit/5
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

        // GET: NguoiDungController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NguoiDungController/Delete/5
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
