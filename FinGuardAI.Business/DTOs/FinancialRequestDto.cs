namespace FinGuardAI.DataAccess.DTOs
{
    public partial class FinancialResponseDTO
    {
        // DTO للـ FinancialRequest
        public class FinancialRequestDto
        {
            public int Id { get; set; }
            public string RequestName { get; set; }
            public decimal Amount { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public string State { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CreatorName { get; set; }
            public FinancialRequestDto() { }

            public FinancialRequestDto(int id, string requestName, decimal amount, string category,
                                       string description, string state, DateTime createdAt, string creatorName)
            {
                Id = id;
                RequestName = requestName;
                Amount = amount;
                Category = category;
                Description = description;
                State = state;
                CreatedAt = createdAt;
                CreatorName = creatorName;
            }
        }
    }
}

