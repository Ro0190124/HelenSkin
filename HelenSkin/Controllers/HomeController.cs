using HelenSkin.Data.Access.Data;
using HelenSkin.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
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
           
            return View();
        }
        public ActionResult SanPhamHome()
        {
            var sanPhams = _db.db_SAN_PHAM.Include(sp => sp.DANH_MUC).ToList();

            return View(sanPhams);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult DangNhap()
        {
            ViewData["HideHeader"] = true;
            return View();

        }
        [HttpGet]
        public IActionResult DangXuat()
        {
            // Xóa cookie với key "ID"
            Response.Cookies.Delete("ID");
            Response.Cookies.Delete("PhanQuyen");
            TempData["ThanhCong"] = "Đã đăng xuất thành công";
            // Chuyển hướng người dùng đến trang chủ
            return RedirectToAction("DangNhap", "Home");
        }

        [HttpPost]
        public IActionResult DangNhap(string tenTaiKhoan, string matKhau , string action)
        {
            Console.WriteLine(tenTaiKhoan + " " + matKhau + " " + action); // nó bị mỗi tk có email với điachi == null thôi ( người dùng ms dk á) 
            if (action == "login")
            {
                var nguoiDung = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == tenTaiKhoan && x.MatKhau == matKhau && x.TrangThai == true);
                var CheckTK = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == tenTaiKhoan && x.TrangThai == true);
                var CheckMK = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.MatKhau == matKhau && x.TrangThai == true);
                if (CheckTK == null)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản không tồn tại");// ổn kh bạn hay là để mai fix tiếp :<
                    return View();
                }
                else if (CheckMK == null)
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu không chính xác");
                    return View();
                }
                else
                if (nguoiDung != null)
                {
                    HttpContext.Response.Cookies.Append("ID", nguoiDung.MaND.ToString());
                    HttpContext.Response.Cookies.Append("PhanQuyen", nguoiDung.PhanQuyen.ToString());
                    TempData["ThanhCong"] = "Đăng nhập thành công";
                    //TempData.Add("TenNguoiDung", nguoiDung.TenTaiKhoan);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
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
        public IActionResult DangKi(NGUOI_DUNG nguoiDung)
        {

            Console.WriteLine("Submit");
            ModelState.Remove("Email");
            ModelState.Remove("DiaChi");
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
                    TempData["ThanhCong"] = "Đăng kí thành công";
                    Console.WriteLine(TempData["TaoTKThanhCong"]);
                    return RedirectToAction("DangNhap", "Home");
                }
                else
                {
                    TempData["ThatBai"] = "Đăng kí thất bại";
                    return View();
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
