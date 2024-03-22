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


        [HttpPost]
        public IActionResult DangNhap(string tenTaiKhoan, string matKhau)
        {
            var nguoiDung = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == tenTaiKhoan && x.MatKhau == matKhau && x.TrangThai == true);
            var CheckTK = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == tenTaiKhoan && x.TrangThai == true);
            var CheckMK = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.MatKhau == matKhau && x.TrangThai == true);
            if(CheckTK == null)
            {
                ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản không tồn tại");
                return View();
            }else if(CheckMK == null)
            {
                ModelState.AddModelError("MatKhau", "Mật khẩu không chính xác");
                return View();
            }else if (nguoiDung != null)
            {

                Response.Cookies.Append("ID", nguoiDung.MaND.ToString());

                //TempData.Add("TenNguoiDung", nguoiDung.TenTaiKhoan);
                return RedirectToAction("Index", "Home");
            }
            else
            {

                TempData["ThongBaoDangNhap"] = "Tên tài khoản hoặc mật khẩu không đúng";
                return View();

            }
        }







        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }





    }
}
