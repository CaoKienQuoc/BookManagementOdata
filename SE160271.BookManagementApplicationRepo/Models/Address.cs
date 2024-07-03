using System;
using System.Collections.Generic;

namespace SE160271.BookManagementApplicationRepo.Models
{
    public partial class Address
    {
        public Address()
        {
            Presses = new HashSet<Press>();
        }

        public int Id { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public virtual ICollection<Press> Presses { get; set; }
    }
}
