namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 Incident entity.
/// </summary>
public record Incident
{
    public Guid IncidentId { get; init; }
    public string TicketNumber { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public Guid? Customerid_Account { get; init; }
    public Guid? Customerid_Contact { get; init; }
    public int PriorityCode { get; init; }
    public int? SeverityCode { get; init; }
    public int? CaseTypeCode { get; init; }
    public string? Casesubject { get; init; }
    public string? Description { get; init; }
    public Guid OwnerId { get; init; }
    public Guid Owningbusinessunit { get; init; }
    public int StateCode { get; init; }
    public int StatusCode { get; init; }
    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime ModifiedOn { get; init; }
    public Guid ModifiedBy { get; init; }
}
