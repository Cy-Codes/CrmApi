namespace Domain.Common;

/// <summary>
/// Side-effect boundary generating server-owned primary keys, injectable so tests can pin identifiers.
/// </summary>
public interface ICrmIdGenerator
{
    /// <summary>Generates an 18-character Salesforce-style identifier starting with the 3-character key prefix.</summary>
    string NewSalesforceId(string keyPrefix);

    /// <summary>Generates a positive, time-ordered 64-bit HubSpot object identifier.</summary>
    long NewHubSpotId();

    /// <summary>Generates a Dynamics 365 GUID identifier.</summary>
    Guid NewDynamicsId();
}
