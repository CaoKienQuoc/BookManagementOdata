using System;
using System.Collections.Generic;

namespace SE160271.BookManagementApplicationRepo.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public string? Isbn { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal? Price { get; set; }
        public int? PressId { get; set; }

        public virtual Press? Press { get; set; }
    }
}
