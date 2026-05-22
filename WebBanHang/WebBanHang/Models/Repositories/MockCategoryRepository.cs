using System.Collections.Generic;

namespace WebBanHang.Models.Repositories
{
    // Đã thêm ": ICategoryRepository" để thực hiện kế thừa Interface hoàn chỉnh
    public class MockCategoryRepository : ICategoryRepository
    {
        private List<Category> _categoryList;

        public MockCategoryRepository()
        {
            _categoryList = new List<Category>
            {
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Desktop" },
                new Category { Id = 3, Name = "Smartphone" },
                new Category { Id = 4, Name = "Test" }// Thêm một danh mục mẫu cho phong phú
            };
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList;
        }
    }
}