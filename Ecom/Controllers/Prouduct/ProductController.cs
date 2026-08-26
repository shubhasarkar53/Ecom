using Ecom.DTOs.Product;
using Ecom.Services;
using Ecom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Controllers.Prouduct
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductItemService _productItemService;
        public ProductController(IProductService productService, IProductItemService productItemService)
        {
            _productService = productService;
            _productItemService = productItemService;
        }

        // GET: api/Product/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDTO>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            return Ok(product);
        }

        // POST: api/product/create
        [HttpPost("create")]
        public async Task<ActionResult<ProductResponseDTO>> Create([FromBody] ProductRequestDTO productRequest)
        {
            var createdProduct = await _productService.CreateAsync(productRequest);

            return CreatedAtAction(nameof(GetById),new { id = createdProduct.Id },createdProduct);
        }

        // PUT: api/product/update/5
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] ProductRequestDTO productRequest)
        {
            var updated = await _productService.UpdateAsync(productRequest, id);

            return NoContent();
        }

        // DELETE: api/product/delete/5
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteAsync(id);

            return NoContent();
        }


        // PUT: api/product/15/quantity/increase
        [HttpPut("{id:int}/quantity/increase")]
        public async Task<ActionResult<int>> IncreaseQuantity(int id)
        {
            var updatedQuantity = await _productItemService.IncreaseQuantityAsync(id);

            return Ok(updatedQuantity);
        }

        // PUT: api/product/15/quantity/decrease
        [HttpPut("{id:int}/quantity/decrease")]
        public async Task<ActionResult<int>> DecreaseQuantity(int id)
        {
            var updatedQuantity = await _productItemService.DecreaseQuantityAsync(id);

            return Ok(updatedQuantity);
        }

        // PUT: api/product/15/quantity?quantity=50
        [HttpPut("{id:int}/quantity")]
        public async Task<ActionResult<int>> SetQuantity(int id,[FromQuery] int quantity)
        {
            var updatedQuantity = await _productItemService.SetQuantityAsync(id, quantity);

            return Ok(updatedQuantity);
        }
    }
}
