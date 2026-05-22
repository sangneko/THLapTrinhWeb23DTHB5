using System.Collections.Generic;
using System.Linq;

namespace WebBanHang.Models.Repositories
{
    // Đã thêm ": IProductRepository" để thực hiện kế thừa Interface
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public MockProductRepository()
        {
            // Tạo một số dữ liệu mẫu
            _products = new List<Product>
            {
                 // Thêm mẫu 1 sản phẩm cho danh sách đỡ trống
            };
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            // Bảo vệ code: Nếu danh sách trống thì bắt đầu từ Id = 1, ngược lại tăng thêm 1
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }

        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}