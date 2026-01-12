namespace GitLogApi.Exceptions;

public class NotAGitRepoException : GitCommandException
{
    public NotAGitRepoException(
        int exitCode, string repoPath, string branch, string command, string stdErr, string stdOut)
        : base($"Not a git repository: {repoPath}", exitCode, repoPath, branch, command, stdErr, stdOut) { }
}