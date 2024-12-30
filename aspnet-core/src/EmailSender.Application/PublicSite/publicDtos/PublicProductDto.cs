using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.PublicSite.publicDtos
{
    public class PublicProductDto
    {

        public int productID { get; set; }
        public string description { get; set; }
        public byte[] Thumbnail { get; set; }
        public decimal BasePrice { get; set; }
        public int Rating { get; set; }

    }
}
