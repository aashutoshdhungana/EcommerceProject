using ECommerce.Application.Core.Interfaces;
using ECommerceServer.Application.Core.DTOs.Enumerations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Application.Core.Entities

{
    public class Order: BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public uint Quantity { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Total")]
        public double TotalPrice { get; set; }

        [DisplayName("Status")]
        public OrderStatus Status { get; set; }
        public DateTime OrderPlacementTime { get; set; }
        public DateTime DeliveryDate { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
    }
}
