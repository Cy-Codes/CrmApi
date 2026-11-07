using System.Collections.Immutable;
using Domain.Dynamics365;

namespace Infrastructure.Repositories;

/// <summary>
/// Pure request-to-column mappings and server-owned audit columns for Dynamics 365 writes.
/// </summary>
public static class Dynamics365WriteColumns
{
    public const string AccountTable = "Dynamics365.Account";
    public const string AccountIdColumn = "accountid";
    public const string ContactTable = "Dynamics365.Contact";
    public const string ContactIdColumn = "contactid";
    public const string OpportunityTable = "Dynamics365.Opportunity";
    public const string OpportunityIdColumn = "opportunityid";

    private const string CreatedOn = "createdon";
    private const string CreatedBy = "createdby";
    private const string ModifiedOn = "modifiedon";
    private const string ModifiedBy = "modifiedby";

    public static ImmutableDictionary<string, object?> ToColumns(this AccountWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("name", r.Name)
            .With("accountnumber", r.AccountNumber)
            .With("parentaccountid", r.ParentAccountId)
            .With("telephone1", r.Telephone1)
            .With("emailaddress1", r.EmailAddress1)
            .With("websiteurl", r.Websiteurl)
            .With("address1_line1", r.Address1_Line1)
            .With("address1_city", r.Address1_City)
            .With("address1_stateorprovince", r.Address1_StateOrProvince)
            .With("address1_postalcode", r.Address1_PostalCode)
            .With("address1_country", r.Address1_Country)
            .With("revenue", r.Revenue)
            .With("numberofemployees", r.NumberOfEmployees)
            .With("industrycode", r.Industrycode)
            .With("description", r.Description)
            .With("ownerid", r.OwnerId)
            .With("owningbusinessunit", r.Owningbusinessunit)
            .WithOptional("statecode", r.StateCode)
            .WithOptional("statuscode", r.StatusCode);

    public static ImmutableDictionary<string, object?> ToColumns(this ContactWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("parentcustomerid", r.ParentCustomerId)
            .With("firstname", r.FirstName)
            .With("lastname", r.LastName)
            .With("fullname", r.ToFullName())
            .With("jobtitle", r.JobTitle)
            .With("emailaddress1", r.EmailAddress1)
            .With("telephone1", r.Telephone1)
            .With("mobilephone", r.Mobilephone)
            .With("address1_line1", r.Address1_Line1)
            .With("address1_city", r.Address1_City)
            .With("address1_stateorprovince", r.Address1_StateOrProvince)
            .With("address1_postalcode", r.Address1_PostalCode)
            .With("address1_country", r.Address1_Country)
            .With("leadsourcecode", r.Leadsourcecode)
            .With("description", r.Description)
            .With("ownerid", r.OwnerId)
            .With("owningbusinessunit", r.Owningbusinessunit)
            .WithOptional("statecode", r.StateCode)
            .WithOptional("statuscode", r.StatusCode);

    public static ImmutableDictionary<string, object?> ToColumns(this OpportunityWriteRequest r) =>
        WriteColumnExtensions.Empty
            .With("name", r.Name)
            .With("parentaccountid", r.ParentAccountId)
            .With("parentcontactid", r.Parentcontactid)
            .With("pricelevelid", r.PriceLevelId)
            .With("originatingleadid", r.OriginatingLeadId)
            .With("estimatedvalue", r.EstimatedValue)
            .With("actualvalue", r.ActualValue)
            .With("estimatedclosedate", r.EstimatedCloseDate.AsDate())
            .With("actualclosedate", r.ActualCloseDate.AsDate())
            .With("closeprobability", r.CloseProbability)
            .With("stepname", r.Stepname)
            .With("salesstage", r.Salesstage)
            .With("description", r.Description)
            .With("ownerid", r.OwnerId)
            .With("owningbusinessunit", r.Owningbusinessunit)
            .WithOptional("statecode", r.StateCode)
            .WithOptional("statuscode", r.StatusCode);

    /// <summary>Pure derivation of the NOT NULL <c>fullname</c> column from the contact's name parts.</summary>
    public static string ToFullName(this ContactWriteRequest r) =>
        string.Join(' ', new[] { r.FirstName, r.LastName }.Where(static part => !string.IsNullOrWhiteSpace(part)));

    /// <summary>Adds the identifier and audit columns for an INSERT; the owner doubles as creator and modifier.</summary>
    public static ImmutableDictionary<string, object?> WithCreateAudit(
        this ImmutableDictionary<string, object?> columns,
        string idColumn,
        Guid id,
        Guid ownerId,
        DateTime utcNow) =>
        columns
            .With(idColumn, id)
            .With(CreatedOn, utcNow)
            .With(CreatedBy, ownerId)
            .With(ModifiedOn, utcNow)
            .With(ModifiedBy, ownerId);

    /// <summary>Adds the modification audit columns for an UPDATE.</summary>
    public static ImmutableDictionary<string, object?> WithUpdateAudit(
        this ImmutableDictionary<string, object?> columns,
        Guid ownerId,
        DateTime utcNow) =>
        columns
            .With(ModifiedOn, utcNow)
            .With(ModifiedBy, ownerId);
}
