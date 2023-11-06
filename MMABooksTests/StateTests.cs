using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.models;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class StateTests
    {
        // ignore this warning about making dbContext nullable.
        // if you add the ?, you'll get a warning wherever you use dbContext
        MMABooksContext dbContext;
        State? s;
        List<State>? states;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            states = dbContext.States.OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(53, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            s = dbContext.States.Find("OR");
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Console.WriteLine(s);
        }

        [Test]
        public void GetUsingWhere()
        {
            states = dbContext.States.Where(s => s.StateName.StartsWith("A")).OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(4, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        [Test]
        public void GetWithCustomersTest()
        {
            s = dbContext.States.Include("Customers").Where(s => s.StateCode == "OR").SingleOrDefault();
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Assert.AreEqual(5, s.Customers.Count);
            Console.WriteLine(s);
        }

        [Test]
        public void DeleteTest()
        {
            s = dbContext.States.Find("HI");
            dbContext.States.Remove(s);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.States.Find("HI"));
        }

        [Test]
        public void CreateTest()
        {
            State state = new State();
            state.StateCode = "??";
            state.StateName = "Where am I";
            dbContext.States.Add(state);
            dbContext.SaveChanges();
            State retrieved = dbContext.States.Find("??");
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Where am I", retrieved.StateName);

        }

        [Test]
        public void UpdateTest()
        {
            State state = new State();
            state.StateCode = "??";
            state.StateName = "Where am I";
            dbContext.States.Add(state);
            dbContext.SaveChanges();
            Assert.AreEqual("Where am I", dbContext.States.Find("??"));
            state.StateName = "I am here";
            dbContext.SaveChanges();
            Assert.AreEqual("I am here", dbContext.States.Find("??"));
            Console.WriteLine(dbContext.States.Find("??").ToString());
        }

        public void PrintAll(List<State> states)
        {
            foreach (State s in states)
            {
                Console.WriteLine(s);
            }
        }
    }
}