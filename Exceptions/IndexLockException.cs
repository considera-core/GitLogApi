namespace GitLogApi.Exceptions;

public class IndexLockException : GitCommandException
{
    public IndexLockException(
        int exitCode, string repoPath, string branch, string command, string stdErr, string stdOut)
        : base($"Git index.lock present (another git process running?) in {repoPath}", exitCode, repoPath, branch, command, stdErr, stdOut) { }
}