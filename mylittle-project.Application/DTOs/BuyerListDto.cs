using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//BuyerListDto.cs is a Data Transfer Object (DTO) that represents a buyer's information in a list format.
//It is used to transfer data between different layers of the application, such as from the service layer to the presentation layer.
//This DTO includes properties like Id, BuyerName, Email, PhoneNumber, TotalOrders, TenantId, DealerId, IsActive, Status, and TenantName.
//It is designed to be simple and lightweight for efficient data transfer.

namespace mylittle_project.Application.DTOs
{
    public class BuyerListDto
    {
        public Guid Id { get; set; }

        public string BuyerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public int TotalOrders { get; set; }
        public Guid TenantId { get; set; }
        public Guid DealerId { get; set; }

        public bool IsActive { get; set; }
        public string Status { get; set; }= string.Empty;
        public string TenantName { get; set; } = string.Empty;

    }
}
