namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Task entity.
/// </summary>
public record Task
{
    public string Id { get; init; } = string.Empty;
    public string? WhoId { get; init; }
    public string? WhatId { get; init; }
    public int WhoCount { get; init; }
    public int WhatCount { get; init; }
    public string Subject { get; init; } = string.Empty;
    public DateTime? ActivityDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public bool IsHighPriority { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsClosed { get; init; }
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
