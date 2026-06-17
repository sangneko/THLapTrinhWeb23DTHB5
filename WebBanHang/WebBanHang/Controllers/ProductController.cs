using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.Models.Repositories;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    // Hiển thị danh sách sản phẩm
    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllAsync();
        return View(products);
    }

    // Hiển thị thông tin chi tiết sản phẩm
    public async Task<IActionResult> Display(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // Hiển thị form thêm sản phẩm mới
    public async Task<IActionResult> Add()
    {
        var categories = await _categoryRepository.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    // Xử lý thêm sản phẩm mới
    [HttpPost]
    public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
    {
        if (ModelState.IsValid)
        {
            if (imageUrl != null && imageUrl.Length > 0)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }

            await _productRepository.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        var categories = await _categoryRepository.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId); // Thêm thông tin ID được chọn
        return View(product);
    }

    // Hiển thị form cập nhật sản phẩm
    public async Task<IActionResult> Update(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var categories = await _categoryRepository.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    // Xử lý cập nhật sản phẩm
    [HttpPost]
    public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
    {
        ModelState.Remove("imageUrl"); // Chữ 'i' thường khớp với tham số nhận từ form [cite: 395, 397]

        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Xử lý hình ảnh thông minh
            if (imageUrl != null && imageUrl.Length > 0)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }
            else
            {
                product.ImageUrl = existingProduct.ImageUrl; // Giữ lại ảnh cũ nếu không up ảnh mới
            }

            // Đồng bộ dữ liệu
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.ImageUrl = product.ImageUrl;

            await _productRepository.UpdateAsync(existingProduct);
            return RedirectToAction(nameof(Index));
        }

        // Fix lỗi nạp lại danh mục nếu form lỗi dữ liệu đầu vào
        var categories = await _categoryRepository.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    // Hiển thị form xác nhận xóa sản phẩm
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // Xử lý xóa sản phẩm
    [HttpPost, ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // Hàm lưu hình ảnh
    private async Task<string> SaveImage(IFormFile image)
    {
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

        using (var fileStream = new FileStream(savePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        return "/images/" + image.FileName;
    }
}