using ECommerce.Application.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECommerce.Application.Core.Entities
{
    public class OrderHistory: BaseEntity
    {
        public ICollection<Order> Orders { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
    }
}
