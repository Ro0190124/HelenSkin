using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelenSkin.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class AddTbToData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "db_DANH_MUC",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_DANH_MUC", x => x.MaDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "db_DON_VI_VAN_CHUYEN",
                columns: table => new
                {
                    MaDonViVanChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDonViVanChuyen = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_DON_VI_VAN_CHUYEN", x => x.MaDonViVanChuyen);
                });

            migrationBuilder.CreateTable(
                name: "db_NGUOI_DUNG",
                columns: table => new
                {
                    MaND = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenND = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhanQuyen = table.Column<bool>(type: "bit", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_NGUOI_DUNG", x => x.MaND);
                });

            migrationBuilder.CreateTable(
                name: "db_SAN_PHAM",
                columns: table => new
                {
                    MaSP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<double>(type: "float", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_SAN_PHAM", x => x.MaSP);
                    table.ForeignKey(
                        name: "FK_db_SAN_PHAM_db_DANH_MUC_MaDanhMuc",
                        column: x => x.MaDanhMuc,
                        principalTable: "db_DANH_MUC",
                        principalColumn: "MaDanhMuc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_GIO_HANG",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_GIO_HANG", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_db_GIO_HANG_db_NGUOI_DUNG_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "db_NGUOI_DUNG",
                        principalColumn: "MaND",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_DS_MEDIA_HINH_ANH",
                columns: table => new
                {
                    MaDSHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaHinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_DS_MEDIA_HINH_ANH", x => x.MaDSHinhAnh);
                    table.ForeignKey(
                        name: "FK_db_DS_MEDIA_HINH_ANH_db_SAN_PHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "db_SAN_PHAM",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_CHI_TIET_GIO_HANG",
                columns: table => new
                {
                    MaCTGH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSP = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_CHI_TIET_GIO_HANG", x => x.MaCTGH);
                    table.ForeignKey(
                        name: "FK_db_CHI_TIET_GIO_HANG_db_GIO_HANG_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "db_GIO_HANG",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_db_CHI_TIET_GIO_HANG_db_SAN_PHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "db_SAN_PHAM",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_HOA_DON",
                columns: table => new
                {
                    MaHD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    MaGioHang = table.Column<int>(type: "int", nullable: false),
                    MaDonViVanChuyen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_HOA_DON", x => x.MaHD);
                    table.ForeignKey(
                        name: "FK_db_HOA_DON_db_DON_VI_VAN_CHUYEN_MaDonViVanChuyen",
                        column: x => x.MaDonViVanChuyen,
                        principalTable: "db_DON_VI_VAN_CHUYEN",
                        principalColumn: "MaDonViVanChuyen",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_db_HOA_DON_db_GIO_HANG_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "db_GIO_HANG",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_db_CHI_TIET_GIO_HANG_MaGioHang",
                table: "db_CHI_TIET_GIO_HANG",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_db_CHI_TIET_GIO_HANG_MaSP",
                table: "db_CHI_TIET_GIO_HANG",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_db_DS_MEDIA_HINH_ANH_MaSP",
                table: "db_DS_MEDIA_HINH_ANH",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_db_GIO_HANG_MaNguoiDung",
                table: "db_GIO_HANG",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_db_HOA_DON_MaDonViVanChuyen",
                table: "db_HOA_DON",
                column: "MaDonViVanChuyen");

            migrationBuilder.CreateIndex(
                name: "IX_db_HOA_DON_MaGioHang",
                table: "db_HOA_DON",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_db_SAN_PHAM_MaDanhMuc",
                table: "db_SAN_PHAM",
                column: "MaDanhMuc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "db_CHI_TIET_GIO_HANG");

            migrationBuilder.DropTable(
                name: "db_DS_MEDIA_HINH_ANH");

            migrationBuilder.DropTable(
                name: "db_HOA_DON");

            migrationBuilder.DropTable(
                name: "db_SAN_PHAM");

            migrationBuilder.DropTable(
                name: "db_DON_VI_VAN_CHUYEN");

            migrationBuilder.DropTable(
                name: "db_GIO_HANG");

            migrationBuilder.DropTable(
                name: "db_DANH_MUC");

            migrationBuilder.DropTable(
                name: "db_NGUOI_DUNG");
        }
    }
}
