using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelenSkin.Model;
using Microsoft.EntityFrameworkCore;


namespace HelenSkin.Data.Access.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<NGUOI_DUNG> db_NGUOI_DUNG { get; set; }
        public DbSet<SAN_PHAM> db_SAN_PHAM { get; set; }
        public DbSet<DANH_MUC> db_DANH_MUC { get; set; }
        public DbSet<DS_MEDIA_HINH_ANH> db_DS_MEDIA_HINH_ANH { get; set; }
        public DbSet<GIO_HANG> db_GIO_HANG { get; set; }
        public DbSet<CHI_TIET_GIO_HANG> db_CHI_TIET_GIO_HANG { get; set; }
        public DbSet<HOA_DON> db_HOA_DON { get; set; }
        public DbSet<DON_VI_VAN_CHUYEN> db_DON_VI_VAN_CHUYEN { get; set; }
    }
}
