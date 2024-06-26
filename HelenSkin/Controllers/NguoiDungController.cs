﻿using HelenSkin.Data.Access.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelenSkin.Model;
using HelenSkin.Data;
using Newtonsoft.Json;
using System.Net;
using Microsoft.EntityFrameworkCore;


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
		private int CalculateAge(DateTime birthDate)
		{
			DateTime today = DateTime.Today;
			int age = today.Year - birthDate.Year;
			if (birthDate > today.AddYears(-age))
			{
				age--;
			}
			return age;
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
					TempData["ThanhCong"] = "Thêm người dùng thành công";
					return RedirectToAction("Index", "NguoiDung");

				}
				else
				{
                    TempData["ThatBai"] = "Thêm người dùng không thành công";
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
			int age = CalculateAge(nguoidung.NgaySinh);


			if (existingPhone != null)
			{
				ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại cho người dùng khác");
			}

			if (existingUsername != null)
			{
				ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại cho người dùng khác");
			}
			if (age < 18)
			{
				ModelState.AddModelError("NgaySinh", "Tuổi phải lớn hơn 18");
				return View(nguoidung);
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
                    TempData["ThanhCong"] = "Sửa người dùng thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ThatBai"] = "Sửa người dùng không thành công";
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
                TempData["ThatBai"] = "Xóa người dùng thất bại";
                return NotFound();
            }
            else
            {
                nguoiDung.TrangThai = false;
                _db.db_NGUOI_DUNG.Update(nguoiDung);
                TempData["ThanhCong"] = "Xóa người dùng thành công";
                _db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult ThongTinTK()
        {
            var userIdCookieValue = HttpContext.Request.Cookies["ID"];
            int userId;

            if (!string.IsNullOrEmpty(userIdCookieValue) && int.TryParse(userIdCookieValue, out userId))
            {
                NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.Where(x => x.MaND == userId).First();
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
            // bool pq = bool.Parse(HttpContext.Request.Cookies["PhanQuyen"]);
            var idNguoiDung = HttpContext.Request.Cookies["ID"];
            // tìm ng dùng database
            NGUOI_DUNG nguoiDung = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.MaND == int.Parse(idNguoiDung)); 


            var existingPhone = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.SoDienThoai == nguoiDung.SoDienThoai && x.MaND != nguoiDung.MaND && x.TrangThai == true);
            var existingUsername = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == nguoiDung.TenTaiKhoan && x.MaND != nguoiDung.MaND && x.TrangThai == true);
			int age = CalculateAge(nguoidung.NgaySinh);

			if (existingPhone != null)
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại");
            }

            if (existingUsername != null)
            {
                ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại");
			}
			if (age < 18)
			{
				ModelState.AddModelError("NgaySinh", "Bạn phải đủ 18 tuổi");
				return View(nguoidung);
			}
			if (!ModelState.IsValid)
            {
                return View(nguoidung);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    nguoiDung.TenND = nguoidung.TenND;
                    nguoiDung.SoDienThoai = nguoidung.SoDienThoai;
                    nguoiDung.Email = nguoidung.Email;
                    nguoiDung.NgaySinh = nguoidung.NgaySinh;
                    nguoiDung.DiaChi = nguoidung.DiaChi;
                    _db.SaveChanges();
                    TempData["ThanhCong"] = "Sửa tài khoản thành công";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ThatBai"] = "Sửa tài khoản không thành công";
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult QuenMK()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuenMK(NGUOI_DUNG nguoidung)
        {
			NGUOI_DUNG user = _db.db_NGUOI_DUNG.FirstOrDefault(u =>	u.TenTaiKhoan == nguoidung.TenTaiKhoan && u.SoDienThoai == nguoidung.SoDienThoai && u.TrangThai == true);
			var CheckTK = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.TenTaiKhoan == nguoidung.TenTaiKhoan && x.TrangThai == true);
			var CheckSDT = _db.db_NGUOI_DUNG.FirstOrDefault(x => x.SoDienThoai == nguoidung.SoDienThoai && x.TrangThai == true);
			if (CheckTK == null)
			{
				ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản không tồn tại");// ổn kh bạn hay là để mai fix tiếp :<
				return View();
			}
            if(CheckSDT == null)
            {
				ModelState.AddModelError("SoDienThoai", "Số điện thoại không chính xác");// ổn kh bạn hay là để mai fix tiếp :<
				return View();
			}
			ModelState.Remove("TenND");
			if (ModelState.IsValid)
            {

                    // Update mật khẩu cho user tại đây
                    user.MatKhau = nguoidung.MatKhau;
                    _db.SaveChanges();
                    TempData["ThanhCong"] = "Đặt lại mật khẩu thành công";
                    return RedirectToAction("DangNhap", "Home");

            }
            else
            {
                TempData["ThatBai"] = "Sửa tài khoản thất bại";
                return View(nguoidung);
            }
        }

	}
}

