using Lab06.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab06.Repositories
{
    // Bổ sung ": IProductRepository" để thực hiện kế thừa giao diện (interface)
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        // Hàm khởi tạo (Constructor) để tiêm DbContext vào sử dụng
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách tất cả sản phẩm
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // 2. Lấy thông tin chi tiết một sản phẩm theo ID
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // 3. Thêm một sản phẩm mới
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // 4. Cập nhật thông tin sản phẩm
        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // 5. Xóa một sản phẩm dựa trên ID
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}