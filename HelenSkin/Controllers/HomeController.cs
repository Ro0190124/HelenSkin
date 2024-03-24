using HelenSkin.Data.Access.Data;
using HelenSkin.Model;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelenSkin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var userIdCookieValue = HttpContext.Request.Cookies["ID"];
            int userId;

            if (!string.IsNullOrEmpty(userIdCookieValue) && int.TryParse(userIdCookieValue, out userId))
            {
                var tennd = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.MaND == userId);
                // Tiếp tục xử lý dữ liệu...
                ViewBag.TenNguoiDung = tennd.TenND;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult DangNhap()
        {
            Response.Cookies.Delete("ID");
            ViewData["HideHeader"] = true;
            return View();

        }
        [HttpGet]
        public IActionResult DangXuat()
        {
            // Xóa cookie với key "ID"
            Response.Cookies.Delete("ID");

            // Chuyển hướng người dùng đến trang chủ
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DangNhap(string tenTaiKhoan, string matKhau , string action)
        {
            if(action == "login")
            {
                var nguoiDung = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == tenTaiKhoan && x.MatKhau == matKhau && x.TrangThai == true);
                var CheckTK = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == tenTaiKhoan && x.TrangThai == true);
                var CheckMK = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.MatKhau == matKhau && x.TrangThai == true);
                if (CheckTK == null)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản không tồn tại");
                    return View();
                }
                else if (CheckMK == null)
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu không chính xác");
                    return View();
                }
                else if (nguoiDung != null)
                {

                    HttpContext.Response.Cookies.Append("ID", nguoiDung.MaND.ToString());

                    //TempData.Add("TenNguoiDung", nguoiDung.TenTaiKhoan);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    TempData["ThongBaoDangNhap"] = "Tên tài khoản hoặc mật khẩu không đúng";
                    return View();

                }
            }
            else
            {
                return RedirectToAction("DangKi");
            }

        }



        public IActionResult DangKi()
        {
            ViewData["HideHeader"] = true;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangKi(NGUOI_DUNG nguoiDung)
        {
            Console.WriteLine("Submit");

            var n = _db.db_NGUOI_DUNG.Where(x => x.TenTaiKhoan == nguoiDung.TenTaiKhoan && x.TrangThai == true).FirstOrDefault();
            var m = _db.db_NGUOI_DUNG.Where(x => x.SoDienThoai == nguoiDung.SoDienThoai && x.TrangThai == true).FirstOrDefault();
            if (n != null)
            {
                ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại");
                return View(nguoiDung);
            }
            if (m != null)
            {
                ModelState.AddModelError("SoDienThoai", " Số điện thoại đã tồn tại");
                return View(nguoiDung);
            }
            else
            {
                if (ModelState.IsValid)
                {

                    _db.db_NGUOI_DUNG.Add(nguoiDung);
                    _db.SaveChanges();
                    TempData["ThongBao"] = "Đăng kí thành công";
                    return RedirectToAction("DangNhap", "Home");
                }
                else
                {
                    return View(nguoiDung);
                }
            }

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }





    }
}
