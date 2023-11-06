using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.models;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        MMABooksContext dbContext;
        Customer? c;
        List<Customer>? customers;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
		}

        [Test]
        public void GetAllTest()
        {
            customers = dbContext.Customers.OrderBy(c => c.CustomerId).ToList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Smith, Ahmad", customers[3].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            c = dbContext.Customers.Find(43);
            Assert.IsNotNull(c);
            Assert.AreEqual("Haldorai, Brent", c.Name);
            Console.WriteLine(c);
        }

        [Test]
        public void GetUsingWhere()
        {
			// get a list of all of the customers who live in OR
			customers = dbContext.Customers.Where(c => c.StateCode.Equals("OR")).OrderBy(c => c.Name).ToList();
            Assert.AreEqual(5, customers.Count);
            Assert.AreEqual("Grants Pass", customers[0].City);
            PrintAll(customers);

		}

        [Test]
        public void GetWithInvoicesTest()
        {
            // get the customer whose id is 20 and all of the invoices for that customer
            c = dbContext.Customers.Include("Invoices").Where(c => c.CustomerId == 20).SingleOrDefault();
            Assert.IsNotNull(c);
            Assert.AreEqual("Doraville", c.City);
            Assert.AreEqual(3, c.Invoices.Count);
            Console.WriteLine(c);
        }

        [Test]
        public void GetWithJoinTest()
        {
            // get a list of objects that include the customer id, name, statecode and statename
            var customers = dbContext.Customers.Join(
               dbContext.Customers,
               c => c.CustomerId,
               s => s.CustomerId,
               (c, s) => new { c.CustomerId, c.Name, c.Address, c.City, c.StateCode, c.ZipCode }).OrderBy(r => r.CustomerId).ToList();
            Assert.AreEqual(696, customers.Count);
            // I wouldn't normally print here but this lets you see what each object looks like
            foreach (var c in customers)
            {
                Console.WriteLine(c);
            }
        }

        [Test]
        public void DeleteTest()
        {
            c = dbContext.Customers.Find(235);
            dbContext.Customers.Remove(c);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Customers.Find(235));
        }

        [Test]
        public void CreateTest()
        {
            Customer customer = new Customer();
            customer.CustomerId = 37;
            customer.Name = "Mike Vandeville";
            customer.Address = "1275 Prairie Drive";
            customer.City = "Flatsville";
            customer.StateCode = "OH";
            customer.ZipCode = "99351";
            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();
            Customer retrieved = dbContext.Customers.Find(37);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Mike Vandeville", retrieved.Name);
        }

        [Test]
        public void UpdateTest()
        {
			Customer customer = new Customer();
			customer.CustomerId = 37;
			customer.Name = "Mike Vandeville";
			customer.Address = "1275 Prairie Drive";
			customer.City = "Flatsville";
			customer.StateCode = "OH";
			customer.ZipCode = "99351";
			dbContext.Customers.Add(customer);
            Assert.AreEqual("Mike Vandeville", dbContext.Customers.Find(37).Name);
			dbContext.SaveChanges();
			customer.CustomerId = 37;
			customer.Name = "William Wayland";
			customer.Address = "903 Mill Road";
			customer.City = "Boring";
			customer.StateCode = "OR";
			customer.ZipCode = "99349";
            dbContext.SaveChanges();
            Assert.AreEqual("William Wayland", dbContext.Customers.Find(37).Name);
            Console.WriteLine(dbContext.Customers.Find(37).ToString());
            

		}

        public void PrintAll(List<Customer> customers)
        {
            foreach (Customer c in customers)
            {
                Console.WriteLine(c);
            }
        }
    }
}