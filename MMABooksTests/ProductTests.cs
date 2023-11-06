using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        MMABooksContext dbContext;
        Product? p;
        List<Product>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Products.Find("A4CS");
            Assert.IsNotNull(p);
            Assert.AreEqual("A4CS", p.ProductCode);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50
            products = dbContext.Products.Where(p => p.UnitPrice.Equals((decimal)56.50)).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);

        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        [Test]
        public void DeleteTest()
        {
            p = dbContext.Products.Find("A4CS");
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("A4CS"));
        }

        [Test]
        public void CreateTest()
        {
            Product product = new Product();
            product.ProductCode = "ABCD";
            product.Description = "Test Product";
            product.UnitPrice = 100;
            product.OnHandQuantity = 100;
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            Product retrieved = dbContext.Products.Find("ABCD");
            Assert.IsNotNull(retrieved);
			Assert.AreEqual("ABCD", retrieved.ProductCode);
        }

        [Test]
        public void UpdateTest()
        {
            Product product = new Product();
			product.ProductCode = "ABCD";
			product.Description = "Test Product";
			product.UnitPrice = 100;
			product.OnHandQuantity = 100;
			dbContext.Products.Add(product);
			dbContext.SaveChanges();
            Assert.AreEqual("Test Product", dbContext.Products.Find("ABCD").Description);
			product.ProductCode = "ABCD";
			product.Description = "Another Test Product";
			product.UnitPrice = 99;
			product.OnHandQuantity = 99;
            dbContext.SaveChanges();
            Assert.AreEqual("Another Test Product", dbContext.Products.Find("ABCD").Description);
            Console.WriteLine(dbContext.Products.Find("ABCD").ToString());
		}

        public void PrintAll(List<Product> products)
        {
            foreach(Product p in products)
            {
                Console.WriteLine(p);
            }
        }
    }
}