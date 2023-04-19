using Microsoft.EntityFrameworkCore;
using ShoppingApp.Authentication;
using ShoppingApp.Dto;
using ShoppingApp.Models;

namespace ShoppingApp.Repos
{
    public interface IProductRepository
    {
        Task<Product> GetProductById(int id);
            Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<Product> Create(ProductDto product);
        Task UpdateProduct(int id, ProductDto prod);
        Task DeleteAsync(int id);

        Task<IEnumerable<Product>> Search(string ProductName);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> GetProductById(int id)
        {
            //return await _dbContext.Set<Product>().FindAsync(id);
            return await _dbContext.products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbContext.Set<Product>().ToListAsync();
        }

        public async Task<Product> Create(ProductDto productDto)
        {
            var category = await _dbContext.categories.FindAsync(productDto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("Invalid Category");
            }

            var product = new Product
            {
                ProductName = productDto.ProductName,
                //ProductCategory = productDto.ProductCategory,
                ProductPrice = productDto.ProductPrice,
                ProductQuantity = productDto.ProductQuantity,
                ProductDescription = productDto.ProductDescription,
                Category = category
            };

            await _dbContext.products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }


        /*public async Task UpdateAsync(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }*/

        public async Task UpdateProduct(int id, ProductDto prod)
        {
            var prods = await _dbContext.products.FindAsync(id);

            if (prods == null)
            {
                throw new ArgumentException("Product not found");
            }
            prods.ProductName = prod.ProductName;
        //prods.ProductCategory = prod.ProductCategory;
       prods.ProductPrice = prod.ProductPrice;
      prods.ProductQuantity = prod.ProductQuantity;
       prods.ProductDescription = prod.ProductDescription;
     //       prods.Category = prod.category;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetProductById(id);
            _dbContext.Set<Product>().Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        Task<IEnumerable<ProductDto>> IProductRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        /*public Task CreateAsync(ProductDto product)
        {
            throw new NotImplementedException();
        }*/

        public Task UpdateAsync(ProductDto product)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Product>> Search(string ProductName)
        {
            IQueryable<Product> query = _dbContext.products;
            if (!string.IsNullOrEmpty(ProductName))
            {
                query = query.Where(a => a.ProductName.Contains(ProductName));
            }
            return await query.ToListAsync();
        }
    }

}
