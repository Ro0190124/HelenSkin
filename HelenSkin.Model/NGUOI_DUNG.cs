using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelenSkin.Model
{
    public class NGUOI_DUNG
    {
        [Key]
        public int MaND { get; set; }

        [Required(ErrorMessage = "Không được trống")]
        [MinLength(5, ErrorMessage = "Tên người dùng không dưới 5 kí tự ")]
        [MaxLength(50, ErrorMessage = "Tên người dùng không lớn hơn 50 kí tự ")]
        [RegularExpression(@"^[\p{L}a-zA-Z\s]*$", ErrorMessage = "Chỉ cho phép nhập ký tự từ chữ.")]
        public string TenND { get; set; }

        [StringLength(10, ErrorMessage = "Số Điện thoại phải có 10 kí tự")]
        [MinLength(10, ErrorMessage = "Số Điện thoại phải có 10 kí tự")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Số điện thoại chỉ chứa số")]
        [Required(ErrorMessage = "Không được trống")]
        public string SoDienThoai { get; set; }



        [RegularExpression(@"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]+$", ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Không được trống")]
        [MaxLength(25)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên tài khoản chỉ được chứa ký tự và số.")]
        [Display(Name = "Tên tài khoản")]
        [MinLength(5, ErrorMessage = "Tên tài khoản không dưới 5 kí tự")]
        public string TenTaiKhoan { get; set; }



        [Required(ErrorMessage = "Không được trống")]
        [MaxLength(25, ErrorMessage = "Mật khẩu không lớn hơn 25 kí tự ")]
        [MinLength(5, ErrorMessage = "Mật khẩu không dưới 5 kí tự")]
        public string MatKhau { get; set; }

        [CustomValidation(typeof(NGUOI_DUNG), "KiemLoiNgaySinh")]

        public DateTime NgaySinh { get; set; }


        public DateTime NgayTao { get; set; } = DateTime.Now;


        //[RegularExpression(@"^[a-zA-Z0-9., \/\(\)'""]+$", ErrorMessage = "Địa chỉ không được chứa ký tự đặc biệt.")]
        [MaxLength(150, ErrorMessage = "Địa chỉ không lớn hơn 150 kí tự ")]
        [MinLength(5, ErrorMessage = "Địa chỉ không dưới 5 kí tự")]
        public string DiaChi { get; set; }



        public bool PhanQuyen { get; set; }//true = admin , false = nguoidung
        public bool TrangThai { get; set; } = true; //true = ton tai , false = nghi;
        public static ValidationResult KiemLoiNgaySinh(DateTime ngaySinh, ValidationContext context)
        {
            if (ngaySinh >= DateTime.Today)
            {
                return new ValidationResult("Ngày sinh phải nhỏ hơn ngày hiện tại");
            }

            return ValidationResult.Success;
        }
    }
}
