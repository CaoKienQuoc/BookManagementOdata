using System;
using System.Collections.Generic;

namespace SE160271.BookManagementApplicationRepo.Models
{
    public partial class Press
    {
        public Press()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int? AddressId { get; set; }

        public virtual Address? Address { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
