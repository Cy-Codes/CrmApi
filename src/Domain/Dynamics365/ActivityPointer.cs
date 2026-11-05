namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 ActivityPointer entity.
/// </summary>
public record ActivityPointer
{
    public Guid ActivityId { get; init; }
    public string ActivityTypeCode { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? Regardingobjectid_Account { get; init; }
    public Guid? Regardingobjectid_Contact { get; init; }
    public Guid? Regardingobjectid_Opportunity { get; init; }
    public Guid? Regardingobjectid_Incident { get; init; }
    public Guid OwnerId { get; init; }
    public Guid Owningbusinessunit { get; init; }
    public DateTime? ScheduledStart { get; init; }
    public DateTime? ScheduledEnd { get; init; }
    public DateTime? ActualStart { get; init; }
    public DateTime? ActualEnd { get; init; }
    public int? ActualDurationMinutes { get; init; }
    public int StateCode { get; init; }
    public int StatusCode { get; init; }
    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime ModifiedOn { get; init; }
    public Guid ModifiedBy { get; init; }
}
