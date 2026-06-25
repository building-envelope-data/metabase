using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz.Util;

namespace Metabase.Services;

[SuppressMessage("Naming", "CA1707")]
public enum GnuPgKeyVerificationResult
{
    SUCCESS,
    UNKNOWN_FAILURE,
    MALFORMED_FINGERPRINT,
    NON_EXISTENT,
    WRONG_EMAIL_ADDRESS
}

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "GnuPG: Failed to search the key with the fingerprint '{Fingerprint}' with the exit code {ExitCode}, the output '{Output}' and the diagnostic information '{Diagnostics}.")]
    public static partial void FailedToSearch(this ILogger<GnuPgService> logger, string fingerprint, int exitCode, string output, string diagnostics);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "GnuPG: Failed to extract the email address from the output '{Output}' of searching the key with the fingerprint '{Fingerprint}'. The diagnostic information is '{Diagnostics}.")]
    public static partial void FailedToExtractEmailAddress(this ILogger<GnuPgService> logger, string fingerprint, string output, string diagnostics);

    [LoggerMessage(
    Level = LogLevel.Debug,
    Message = "GnuPG: Execute Command: {Command}")]
    public static partial void ExecuteCommand(this ILogger<GnuPgService> logger, string command);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "GnuPG Execute Command: {Error}")]
    public static partial void ExecuteCommandDiagnostics(this ILogger<GnuPgService> logger, string error);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "GnuPG: Execute Command Output: {Output}")]
    public static partial void ExecuteCommandOutput(this ILogger<GnuPgService> logger, string output);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "GnuPG: Execute Command Result: {result}")]
    public static partial void ExecuteCommandExitCode(this ILogger<GnuPgService> logger, int result);
}

public sealed partial class GnuPgService(
    ILogger<GnuPgService> logger
)
{
    public const string KeyServerUrl = "hkps://keys.openpgp.org/";

    [GeneratedRegex(@"<([^>]+)>")]
    private static partial Regex ExtractEmailAddressRegex();

    public async Task<GnuPgKeyVerificationResult> VerifyGnuPgKeyAsync(
        string fingerprint,
        string emailAddress,
        CancellationToken cancellationToken
    )
    {
        if (fingerprint.IsNullOrWhiteSpace())
        {
            return GnuPgKeyVerificationResult.MALFORMED_FINGERPRINT;
        }
        var (exitCode, output, diagnostics) = await ExecuteCommandAsync(
            $"gpg --batch --with-colons --keyserver '{KeyServerUrl}' --search-keys '{fingerprint}'",
            null,
            cancellationToken
        );
        if (exitCode is 2)
        {
            return GnuPgKeyVerificationResult.NON_EXISTENT;
        }
        // Exit code 1 is a general failure of executing `gpg --search-keys`.
        if (exitCode is not 0)
        {
            logger.FailedToSearch(fingerprint, exitCode, output, diagnostics);
            return GnuPgKeyVerificationResult.UNKNOWN_FAILURE;
        }
        var extractedEmailAddress = ExtractEmailAddress(output);
        if (extractedEmailAddress is null)
        {
            logger.FailedToExtractEmailAddress(fingerprint, output, diagnostics);
        }
        if (extractedEmailAddress is not null && extractedEmailAddress.Trim() != emailAddress.Trim())
        {
            return GnuPgKeyVerificationResult.WRONG_EMAIL_ADDRESS;
        }
        return GnuPgKeyVerificationResult.SUCCESS;
    }

    private static string? ExtractEmailAddress(
        string output
    )
    {
        var userId = WebUtility.UrlDecode(
            output
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault(line => line.StartsWith("uid:", StringComparison.InvariantCulture))
            ?.Split(':')
            ?.ElementAtOrDefault(1)
        );
        if (userId is null)
        {
            return null;
        }
        var match = ExtractEmailAddressRegex().Match(userId);
        if (!match.Success)
        {
            return null;
        }
        return match.Groups[1].Value;
    }

    private async Task<(int ExitCode, string Output, string Diagnostics)> ExecuteCommandAsync(
        string command,
        string? input,
        CancellationToken cancellationToken
    )
    {
        logger.ExecuteCommand(command);
        var process = Process.Start(
            new ProcessStartInfo()
            {
                FileName = "bash",
                ArgumentList = {
                    "-o", "errexit",
                    "-o", "errtrace",
                    "-o", "nounset",
                    "-o", "pipefail",
                    "-c", command
                },
                ErrorDialog = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        ) ?? throw new InvalidOperationException($"The process for command '{command}' with the input '{input}' failed to start.");
        if (input is not null)
        {
            await process.StandardInput.WriteLineAsync(input.AsMemory(), cancellationToken);
            await process.StandardInput.FlushAsync(cancellationToken);
        }
        process.StandardInput.Close();
        var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
        var diagnostics = await process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);
        logger.ExecuteCommandOutput(output);
        logger.ExecuteCommandDiagnostics(diagnostics);
        logger.ExecuteCommandExitCode(process.ExitCode);
        return (process.ExitCode, output, diagnostics);
    }
}