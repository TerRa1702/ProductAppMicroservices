using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using SharedLibrary.Interfaces;
using SharedLibrary.Responses;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController(IBaseMapper<Product, ProductDTO> mapTo,
        IBaseMapper<ProductDTO, Product> mapFrom, IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts(CancellationToken cancellationToken)
        {
            var entities = await productInterface.GetAllAsync(cancellationToken);

            if (!entities.Any())
                return NotFound("No products detected in database");

            var products = mapTo.MapList(entities);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id, CancellationToken cancellationToken)
        {
            var entity = await productInterface.FindByIdAsync(id, cancellationToken);

            if (entity is null)
                return NotFound("Requested product not found");

            var product = mapTo.MapDTO(entity);
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = mapFrom.MapDTO(product);
            var response = await productInterface.CreateAsync(entity, cancellationToken);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = mapFrom.MapDTO(product);
            var response = await productInterface.UpdateAsync(entity, cancellationToken);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product, CancellationToken cancellationToken)
        {
            var entity = await productInterface.FindByIdAsync(product.Id, cancellationToken);
            var response = await productInterface.DeleteAsync(entity, cancellationToken);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
