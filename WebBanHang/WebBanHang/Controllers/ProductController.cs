using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
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

    public IActionResult Index()
    {
        var products = _productRepository.GetAll();
        return View(products);
    }

    public IActionResult Display(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    public IActionResult Add()
    {
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
    {
        if (ModelState.IsValid)
        {
            if (imageUrl != null && imageUrl.Length > 0)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }

            if (imageUrls != null)
            {
                product.ImageUrls = new List<string>();
                foreach (var file in imageUrls)
                {
                    if (file.Length > 0)
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }
            }

            _productRepository.Add(product);
            return RedirectToAction("Index");
        }

        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View(product);
    }

    public IActionResult Update(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
    {
        if (ModelState.IsValid)
        {
            if (imageUrl != null && imageUrl.Length > 0)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }

            if (imageUrls != null)
            {
                product.ImageUrls = new List<string>();
                foreach (var file in imageUrls)
                {
                    if (file.Length > 0)
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }
            }

            _productRepository.Update(product);
            return RedirectToAction("Index");
        }

        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    public IActionResult Delete(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost, ActionName("DeleteConfirmed")]
    public IActionResult DeleteConfirmed(int id)
    {
        _productRepository.Delete(id);
        return RedirectToAction("Index");
    }

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