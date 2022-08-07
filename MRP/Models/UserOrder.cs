using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRP.Models
{
    public class UserOrder
    {
        public User user;
        public List<Product> product;
        public List<Order> order;
    }
}
