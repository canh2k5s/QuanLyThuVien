using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Xử lý đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string HoTen,
            string TenDangNhap,
            string Email,
            string SoDienThoai,
            string MatKhau,
            string XacNhanMatKhau)
        {
            if (string.IsNullOrWhiteSpace(HoTen) ||
                string.IsNullOrWhiteSpace(TenDangNhap) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(MatKhau))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            if (MatKhau != XacNhanMatKhau)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp.";
                return View();
            }

            bool daTonTai = await _context.NguoiDung.AnyAsync(
                x => x.TenDangNhap == TenDangNhap || x.Email == Email
            );

            if (daTonTai)
            {
                ViewBag.Error = "Tên đăng nhập hoặc email đã được sử dụng.";
                return View();
            }

            var vaiTroDocGia = await _context.VaiTro
                .FirstOrDefaultAsync(x => x.TenVaiTro == "DocGia");

            if (vaiTroDocGia == null)
            {
                ViewBag.Error = "Không tìm thấy vai trò độc giả.";
                return View();
            }

            var nguoiDung = new NguoiDung
            {
                HoTen = HoTen,
                TenDangNhap = TenDangNhap,
                Email = Email,
                SoDienThoai = SoDienThoai,
                MatKhau = MatKhau,
                MaVaiTro = vaiTroDocGia.MaVaiTro,
                TrangThai = true
            };

            _context.NguoiDung.Add(nguoiDung);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công. Anh hãy đăng nhập.";

            return RedirectToAction("Login");
        }

        // Trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string TaiKhoan,
            string MatKhau)
        {
            if (string.IsNullOrWhiteSpace(TaiKhoan) ||
                string.IsNullOrWhiteSpace(MatKhau))
            {
                ViewBag.Error = "Vui lòng nhập tài khoản và mật khẩu.";
                return View();
            }

            // Tìm tài khoản bằng tên đăng nhập hoặc email
            var nguoiDung = await _context.NguoiDung
                .FirstOrDefaultAsync(x =>
                    (x.TenDangNhap == TaiKhoan ||
                     x.Email == TaiKhoan)
                    && x.MatKhau == MatKhau
                    && x.TrangThai == true
                );

            if (nguoiDung == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác.";
                return View();
            }

            // Lấy vai trò của tài khoản
            var vaiTro = await _context.VaiTro
                .FirstOrDefaultAsync(x =>
                    x.MaVaiTro == nguoiDung.MaVaiTro
                );

            if (vaiTro == null)
            {
                ViewBag.Error = "Tài khoản chưa được phân quyền.";
                return View();
            }

            // Lưu thông tin đăng nhập vào Session
            HttpContext.Session.SetInt32(
                "MaNguoiDung",
                nguoiDung.MaNguoiDung
            );

            HttpContext.Session.SetString(
                "HoTen",
                nguoiDung.HoTen
            );

            HttpContext.Session.SetString(
                "VaiTro",
                vaiTro.TenVaiTro
            );

            // Phân quyền chuyển trang
            if (vaiTro.TenVaiTro == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            if (vaiTro.TenVaiTro == "DocGia")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Vai trò tài khoản không hợp lệ.";
            return View();
        }

        // Đăng xuất
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
