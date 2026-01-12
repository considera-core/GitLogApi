namespace GitLogApi.Exceptions;

public class AuthFailedException : GitCommandException
{
    public AuthFailedException(
        int exitCode, string repoPath, string branch, string command, string stdErr, string stdOut)
        : base($"Git authentication failed for repo: {repoPath}", exitCode, repoPath, branch, command, stdErr, stdOut) { }
}