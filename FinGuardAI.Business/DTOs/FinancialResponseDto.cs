namespace FinGuardAI.DataAccess.DTOs
{
    public partial class FinancialResponseDTO
    {
        // DTO للـ FinancialResponse
        public class FinancialResponseDto
        {
            public int Id { get; set; }
            public int RequestId { get; set; }
            public string Decision { get; set; }
            public decimal AcceptedAmount { get; set; }
            public string Justification { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CreatorName { get; set; }

          public  FinancialResponseDto() { }

            public FinancialResponseDto(int id, int requestId, string decision, decimal acceptedAmount,
                                        string justification, DateTime createdAt, string creatorName)
            {
                Id = id;
                RequestId = requestId;
                Decision = decision;
                AcceptedAmount = acceptedAmount;
                Justification = justification;
                CreatedAt = createdAt;
                CreatorName = creatorName;
            }
        }
    }
}

