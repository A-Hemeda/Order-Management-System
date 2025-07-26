// Controller for managing invoice-related API endpoints
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IRepository<Invoice> _invoiceRepo; // Repository for invoice data access

    public InvoiceController(IRepository<Invoice> invoiceRepo)
    {
        _invoiceRepo = invoiceRepo;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        // Retrieve all invoices (admin only)
        var invoices = await _invoiceRepo.GetAllAsync();
        return Ok(invoices);
    }

    [HttpGet("{invoiceId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int invoiceId)
    {
        // Retrieve an invoice by its ID (admin only)
        var invoice = await _invoiceRepo.GetByIdAsync(invoiceId);
        if (invoice == null) return NotFound();
        return Ok(invoice);
    }
}
