using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelenSkin.Model
{
    public class DON_VI_VAN_CHUYEN
    {
        [Key]
        public int MaDonViVanChuyen { get; set; }
        [Required(ErrorMessage = "Không được trống")]
        [MaxLength(25)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên đơn vị vận chuyển chỉ được chứa ký tự và số.")]
        [MinLength(5, ErrorMessage = "Tên đơn vị vận chuyển không dưới 5 kí tự")]
        public string TenDonViVanChuyen { get; set; }
        [StringLength(10, ErrorMessage = "Số Điện thoại phải có 10 kí tự")]
        [MinLength(10, ErrorMessage = "Số Điện thoại phải có 10 kí tự")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Số điện thoại chỉ chứa số")]
        [Required(ErrorMessage = "Không được trống")]
        public string SoDienThoai { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9.,\s]+$", ErrorMessage = "Địa chỉ không được chứa ký tự đặc biệt.")]
        [MaxLength(150, ErrorMessage = "Địa chỉ không lớn hơn 150 kí tự ")]
        [MinLength(5, ErrorMessage = "Địa chỉ không dưới 5 kí tự")]
        public string DiaChi { get; set; }
    }
}
