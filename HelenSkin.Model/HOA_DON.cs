using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelenSkin.Model
{
    public class HOA_DON
    {
        [Key]
        public int MaHD { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public byte TrangThai { get; set; } = 0;
        public int MaGioHang { get; set; }
        [ForeignKey("MaGioHang")]
        public GIO_HANG? GIO_HANG { get; set; }
        public int MaDonViVanChuyen { get; set; }
        [ForeignKey("MaDonViVanChuyen")]
        public DON_VI_VAN_CHUYEN? DON_VI_VAN_CHUYEN { get; set; }
    }
}
