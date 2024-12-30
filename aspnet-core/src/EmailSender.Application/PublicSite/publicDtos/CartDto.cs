using EmailSender.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.PublicSite.publicDtos
{
    public class CartDto
    {
        public int CartID { get; set; }
        public long userID { get; set; }

        public int productID { get; set; }

        public int quantity { get; set; }

        public virtual ProductDto products { get; set; }
    }

    public class CartQuantityDto
    {
        public int cartId { get; set; }
        public int quantitychange { get; set; }
        

    }
}
