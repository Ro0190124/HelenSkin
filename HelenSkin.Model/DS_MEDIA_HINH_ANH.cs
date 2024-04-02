using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HelenSkin.Model
{
	public class DS_MEDIA_HINH_ANH
	{
		[Key]
		public int MaDSHinhAnh { get; set; }
		public string MediaHinhAnh { get; set; }
		public int MaSP { get; set; }
		[ForeignKey("MaSP")]
		public SAN_PHAM SAN_PHAM { get; set; }
	}
}
