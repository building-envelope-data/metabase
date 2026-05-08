using System;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Metabase.GraphQl;

namespace Metabase.Data.OpenIdConnect;

[JsonPolymorphic(TypeDiscriminatorPropertyName = GraphQlConstants.TypeDiscriminatorPropertyName)]
[JsonDerivedType(typeof(User), typeDiscriminator: nameof(User))]
[JsonDerivedType(typeof(OpenIdConnectApplication), typeDiscriminator: nameof(OpenIdConnectApplication))]
public interface IOpenIdConnectSubject
{
    private const string ClientSubjectPrefix = "client:";

    public static string BuildUserSubject(Guid userId) =>
        userId.ToString("D");

    public static string BuildClientSubject(string clientId) =>
        $"{ClientSubjectPrefix}{clientId}";

    public static Task<T> SwitchSubjectAsync<T>(
        string? subject,
        Func<Guid, Task<T>> handleUserId,
        Func<string, Task<T>> handleClientId,
        Func<Task<T>> handleUnknownSubject
    )
    {
        if (subject is null)
        {
            return handleUnknownSubject();
        }
        if (subject.StartsWith(ClientSubjectPrefix, ignoreCase: false, culture: CultureInfo.InvariantCulture))
        {
            return handleClientId(subject[ClientSubjectPrefix.Length..]);
        }
        if (Guid.TryParse(subject, out Guid userId))
        {
            return handleUserId(userId);
        }
        return handleUnknownSubject();
    }
}