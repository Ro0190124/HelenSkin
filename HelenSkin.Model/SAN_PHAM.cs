using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelenSkin.Model
{
    public class SAN_PHAM
    {
        [Key]
        public int MaSP { get; set; }
        [MaxLength(100, ErrorMessage = "Tên sản phẩm không quá 40 kí tự")]
        [MinLength(5, ErrorMessage = "Tên sản phẩm không dưới 5 kí tự")]
        [Required(ErrorMessage = "Tên sản phẩm Không được trống")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Tên sản phẩm không được chứa kí tự đặc biệt")]
        public string TenSP { get; set; }

        [Range(0.01, Double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Giá chỉ chấp nhận kí tự số")]
        public double Gia { get; set; }

        [Range(0.01, Double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Số lượng chỉ chấp nhận kí tự số")]
        public int SoLuong { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; } = true;
        [MaxLength(1000, ErrorMessage = "Mô tả không quá 1000 kí tự")]
        public string MoTa { get; set; }

        public int MaDanhMuc { get; set; }

        [ForeignKey("MaDanhMuc")]
        public DANH_MUC DANH_MUC { get; set; }
    }
}
