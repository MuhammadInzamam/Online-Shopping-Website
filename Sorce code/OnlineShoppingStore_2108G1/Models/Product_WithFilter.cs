using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore_2108G1.Models
{
    public class Product_WithFilter
    {
        public PagedList.IPagedList<Product> productList { get; set; }
    }

    public class Product_List
    {
        public PagedList.IPagedList<Product> productList { get; set; }
    }
}