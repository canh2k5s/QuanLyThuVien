using Microsoft.AspNetCore.Mvc;

namespace QuanLyThuVien.Controllers
{
    public class AdminController : Controller
    {
        // Kiểm tra quyền Admin
        private bool KiemTraAdmin()
        {
            return HttpContext.Session.GetInt32("MaNguoiDung") != null
                && HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        // Tổng quan
        public IActionResult Index()
        {
            if (!KiemTraAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Phiếu mượn
        public IActionResult PhieuMuon()
        {
            if (!KiemTraAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Phiếu quá hạn
        public IActionResult QuaHan()
        {
            if (!KiemTraAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Quản lý sách
        public IActionResult Sach()
        {
            if (!KiemTraAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Quản lý người dùng
        public IActionResult NguoiDung()
        {
            if (!KiemTraAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}
