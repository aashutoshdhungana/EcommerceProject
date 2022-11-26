using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Core.Interfaces
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
