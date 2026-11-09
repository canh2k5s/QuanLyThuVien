using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string? SoDienThoai { get; set; }

        public int MaVaiTro { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}