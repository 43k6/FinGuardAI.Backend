namespace FinGuardAI.DataAccess.DTOs
{
    public partial class FinancialResponseDTO
    {
        // DTO للـ AuditLog
        public class AuditLogDto
        {
            public int Id { get; set; }
            public int ActionId { get; set; }
            public string ActionType { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CreatorName { get; set; }

            public AuditLogDto(int id, int actionId, string actionType, DateTime createdAt, string creatorName)
            {
                Id = id;
                ActionId = actionId;
                ActionType = actionType;
                CreatedAt = createdAt;
                CreatorName = creatorName;
            }
        }
    }
}

