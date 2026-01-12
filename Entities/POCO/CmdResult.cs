namespace GitLogApi.Entities.POCO;

public sealed record CmdResult(int ExitCode, string StdOut, string StdErr);