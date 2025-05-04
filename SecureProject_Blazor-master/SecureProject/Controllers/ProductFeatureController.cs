using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureProject.Interface.Service;
using SecureProject.Shared;
using SecureProject.Shared.DTO;

namespace SecureProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ProductFeatureController : ControllerBase
    {
        private readonly IProductFeatureService _service;
        public ProductFeatureController(IProductFeatureService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetProductFeatures() =>
            Ok(await _service.GetProductFeaturesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductFeature(int id)
        {
            var product = await _service.GetProductFeatureByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductFeature([FromBody] ProductFeatureAddEditDTO product)
        {
            await _service.AddProductFeatureAsync(product);
            return CreatedAtAction(nameof(GetProductFeature), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductFeature(int id, [FromBody] ProductFeatureAddEditDTO productFeature)
        {
            if (id != productFeature.Id) return BadRequest();
            var updated = await _service.UpdateProductFeatureAsync(productFeature);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductFeature(int id)
        {
            var deleted = await _service.DeleteProductFeatureAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
