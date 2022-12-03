using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Product.Api.Repository;
using Product.API.Entities;
using Shared.DTOs.Product;
namespace Product.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepo;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;
    private readonly IElasticClient _elasticClient;
    public ProductsController(ILogger<ProductsController> logger,
     IProductRepository productRepo,
     IMapper mapper,
     IElasticClient elasticClient)
    {
        _logger = logger;
        _productRepo = productRepo;
        _mapper = mapper;
        _elasticClient = elasticClient;
    }

    [Route("/search")]
    [HttpGet]
    public async Task<IActionResult> Find(string? query, int page = 1, int pageSize = 5)
    {
        var response = await _elasticClient.SearchAsync<CreateProductDto>(
            s => s.Query(q => q.QueryString(d => d.Query(query)))
                .From((page - 1) * pageSize)
                .Size(pageSize));

        if (!response.IsValid)
        {

            _logger.LogError("Failed to search documents");
            return Ok();
        }

        return Ok(response.Documents);
    }
    #region CRUD
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productRepo.GetProducts();
        var result = _mapper.Map<IEnumerable<ProductDto>>(products);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetProduct([Required] long id)
    {
        var product = await _productRepo.GetProduct(id);
        if (product == null)
            return NotFound();

        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
    {
        var productEntity = await _productRepo.GetProductByNo(productDto.No);
        if (productEntity != null) return BadRequest($"Product No: {productDto.No} is existed.");

        var product = _mapper.Map<CatalogProduct>(productDto);
        await _productRepo.CreateProduct(product);
        await _productRepo.SaveChangesAsync();
        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] UpdateProductDto productDto)
    {
        var product = await _productRepo.GetProduct(id);
        if (product == null)
            return NotFound();

        var updateProduct = _mapper.Map(productDto, product);
        await _productRepo.UpdateProduct(updateProduct);
        await _productRepo.SaveChangesAsync();
        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteProduct([Required] long id)
    {
        var product = await _productRepo.GetProduct(id);
        if (product == null)
            return NotFound();

        await _productRepo.DeleteProduct(id);
        await _productRepo.SaveChangesAsync();
        return NoContent();
    }

    #endregion

    #region Additional Resources

    [HttpGet("get-product-by-no/{productNo}")]
    public async Task<IActionResult> GetProductByNo([Required] string productNo)
    {
        var product = await _productRepo.GetProductByNo(productNo);
        if (product == null)
            return NotFound();

        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    #endregion


}

