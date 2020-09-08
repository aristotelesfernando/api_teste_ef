using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using teste_ef.Models;

namespace Controllers
{
    [ApiController]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext ct)
        {
            var products = await ct.Products.Include(p => p.Category).ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext ct, int id)
        {
            var product = await ct.Products.Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        [HttpGet]
        [Route("{categories/id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext ct, int id)
        {
            var products = await ct.Products
                .Include(c => c.Category)
                .AsNoTracking()
                .Where(c => c.CategoryId == id)
                .ToListAsync();
            return products;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext ct, [FromBody] Product model)
        {
            if (ModelState.IsValid)
            {
                ct.Products.Add(model);
                await ct.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}