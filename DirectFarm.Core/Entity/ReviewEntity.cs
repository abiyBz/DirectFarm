using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class ReviewEntity
    {
        public Guid Id { get; set; } = Guid.Empty;
        //public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;
        //public Guid CustomerId { get; set; }
        public CustomerEntity Customer { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
