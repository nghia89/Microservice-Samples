using Domain.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;

namespace Product.Api.Repository;

public class ProductRepository : RepositoryBase<CatalogProduct, long, ProductContext>, IProductRepository
{
    public ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork) : base(dbContext, unitOfWork)
    {
    }

    public async Task CreateProduct(CatalogProduct product) => await CreateAsync(product);

    public async Task DeleteProduct(long id)
    {
        var product = await GetProduct(id);
        if (product != null) DeleteAsync(product);
    }

    public async Task<CatalogProduct> GetProduct(long id) => await GetByIdAsync(id);

    public Task<CatalogProduct> GetProductByNo(string productNo)
     => FindByCondition(x => x.No.Equals(productNo)).SingleOrDefaultAsync();


    public async Task<IEnumerable<CatalogProduct>> GetProducts() => await FindAll().ToListAsync();

    public Task UpdateProduct(CatalogProduct product) => UpdateAsync(product);
}