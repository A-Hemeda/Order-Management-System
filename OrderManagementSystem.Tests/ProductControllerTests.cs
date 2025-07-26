using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrderManagementSystem.Controllers;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;
using Xunit;
using FluentAssertions;

namespace OrderManagementSystem.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<ILogger<ProductController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductController>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ProductController(_mockProductRepo.Object, _mockLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Product 1", Price = 10.99m, Stock = 100 },
                new Product { ProductId = 2, Name = "Product 2", Price = 20.99m, Stock = 50 }
            };

            var productResponses = new List<ProductResponse>
            {
                new ProductResponse { ProductId = 1, Name = "Product 1", Price = 10.99m, Stock = 100 },
                new ProductResponse { ProductId = 2, Name = "Product 2", Price = 20.99m, Stock = 50 }
            };

            _mockProductRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);
            _mockMapper.Setup(mapper => mapper.Map<List<ProductResponse>>(products)).Returns(productResponses);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProducts = okResult.Value.Should().BeOfType<List<ProductResponse>>().Subject;
            returnedProducts.Should().HaveCount(2);
            returnedProducts[0].Name.Should().Be("Product 1");
            returnedProducts[1].Name.Should().Be("Product 2");
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "Test Product", Price = 15.99m, Stock = 25 };
            var productResponse = new ProductResponse { ProductId = 1, Name = "Test Product", Price = 15.99m, Stock = 25 };

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductResponse>(product)).Returns(productResponse);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProduct = okResult.Value.Should().BeOfType<ProductResponse>().Subject;
            returnedProduct.ProductId.Should().Be(1);
            returnedProduct.Name.Should().Be("Test Product");
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_WithValidRequest_ShouldReturnCreatedProduct()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "New Product",
                Price = 25.99m,
                Stock = 75
            };

            var product = new Product
            {
                ProductId = 1,
                Name = "New Product",
                Price = 25.99m,
                Stock = 75
            };

            var productResponse = new ProductResponse
            {
                ProductId = 1,
                Name = "New Product",
                Price = 25.99m,
                Stock = 75
            };

            _mockMapper.Setup(mapper => mapper.Map<Product>(request)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductResponse>(product)).Returns(productResponse);
            _mockProductRepo.Setup(repo => repo.AddAsync(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var returnedProduct = createdResult.Value.Should().BeOfType<ProductResponse>().Subject;
            returnedProduct.Name.Should().Be("New Product");
            returnedProduct.Price.Should().Be(25.99m);
        }

        [Fact]
        public async Task Create_WithInvalidModelState_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "", // Invalid - empty name
                Price = -1, // Invalid - negative price
                Stock = -5 // Invalid - negative stock
            };

            _controller.ModelState.AddModelError("Name", "Name is required");
            _controller.ModelState.AddModelError("Price", "Price must be greater than 0");

            // Act
            var result = await _controller.Create(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
} 