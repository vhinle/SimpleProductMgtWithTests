using NUnit.Framework;
using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ProductMgt.Tests
{
    [TestFixture]
    public class Tests : IDisposable
    {

        private AppDbContext _context;
        private DBService _dbService;


        [SetUp]
        public void Setup()
        {

            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>(options =>
                    options.UseMySql("Server=localhost;Database=productsdb;User ID=root;Password=;",
                        new MySqlServerVersion(new Version(8, 0, 21))))
                .BuildServiceProvider();

            _context = serviceProvider.GetRequiredService<AppDbContext>();
            _dbService = new DBService(_context);

            // Ensure the test database is clean and ready for each test run
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed test data
            _context.Products.Add(new Product { Name = "Product1", Price = 10.0m, Stocks = 100 });
            _context.Products.Add(new Product { Name = "Product2", Price = 20.0m, Stocks = 50 });
            _context.SaveChanges();

            //var serviceProvider = new ServiceCollection()
            //   .AddDbContext<AppDbContext>(options =>
            //       options.UseInMemoryDatabase(databaseName: "TestDatabase"))
            //   .BuildServiceProvider();

            //_context = serviceProvider.GetRequiredService<AppDbContext>();
            //_dbService = new DBService(_context);

            //// Seed test data
            //_context.Products.Add(new Product { Name = "Product1", Price = 10.0m, Stocks = 100 });
            //_context.Products.Add(new Product { Name = "Product2", Price = 20.0m, Stocks = 50 });
            //_context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void AddProduct_Should_AddNewProduct()
        {
            // Arrange
            var newProduct = new Product { Name = "New Product", Price = 15.0m, Stocks = 75 };

            // Act
            _dbService.AddProduct(newProduct);

            // Assert
            var savedProduct = _context.Products.Find(newProduct.Id);

            Debug.WriteLine(newProduct.Id);

            Assert.That(savedProduct, Is.Not.Null);
            Assert.That(newProduct.Name, Is.EqualTo( savedProduct.Name));
            Assert.That(newProduct.Price, Is.EqualTo(savedProduct.Price));
            Assert.That(newProduct.Stocks, Is.EqualTo(savedProduct.Stocks));

            //old methods
            //Assert.IsNotNull(savedProduct);
            //Assert.AreEqual(newProduct.Name, savedProduct.Name);
            //Assert.AreEqual(newProduct.Price, savedProduct.Price);
            //Assert.AreEqual(newProduct.Price, savedProduct.Stocks);
        }

        [Test]
        public void UpdateProduct_Should_UpdateExistingProduct()
        {
            // Arrange
            var productToUpdate = _context.Products.First();
            productToUpdate.Price = 25.0m;
            productToUpdate.Stocks = 120;

            // Act
            _dbService.UpdateProduct(productToUpdate);

            // Assert
            var updatedProduct = _context.Products.Find(productToUpdate.Id);
            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(productToUpdate.Name, updatedProduct.Name);
            Assert.AreEqual(productToUpdate.Price, updatedProduct.Price);
            Assert.AreEqual(productToUpdate.Stocks, updatedProduct.Stocks);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}