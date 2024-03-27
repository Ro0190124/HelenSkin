using HelenSkin.Data.Access.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelenSkin.Model;
using HelenSkin.Data;
using Newtonsoft.Json;


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
        private bool CheckPhanQuyen()
        {
            var phanquyenCookie = HttpContext.Request.Cookies["PhanQuyen"];
            bool quyen = false;

            if (!string.IsNullOrEmpty(phanquyenCookie))
            {
                bool.TryParse(phanquyenCookie, out quyen);
            }

            return quyen;
        }


        public ActionResult Index(string searchString)
        {
            var phanquyen = CheckPhanQuyen();
            if (phanquyen)
            {
                var nguoiDung = _db.db_NGUOI_DUNG.Where(x => x.TrangThai == true && x.PhanQuyen == true).OrderByDescending(x => x.MaND).ToList();
                if (!string.IsNullOrEmpty(searchString))
                {
                    nguoiDung = nguoiDung.Where(x => x.TenTaiKhoan.Contains(searchString) || x.TenND.Contains(searchString) || x.SoDienThoai.Contains(searchString)).ToList();
                }
                return View(nguoiDung);
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }

        }

        // GET: NguoiDungController/Details/5
        public ActionResult Details(int id)
        {
            var quyen = CheckPhanQuyen();
            if (quyen)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }

        // GET: NguoiDungController/Create
        public ActionResult Create()
        {
            var phanquyen = CheckPhanQuyen();
            if (phanquyen)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
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
            var quyen = CheckPhanQuyen();
            if (quyen)
            {
                if (id == null || id == 0)
            {
                return NotFound();
            }
            NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.First(x => x.MaND == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            return View(nguoiDung);
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }

        // POST: NguoiDungController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NGUOI_DUNG nguoidung)
        {
            var existingPhone = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.SoDienThoai == nguoidung.SoDienThoai && x.MaND != nguoidung.MaND && x.TrangThai == true);
			var existingUsername = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == nguoidung.TenTaiKhoan && x.MaND != nguoidung.MaND && x.TrangThai == true);


            if (existingPhone != null)
			{
				ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại cho người dùng khác");
			}

			if (existingUsername != null)
			{
				ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại cho người dùng khác");
			}
			if (!ModelState.IsValid)
			{
				return View(nguoidung);
			}
			else
            {
                if (ModelState.IsValid)
                {
                   Console.WriteLine(nguoidung.PhanQuyen);
                    _db.db_NGUOI_DUNG.Update(nguoidung);
                    _db.SaveChanges();
                    TempData["ThongBao"] = "Sửa người dùng thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(nguoidung);
                }
            }
        }
        // GET: NguoiDungController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.MaND == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            else
            {
                nguoiDung.TrangThai = false;
                _db.db_NGUOI_DUNG.Update(nguoiDung);
                TempData["ThongBaoXoa"] = "Xóa người dùng thành công";
                _db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult ThongTinTK(int id)
        {
            var userIdCookieValue = HttpContext.Request.Cookies["ID"];
            int userId;

            if (!string.IsNullOrEmpty(userIdCookieValue) && int.TryParse(userIdCookieValue, out userId))
            {
                // Kiểm tra nếu id không khớp với userId từ cookie thì chuyển hướng về trang chủ
                if (id != userId)
                {
                    return RedirectToAction("Index", "Home");
                }

                NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.MaND == id);
                if (nguoiDung == null)
                {
                    return NotFound();
                }
                return View(nguoiDung); 
            }
            else
            {
                // Nếu không có cookie hoặc không thể chuyển đổi thành int, chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTinTK(NGUOI_DUNG nguoidung)
        {
            var existingPhone = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.SoDienThoai == nguoidung.SoDienThoai && x.MaND != nguoidung.MaND && x.TrangThai == true);
            var existingUsername = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == nguoidung.TenTaiKhoan && x.MaND != nguoidung.MaND && x.TrangThai == true);

            if (existingPhone != null)
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại");
            }

            if (existingUsername != null)
            {
                ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại");
            }
            if (!ModelState.IsValid)
            {
                return View(nguoidung);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _db.db_NGUOI_DUNG.Update(nguoidung);
                    _db.SaveChanges();
                    TempData["ThongBao"] = "Sửa tài khoản thành công";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

    }
}
