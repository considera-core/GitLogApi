namespace GitLogApi.Exceptions;

public class BranchNotFoundException : GitCommandException
{
    public BranchNotFoundException(
        int exitCode, string repoPath, string branch, string command, string stdErr, string stdOut)
        : base($"Branch not found: {branch} (repo: {repoPath})", exitCode, repoPath, branch, command, stdErr, stdOut) { }
}