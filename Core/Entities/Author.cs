﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string?  Description { get; set; }
        public ICollection<Book> Books { get; } = new List<Book>();
    }
}
