using System.Security.Cryptography;
using Domain.Common;

namespace Infrastructure.Identifiers;

/// <summary>
/// Production <see cref="ICrmIdGenerator"/> using the system clock and cryptographic randomness.
/// </summary>
public sealed class CrmIdGenerator : ICrmIdGenerator
{
    public const int SalesforceIdLength = 18;
    public const int SalesforceKeyPrefixLength = 3;
    private const string SalesforceAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int HubSpotSequenceModulus = 1000;

    private readonly TimeProvider _timeProvider;

    public CrmIdGenerator(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public string NewSalesforceId(string keyPrefix)
    {
        if (keyPrefix is not { Length: SalesforceKeyPrefixLength })
        {
            throw new ArgumentException(
                $"Salesforce key prefix must be exactly {SalesforceKeyPrefixLength} characters.",
                nameof(keyPrefix));
        }

        var suffix = RandomNumberGenerator.GetString(SalesforceAlphabet, SalesforceIdLength - SalesforceKeyPrefixLength);
        return keyPrefix + suffix;
    }

    public long NewHubSpotId() =>
        _timeProvider.GetUtcNow().ToUnixTimeMilliseconds() * HubSpotSequenceModulus
        + RandomNumberGenerator.GetInt32(HubSpotSequenceModulus);

    public Guid NewDynamicsId() => Guid.NewGuid();
}
