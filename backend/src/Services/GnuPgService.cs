using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Quartz.Util;

namespace Metabase.Services;

public static class GnuPgService
{
    public const string KeyServerUrl = "hkps://keys.openpgp.org/";

    public static async Task<bool> DoesGnuPgKeyExist(
        string fingerprint
    )
    {
        if (fingerprint.IsNullOrWhiteSpace())
        {
            throw new ArgumentException($"The fingerprint '{fingerprint}' consists only of whitespaces.");
        }
        var (success, output, diagnostics) = await ExecuteCommand(
            $"gpg --batch --with-colons --keyserver '{KeyServerUrl}' --search-keys '{fingerprint}'"
        );
        return success;
    }

    private static async Task<(bool Success, string Output, string Diagnostics)> ExecuteCommand(
        string command,
        string? input = null
    )
    {
        var escapedCommand = command.Replace("\"", "\\\"");
        var process = Process.Start(
            new ProcessStartInfo()
            {
                FileName = "bash",
                Arguments = $"-c \"{escapedCommand}\"",
                ErrorDialog = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        ) ?? throw new InvalidOperationException($"The process for command '${command}' with the input '${input}' failed to start.");
        if (input is not null)
        {
            await process.StandardInput.WriteLineAsync(input);
            await process.StandardInput.FlushAsync();
        }
        process.StandardInput.Close();
        var output = process.StandardOutput.ReadToEnd();
        var diagnostics = process.StandardError.ReadToEnd();
        await process.WaitForExitAsync();
        return (process.ExitCode == 0, output, diagnostics);
    }

}