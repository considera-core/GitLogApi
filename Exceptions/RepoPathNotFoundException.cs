namespace GitLogApi.Exceptions;

public class RepoPathNotFoundException : GitCommandException
{
    public RepoPathNotFoundException(
        int exitCode, string repoPath, string branch, string command, string stdErr, string stdOut)
        : base($"Repo path not found: {repoPath}", exitCode, repoPath, branch, command, stdErr, stdOut) { }
}