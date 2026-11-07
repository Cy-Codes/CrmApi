using System.Collections.Immutable;
using Domain.HubSpot;

namespace Infrastructure.Repositories;

/// <summary>
/// Pure request-to-column mappings and server-owned audit columns for HubSpot writes.
/// </summary>
public static class HubSpotWriteColumns
{
    public const string CompanyTable = "HubSpot.Company";
    public const string CompanyIdColumn = "companyId";
    public const string ContactTable = "HubSpot.Contact";
    public const string ContactIdColumn = "contactId";
    public const string DealTable = "HubSpot.Deal";
    public const string DealIdColumn = "dealId";

    private const string CreatedAt = "createdAt";
    private const string UpdatedAt = "updatedAt";

    public static ImmutableDictionary<string, object?> ToColumns(this CompanyWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("name", r.Name)
            .With("domain", r.Domain)
            .With("industry", r.Industry)
            .With("phone", r.Phone)
            .With("address", r.Address)
            .With("city", r.City)
            .With("state", r.State)
            .With("zip", r.Zip)
            .With("country", r.Country)
            .With("annualRevenue", r.AnnualRevenue)
            .With("numberOfEmployees", r.NumberOfEmployees)
            .With("description", r.Description)
            .With("ownerId", r.OwnerId)
            .WithOptional("lifecyclestage", r.Lifecyclestage)
            .WithOptional("archived", r.Archived);

    public static ImmutableDictionary<string, object?> ToColumns(this ContactWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("email", r.Email)
            .With("firstName", r.FirstName)
            .With("lastName", r.LastName)
            .With("phone", r.Phone)
            .With("mobilePhone", r.MobilePhone)
            .With("jobTitle", r.JobTitle)
            .With("associatedCompanyId", r.AssociatedCompanyId)
            .With("ownerId", r.OwnerId)
            .WithOptional("lifecyclestage", r.Lifecyclestage)
            .With("leadStatus", r.LeadStatus)
            .With("address", r.Address)
            .With("city", r.City)
            .With("state", r.State)
            .With("zip", r.Zip)
            .With("country", r.Country)
            .WithOptional("archived", r.Archived);

    public static ImmutableDictionary<string, object?> ToColumns(this DealWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("dealName", r.DealName)
            .With("pipelineId", r.PipelineId)
            .With("dealStageId", r.DealStageId)
            .With("amount", r.Amount)
            .With("closedate", r.Closedate.AsUtc())
            .With("dealType", r.DealType)
            .With("ownerId", r.OwnerId)
            .WithOptional("archived", r.Archived);

    /// <summary>Adds the identifier and audit columns for an INSERT.</summary>
    public static ImmutableDictionary<string, object?> WithCreateAudit(
        this ImmutableDictionary<string, object?> columns,
        string idColumn,
        long id,
        DateTime utcNow) =>
        columns
            .With(idColumn, id)
            .With(CreatedAt, utcNow)
            .With(UpdatedAt, utcNow);

    /// <summary>Adds the modification audit column for an UPDATE.</summary>
    public static ImmutableDictionary<string, object?> WithUpdateAudit(
        this ImmutableDictionary<string, object?> columns,
        DateTime utcNow) =>
        columns.With(UpdatedAt, utcNow);
}
