// Controller for managing customer-related API endpoints
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;

namespace OrderManagementSystem.Controllers
{
    /// <summary>
    /// Controller for managing customers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepo; // Repository for customer data access
        private readonly ILogger<CustomerController> _logger; // Logger for this controller
        private readonly IMapper _mapper; // AutoMapper for DTO/entity mapping

        public CustomerController(
            ICustomerRepository customerRepo,
            ILogger<CustomerController> logger,
            IMapper mapper)
        {
            _customerRepo = customerRepo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="request">The customer creation request</param>
        /// <returns>The created customer</returns>
        /// <response code="201">Returns the newly created customer</response>
        /// <response code="400">If the request data is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CustomerResponse>> Create([FromBody] CreateCustomerRequest request)
        {
            // Validate the request model
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for customer creation");
                return BadRequest(ModelState);
            }
            // Create and save a new customer
            _logger.LogInformation("Creating new customer: {CustomerName}", request.Name);
            var customer = _mapper.Map<Customer>(request);
            await _customerRepo.AddAsync(customer);
            var customerResponse = _mapper.Map<CustomerResponse>(customer);
            return CreatedAtAction(nameof(GetById), new { customerId = customer.CustomerId }, customerResponse);
        }

        /// <summary>
        /// Get a customer by ID
        /// </summary>
        /// <param name="customerId">The ID of the customer</param>
        /// <returns>The customer details</returns>
        /// <response code="200">Returns the requested customer</response>
        /// <response code="404">If the customer is not found</response>
        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(CustomerResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CustomerResponse>> GetById(int customerId)
        {
            // Retrieve a customer by their ID
            _logger.LogInformation("Getting customer with ID: {CustomerId}", customerId);
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found", customerId);
                return NotFound();
            }
            var customerResponse = _mapper.Map<CustomerResponse>(customer);
            return Ok(customerResponse);
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>List of all customers</returns>
        /// <response code="200">Returns the list of customers</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerResponse>), 200)]
        public async Task<ActionResult<List<CustomerResponse>>> GetAll()
        {
            // Retrieve all customers from the repository
            _logger.LogInformation("Getting all customers");
            var customers = await _customerRepo.GetAllAsync();
            var customerResponses = _mapper.Map<List<CustomerResponse>>(customers);
            return Ok(customerResponses);
        }

        /// <summary>
        /// Get orders for a specific customer
        /// </summary>
        /// <param name="customerId">The ID of the customer</param>
        /// <returns>List of customer orders</returns>
        /// <response code="200">Returns the customer's orders</response>
        /// <response code="404">If the customer is not found</response>
        [HttpGet("{customerId}/orders")]
        [ProducesResponseType(typeof(List<OrderResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<OrderResponse>>> GetOrders(int customerId)
        {
            // Retrieve all orders for a specific customer
            _logger.LogInformation("Getting orders for customer ID: {CustomerId}", customerId);
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found", customerId);
                return NotFound();
            }
            var orders = await _customerRepo.GetCustomerOrdersAsync(customerId);
            var orderResponses = _mapper.Map<List<OrderResponse>>(orders);
            return Ok(orderResponses);
        }

        /// <summary>
        /// Update an existing customer
        /// </summary>
        /// <param name="customerId">The ID of the customer to update</param>
        /// <param name="request">The customer update request</param>
        /// <returns>No content on success</returns>
        /// <response code="204">If the customer was successfully updated</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="404">If the customer is not found</response>
        [HttpPut("{customerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int customerId, [FromBody] UpdateCustomerRequest request)
        {
            // Validate the request model
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for customer update");
                return BadRequest(ModelState);
            }
            // Update an existing customer
            _logger.LogInformation("Updating customer with ID: {CustomerId}", customerId);
            var existing = await _customerRepo.GetByIdAsync(customerId);
            if (existing == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found for update", customerId);
                return NotFound();
            }
            _mapper.Map(request, existing);
            await _customerRepo.UpdateAsync(existing);
            return NoContent();
        }
    }
}
