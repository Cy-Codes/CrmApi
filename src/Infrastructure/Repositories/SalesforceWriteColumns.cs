using System.Collections.Immutable;
using Domain.Salesforce;

namespace Infrastructure.Repositories;

/// <summary>
/// Pure request-to-column mappings and server-owned audit columns for Salesforce writes.
/// </summary>
public static class SalesforceWriteColumns
{
    public const string IdColumn = "Id";
    public const string AccountTable = "Salesforce.Account";
    public const string ContactTable = "Salesforce.Contact";
    public const string OpportunityTable = "Salesforce.Opportunity";
    public const string AccountKeyPrefix = "001";
    public const string ContactKeyPrefix = "003";
    public const string OpportunityKeyPrefix = "006";

    private const string CreatedDate = "CreatedDate";
    private const string CreatedById = "CreatedById";
    private const string LastModifiedDate = "LastModifiedDate";
    private const string LastModifiedById = "LastModifiedById";

    public static ImmutableDictionary<string, object?> ToColumns(this AccountWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("ParentId", r.ParentId)
            .With("Name", r.Name)
            .With("Type", r.Type)
            .With("BillingStreet", r.BillingStreet)
            .With("BillingCity", r.BillingCity)
            .With("BillingState", r.BillingState)
            .With("BillingPostalCode", r.BillingPostalCode)
            .With("BillingCountry", r.BillingCountry)
            .With("ShippingStreet", r.ShippingStreet)
            .With("ShippingCity", r.ShippingCity)
            .With("ShippingState", r.ShippingState)
            .With("ShippingPostalCode", r.ShippingPostalCode)
            .With("ShippingCountry", r.ShippingCountry)
            .With("Phone", r.Phone)
            .With("Fax", r.Fax)
            .With("Website", r.Website)
            .With("Industry", r.Industry)
            .With("AnnualRevenue", r.AnnualRevenue)
            .With("NumberOfEmployees", r.NumberOfEmployees)
            .With("Description", r.Description)
            .With("OwnerId", r.OwnerId);

    public static ImmutableDictionary<string, object?> ToColumns(this ContactWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("AccountId", r.AccountId)
            .With("LastName", r.LastName)
            .With("FirstName", r.FirstName)
            .With("Salutation", r.Salutation)
            .With("ReportsToId", r.ReportsToId)
            .With("Title", r.Title)
            .With("Department", r.Department)
            .With("Email", r.Email)
            .With("Phone", r.Phone)
            .With("MobilePhone", r.MobilePhone)
            .With("MailingStreet", r.MailingStreet)
            .With("MailingCity", r.MailingCity)
            .With("MailingState", r.MailingState)
            .With("MailingPostalCode", r.MailingPostalCode)
            .With("MailingCountry", r.MailingCountry)
            .With("LeadSource", r.LeadSource)
            .With("Description", r.Description)
            .With("OwnerId", r.OwnerId);

    public static ImmutableDictionary<string, object?> ToColumns(this OpportunityWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("AccountId", r.AccountId)
            .WithOptional("IsPrivate", r.IsPrivate)
            .With("Name", r.Name)
            .With("Description", r.Description)
            .With("StageName", r.StageName)
            .With("Amount", r.Amount)
            .With("Probability", r.Probability)
            .With("CloseDate", r.CloseDate.AsDate())
            .With("Type", r.Type)
            .With("LeadSource", r.LeadSource)
            .WithOptional("IsClosed", r.IsClosed)
            .WithOptional("IsWon", r.IsWon)
            .WithOptional("ForecastCategoryName", r.ForecastCategoryName)
            .With("CampaignId", r.CampaignId)
            .With("Pricebook2Id", r.Pricebook2Id)
            .With("OwnerId", r.OwnerId);

    /// <summary>Adds the identifier and audit columns for an INSERT; the owner doubles as creator and modifier.</summary>
    public static ImmutableDictionary<string, object?> WithCreateAudit(
        this ImmutableDictionary<string, object?> columns,
        string id,
        string ownerId,
        DateTime utcNow) =>
        columns
            .With(IdColumn, id)
            .With(CreatedDate, utcNow)
            .With(CreatedById, ownerId)
            .With(LastModifiedDate, utcNow)
            .With(LastModifiedById, ownerId);

    /// <summary>Adds the modification audit columns for an UPDATE.</summary>
    public static ImmutableDictionary<string, object?> WithUpdateAudit(
        this ImmutableDictionary<string, object?> columns,
        string ownerId,
        DateTime utcNow) =>
        columns
            .With(LastModifiedDate, utcNow)
            .With(LastModifiedById, ownerId);
}
