using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.models
{
    public partial class State
    {
        public State()
        {
            Customers = new HashSet<Customer>();
        }

        public string StateCode { get; set; } = null!;
        public string StateName { get; set; } = null!;
		public override string ToString()
		{
			return StateCode + ", " + StateName;
		}
		public virtual ICollection<Customer> Customers { get; set; }
    }
}
