using System.Diagnostics;
using GitLogApi.Entities.POCO;
using GitLogApi.Exceptions;

namespace GitLogApi.Extensions;

public static class CommandExtensions
{
    public static bool Lock;

    public static async Task<CmdResult> Execute(this string command, string? workingDir = null)
    {
        var processId = Guid.NewGuid();
        Console.WriteLine($"[exec:{processId}] {command} (in {workingDir ?? "."})");
        var process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = "/c " + command;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WorkingDirectory = workingDir ?? ".";
        process.Start();
        var outTask = process.StandardOutput.ReadToEndAsync();
        var errTask = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return new CmdResult(process.ExitCode, await outTask, await errTask);
    }

    public static CmdResult Assert(this CmdResult result, string repoPath, string branch, string command) =>
        result.ExitCode == 0
            ? result
            : throw GitErrorClassifier.Classify(result.ExitCode, repoPath, branch, command, result.StdOut, result.StdErr);
}