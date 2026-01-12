namespace GitLogApi.Exceptions;

public class MergeConflictException : GitCommandException
{
    public MergeConflictException(
        int exitCode, string repoPath, string branch, string command, string stdErr, string stdOut)
        : base($"Merge conflict while pulling {repoPath}", exitCode, repoPath, branch, command, stdErr, stdOut) { }
}