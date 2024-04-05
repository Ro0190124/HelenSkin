using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Http;

namespace HelenSkin.Model
{
    public class SAN_PHAM
    {
        [Key]
        public int MaSP { get; set; }
        [MaxLength(100, ErrorMessage = "Tên sản phẩm không quá 100 kí tự")]
        [MinLength(5, ErrorMessage = "Tên sản phẩm không dưới 5 kí tự")]
        [Required(ErrorMessage = "Tên sản phẩm Không được trống")]
        [RegularExpression(@"^[\p{L}a-zA-Z0-9\s]*$", ErrorMessage = "Tên sản phẩm không chứa kí tự đặc biệt")]

        public string TenSP { get; set; }

        [Range(0.01, Double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Giá chỉ chấp nhận kí tự số")]
        [Required(ErrorMessage = "Giá không được trống")]
        public double Gia { get; set; }

        [Range(0.01, Double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Số lượng chỉ chấp nhận kí tự số")]
        [Required(ErrorMessage = "Số lượng không được trống")]
        public int SoLuong { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; } = true;
        [MaxLength(5000, ErrorMessage = "Mô tả không quá 5000 kí tự")]
        public string MoTa { get; set; }

        public int MaDanhMuc { get; set; }
        [ValidateNever]
        [ForeignKey("MaDanhMuc")]
        public DANH_MUC DANH_MUC { get; set; }
        public virtual ICollection<DS_MEDIA_HINH_ANH>? db_DS_MEDIA_HINH_ANH { get; set; }
		[NotMapped]
		public IFormFile HinhAnhTaiLen { get; set; }
    }
}
