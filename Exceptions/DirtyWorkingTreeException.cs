namespace GitLogApi.Exceptions;

public class DirtyWorkingTreeException : GitCommandException
{
    public IReadOnlyList<string> Files { get; }

    public DirtyWorkingTreeException(
        int exitCode, string repoPath, string branch, string command, string stdErr, string stdOut, IReadOnlyList<string> files)
        : base($"Local changes would be overwritten by checkout in {repoPath}. Please fix first.", exitCode, repoPath, branch, command, stdErr, stdOut)
    {
        Files = files;
    }
}