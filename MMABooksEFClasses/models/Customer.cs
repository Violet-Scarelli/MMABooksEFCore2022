﻿using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.models
{
    public partial class Customer
    {
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string StateCode { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
		public override string ToString()
		{
			return CustomerId + ", " + Name + ", " + Address + ", " + City + ", " + StateCode + ", " + ZipCode;
		}
		public virtual State? State { get; set; } = null!;
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
