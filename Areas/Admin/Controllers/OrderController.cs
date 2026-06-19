using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.ApplicationUser)
                .ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // BỔ SUNG: CHỨC NĂNG XÓA ĐƠN HÀNG AN TOÀN (GỠ RÀNG BUỘC KHÓA NGOẠI TỰ ĐỘNG)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Nạp đơn hàng đi kèm danh sách chi tiết hóa đơn của nó
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Bước 1: Xóa toàn bộ dữ liệu trong bảng con (OrderDetails) trước để gỡ liên kết
            if (order.OrderDetails != null && order.OrderDetails.Any())
            {
                _context.OrderDetails.RemoveRange(order.OrderDetails);
            }

            // Bước 2: Tiến hành xóa thực thể đơn hàng chính (Orders)
            _context.Orders.Remove(order);

            // Bước 3: Đồng bộ và lưu thay đổi xuống SQL Server
            await _context.SaveChangesAsync();

            // Chuyển hướng quay trở lại trang danh sách sau khi xóa thành công
            return RedirectToAction(nameof(Index));
        }
    }
}