namespace GitLogApi.Exceptions;

public class GitCommandException : Exception
{
    public int ExitCode { get; }
    public string RepoPath { get; }
    public string Branch { get; }
    public string Command { get; }
    public string StdErr { get; }
    public string StdOut { get; }

    public GitCommandException(
        string message,
        int exitCode,
        string repoPath,
        string branch,
        string command,
        string stdErr,
        string stdOut) : base(message)
    {
        ExitCode = exitCode;
        RepoPath = repoPath;
        Branch = branch;
        Command = command;
        StdErr = stdErr;
        StdOut = stdOut;
    }
}