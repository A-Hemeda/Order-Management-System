// Controller for managing product-related API endpoints
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;

namespace OrderManagementSystem.Controllers
{
    /// <summary>
    /// Controller for managing products
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo; // Repository for product data access
        private readonly ILogger<ProductController> _logger; // Logger for this controller
        private readonly IMapper _mapper; // AutoMapper for DTO/entity mapping

        public ProductController(
            IProductRepository productRepo, 
            ILogger<ProductController> logger,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of all products</returns>
        /// <response code="200">Returns the list of products</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProductResponse>), 200)]
        public async Task<ActionResult<List<ProductResponse>>> GetAll()
        {
            // Retrieve all products from the repository
            _logger.LogInformation("Getting all products");
            var products = await _productRepo.GetAllAsync();
            var productResponses = _mapper.Map<List<ProductResponse>>(products);
            return Ok(productResponses);
        }

        /// <summary>
        /// Get a product by ID
        /// </summary>
        /// <param name="productId">The ID of the product</param>
        /// <returns>The product details</returns>
        /// <response code="200">Returns the requested product</response>
        /// <response code="404">If the product is not found</response>
        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(ProductResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductResponse>> GetById(int productId)
        {
            // Retrieve a product by its ID
            _logger.LogInformation("Getting product with ID: {ProductId}", productId);
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", productId);
                return NotFound();
            }
            var productResponse = _mapper.Map<ProductResponse>(product);
            return Ok(productResponse);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="request">The product creation request</param>
        /// <returns>The created product</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request)
        {
            // Validate the request model
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for product creation");
                return BadRequest(ModelState);
            }
            // Create and save a new product
            _logger.LogInformation("Creating new product: {ProductName}", request.Name);
            var product = _mapper.Map<Product>(request);
            await _productRepo.AddAsync(product);
            var productResponse = _mapper.Map<ProductResponse>(product);
            return CreatedAtAction(nameof(GetById), new { productId = product.ProductId }, productResponse);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="productId">The ID of the product to update</param>
        /// <param name="request">The product update request</param>
        /// <returns>No content on success</returns>
        /// <response code="204">If the product was successfully updated</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If the product is not found</response>
        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int productId, [FromBody] UpdateProductRequest request)
        {
            // Validate the request model
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for product update");
                return BadRequest(ModelState);
            }
            // Update an existing product
            _logger.LogInformation("Updating product with ID: {ProductId}", productId);
            var existing = await _productRepo.GetByIdAsync(productId);
            if (existing == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for update", productId);
                return NotFound();
            }
            _mapper.Map(request, existing);
            await _productRepo.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="productId">The ID of the product to delete</param>
        /// <returns>No content on success</returns>
        /// <response code="204">If the product was successfully deleted</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If the product is not found</response>
        [HttpDelete("{productId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int productId)
        {
            // Delete a product by its ID
            _logger.LogInformation("Deleting product with ID: {ProductId}", productId);
            var existing = await _productRepo.GetByIdAsync(productId);
            if (existing == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for deletion", productId);
                return NotFound();
            }
            await _productRepo.DeleteAsync(productId);
            return NoContent();
        }
    }
}
