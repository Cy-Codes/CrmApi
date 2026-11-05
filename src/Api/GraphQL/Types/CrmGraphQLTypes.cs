using HotChocolate.Types;
using SfAccount = Domain.Salesforce.Account;
using SfContact = Domain.Salesforce.Contact;
using HsCompany = Domain.HubSpot.Company;
using HsContact = Domain.HubSpot.Contact;
using D365Account = Domain.Dynamics365.Account;
using D365Contact = Domain.Dynamics365.Contact;

namespace Api.GraphQL.Types;

public class SalesforceAccountType : ObjectType<SfAccount>
{
    protected override void Configure(IObjectTypeDescriptor<SfAccount> descriptor)
    {
        descriptor.Name("SalesforceAccount");
    }
}

public class SalesforceContactType : ObjectType<SfContact>
{
    protected override void Configure(IObjectTypeDescriptor<SfContact> descriptor)
    {
        descriptor.Name("SalesforceContact");
    }
}

public class HubSpotCompanyType : ObjectType<HsCompany>
{
    protected override void Configure(IObjectTypeDescriptor<HsCompany> descriptor)
    {
        descriptor.Name("HubSpotCompany");
    }
}

public class HubSpotContactType : ObjectType<HsContact>
{
    protected override void Configure(IObjectTypeDescriptor<HsContact> descriptor)
    {
        descriptor.Name("HubSpotContact");
    }
}

public class Dynamics365AccountType : ObjectType<D365Account>
{
    protected override void Configure(IObjectTypeDescriptor<D365Account> descriptor)
    {
        descriptor.Name("Dynamics365Account");
    }
}

public class Dynamics365ContactType : ObjectType<D365Contact>
{
    protected override void Configure(IObjectTypeDescriptor<D365Contact> descriptor)
    {
        descriptor.Name("Dynamics365Contact");
    }
}
