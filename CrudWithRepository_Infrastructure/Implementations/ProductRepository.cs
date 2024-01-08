using CrudWithRepository_Core;
using CrudWithRepository_Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudWithRepository_Infrastructure.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDBContext _dbContext;

        public ProductRepository(AppDBContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var product = await _dbContext.products.ToListAsync();
            return product;
            
        }

        public async Task<Product> GetById(int id)
        {
            return await _dbContext.products.FindAsync(id);
        }
        public async Task Add(Product product)
        {
            await _dbContext.products.AddAsync(product);
            await Save();
        }

        

        public async Task Update(Product product)
        {
            var Product = await _dbContext.products.FindAsync(product.Id);
            if(product != null)
            {
                Product.ProductName = product.ProductName;
                Product.Price = product.Price;
                Product.Qty = product.Qty;
                _dbContext.products.Update(Product);
                await Save();
            }

        }
        public async Task DeleteById(int id)
        {
            var product = await _dbContext.products.FindAsync(id);
            if(product != null)
            {
                 _dbContext.products.Remove(product);
                await Save();
            }
        }
        private async Task Save()
        {
            try
            {
               await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }



    }
}
