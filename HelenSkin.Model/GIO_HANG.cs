using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelenSkin.Model
{
    public class GIO_HANG
    {
        [Key]
        public int MaGioHang { get; set; }
        public int MaNguoiDung { get; set; }
        [ForeignKey("MaNguoiDung")]
        public NGUOI_DUNG? NGUOI_DUNG { get; set; }
    }
}
