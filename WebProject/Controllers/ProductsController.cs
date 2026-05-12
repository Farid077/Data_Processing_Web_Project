using Microsoft.AspNetCore.Mvc;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // In-memory store — replace with your DbContext
    private static List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Widget A", Category = "Electronics", Price = 29.99m, Stock = 100 },
        new Product { Id = 2, Name = "Gadget B", Category = "Electronics", Price = 49.99m, Stock = 50  },
        new Product { Id = 3, Name = "Tool C",   Category = "Hardware",     Price = 14.99m, Stock = 200 },
    };
    private static int _nextId = 4;

    [HttpGet]
    public IActionResult GetAll() => Ok(_products);

    [HttpPost]
    public IActionResult Create([FromBody] Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name)) return BadRequest("Name is required.");
        product.Id = _nextId++;
        _products.Add(product);
        return Ok(product);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Product updated)
    {
        var item = _products.FirstOrDefault(p => p.Id == id);
        if (item == null) return NotFound();
        item.Name = updated.Name;
        item.Category = updated.Category;
        item.Price = updated.Price;
        item.Stock = updated.Stock;
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _products.FirstOrDefault(p => p.Id == id);
        if (item == null) return NotFound();
        _products.Remove(item);
        return Ok();
    }
}