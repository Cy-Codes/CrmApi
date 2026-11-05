namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Event entity.
/// </summary>
public record Event
{
    public string Id { get; init; } = string.Empty;
    public string? WhoId { get; init; }
    public string? WhatId { get; init; }
    public int WhoCount { get; init; }
    public int WhatCount { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string? Location { get; init; }
    public bool IsAllDayEvent { get; init; }
    public DateTime? ActivityDateTime { get; init; }
    public DateTime? ActivityDate { get; init; }
    public int? DurationInMinutes { get; init; }
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
    public string? Description { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
