using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DBService
    {
        private readonly AppDbContext _context;

        public DBService(AppDbContext context)
        {
            _context = context;
        }

        // Method to retrieve all products
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        // Method to retrieve a product by id
        public Product GetProductById(int id)
        {
            return _context.Products.Find(id);
        }

        // Method to add a new product
        public void AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Debug.WriteLine($"Error adding product: {ex.Message}");
                throw; // Optionally rethrow or handle the exception
            }
        }

        // Method to update an existing product
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        // Method to delete a product by id
        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

    }
}
