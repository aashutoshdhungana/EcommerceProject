using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ECommerce.Application.Core.Interfaces;
using ECommerceServer.Application.Core.DTOs.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Core.Entities
{
    public class User: IdentityUser
    {
        [DisplayName("Wallet")]
        public double Wallet { get; set; }
    }
}
