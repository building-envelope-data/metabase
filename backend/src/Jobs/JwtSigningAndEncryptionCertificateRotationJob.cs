using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Metabase.Authentication;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Metabase.Jobs;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to create certificate. Distinguished Name: '{DistinguishedName}'")]
    internal static partial void FailedToCreateCertificate(
        this ILogger<JwtSigningAndEncryptionCertificateRotationJob> logger,
        string distinguishedName,
        Exception exception
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store certificates.")]
    internal static partial void FailedToStoreCertificates(
        this ILogger<JwtSigningAndEncryptionCertificateRotationJob> logger,
        Exception exception
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to cleanup long expired certificates.")]
    internal static partial void FailedToCleanupLongExpiredCertificates(
        this ILogger<JwtSigningAndEncryptionCertificateRotationJob> logger,
        Exception exception
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to remove certificate. Thumbprint: '{Thumbprint}'. Distinguished Name: '{DistinguishedName}'.")]
    internal static partial void FailedToRemoveCertificate(
        this ILogger<JwtSigningAndEncryptionCertificateRotationJob> logger,
        string thumbprint,
        string distinguishedName,
        Exception exception
    );
}

public sealed class JwtSigningAndEncryptionCertificateRotationJob(
    ILogger<JwtSigningAndEncryptionCertificateRotationJob> logger
)
: IJob
{
    public const string KeyName = "JwtSigningAndEncryptionKeyRotationJob";
    public const string TriggerIdentityName = $"{KeyName}.Trigger";

    private static readonly TimeSpan s_notBeforeOffset = TimeSpan.FromMinutes(-5);
    private static readonly TimeSpan s_notAfterOffset = TimeSpan.FromDays(3);

    public async Task Execute(IJobExecutionContext context)
    {
        StoreCertificatesWithErrorHandling(
            CreateSigningCertificateWithErrorHandling(OpenIdConnectConstants.Server.SigningSubjectDistinguishedName),
            CreateEncryptionCertificateWithErrorHandling(OpenIdConnectConstants.Server.EncryptionSubjectDistinguishedName),
            CreateSigningCertificateWithErrorHandling(OpenIdConnectConstants.Client.SigningSubjectDistinguishedName),
            CreateEncryptionCertificateWithErrorHandling(OpenIdConnectConstants.Client.EncryptionSubjectDistinguishedName)
        );
        CleanupLongExpiredCertificatesWithErrorHandling(
            OpenIdConnectConstants.Server.SigningSubjectDistinguishedName,
            OpenIdConnectConstants.Server.EncryptionSubjectDistinguishedName,
            OpenIdConnectConstants.Client.SigningSubjectDistinguishedName,
            OpenIdConnectConstants.Client.EncryptionSubjectDistinguishedName
        );
        // TODO: Trigger OpenIddict reload. Currently done dialy with a cron job that restart all services.
    }

    public static X509Certificate2 CreateSigningCertificate(string distinguishedName)
    {
        // In the future use ECDSA.
        // using var algorithm = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        // var request = new CertificateRequest(
        //     new X500DistinguishedName(distinguishedName),
        //     algorithm,
        //     HashAlgorithmName.SHA256
        // );
        using var algorithm = RSA.Create(keySizeInBits: 3072);
        var request = new CertificateRequest(
            new X500DistinguishedName(distinguishedName),
            algorithm,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                X509KeyUsageFlags.DigitalSignature,
                critical: true
            )
        );
        var now = TimeProvider.System.GetUtcNow();
        var ephemeralCertificate = request.CreateSelfSigned(
            notBefore: now.Add(s_notBeforeOffset),
            notAfter: now.Add(s_notAfterOffset)
        );
        // Roundtrip through PFX to ensure the private key is persisted with the certificate
        return X509CertificateLoader.LoadPkcs12(
            ephemeralCertificate.Export(X509ContentType.Pfx),
            null,
            X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.UserKeySet
        );
    }

    private X509Certificate2? CreateSigningCertificateWithErrorHandling(string distinguishedName)
    {
        try
        {
            return CreateSigningCertificate(distinguishedName);
        }
        catch (Exception exception)
        {
            logger.FailedToCreateCertificate(distinguishedName, exception);
            return null;
        }
    }

    public static X509Certificate2 CreateEncryptionCertificate(string distinguishedName)
    {
        // In the furture use `ML-KEM`.
        using var algorithm = RSA.Create(keySizeInBits: 3072);
        var request = new CertificateRequest(
            new X500DistinguishedName(distinguishedName),
            algorithm,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                X509KeyUsageFlags.KeyEncipherment,
                critical: true
            )
        );
        var now = TimeProvider.System.GetUtcNow();
        var ephemeralCertificate = request.CreateSelfSigned(
            notBefore: now.Add(s_notBeforeOffset),
            notAfter: now.Add(s_notAfterOffset)
        );
        // Roundtrip through PFX to ensure the private key is persisted with the certificate
        return X509CertificateLoader.LoadPkcs12(
            ephemeralCertificate.Export(X509ContentType.Pfx),
            null,
            X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.UserKeySet
        );
    }

    private X509Certificate2? CreateEncryptionCertificateWithErrorHandling(string distinguishedName)
    {
        try
        {
            return CreateEncryptionCertificate(distinguishedName);
        }
        catch (Exception exception)
        {
            logger.FailedToCreateCertificate(distinguishedName, exception);
            return null;
        }
    }

    private void StoreCertificatesWithErrorHandling(params X509Certificate2?[] certificates)
    {
        using var store = new X509Store(OpenIdConnectConstants.CertificateStoreName, OpenIdConnectConstants.CertificateStoreLocation);
        try
        {
            store.Open(OpenFlags.ReadWrite);
            foreach (var certificate in certificates)
            {
                if (certificate is not null)
                {
                    store.Add(certificate);
                }
            }
        }
        catch (Exception exception)
        {
            logger.FailedToStoreCertificates(exception);
        }
        finally
        {
            store.Close();
        }
    }

    public void CleanupLongExpiredCertificatesWithErrorHandling(params string[] distinguishedNames)
    {
        using var store = new X509Store(OpenIdConnectConstants.CertificateStoreName, OpenIdConnectConstants.CertificateStoreLocation);
        try
        {
            store.Open(OpenFlags.ReadWrite | OpenFlags.IncludeArchived);
            foreach (var distinguishedName in distinguishedNames)
            {
                var certificates = store.Certificates.Find(
                    X509FindType.FindBySubjectDistinguishedName,
                    distinguishedName,
                    validOnly: false
                );
                var now = TimeProvider.System.GetUtcNow();
                foreach (var certificate in certificates)
                {
                    // Use `NotAfterDaysOffset` as overlap period.
                    if (certificate.NotAfter.Add(OpenIdConnectConstants.RefreshTokenLifetime) < now)
                    {
                        try
                        {
                            store.Remove(certificate);
                        }
                        catch (Exception exception)
                        {
                            logger.FailedToRemoveCertificate(certificate.Thumbprint, distinguishedName, exception);
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            logger.FailedToCleanupLongExpiredCertificates(exception);
        }
        finally
        {
            store.Close();
        }
    }
}