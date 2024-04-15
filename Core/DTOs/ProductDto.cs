﻿using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public Decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}