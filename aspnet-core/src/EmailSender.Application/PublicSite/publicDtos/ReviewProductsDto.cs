using EmailSender.ProductDtos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.PublicSite.publicDtos
{
    public class ReviewProductsDto
    {
        public byte[] ProductThumbnail {  get; set; }

        public int ProductId { get; set; }

        public String ProductName {  get; set; }
      
    }

    
}
