namespace mylittle_project.Application.DTOs
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public string BuyerName { get; set; } = string.Empty;
        public string Portal { get; set; } = string.Empty;
        public string Dealer { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
