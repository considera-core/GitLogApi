using System.Text.RegularExpressions;

namespace GitLogApi.Exceptions;

public static class GitErrorClassifier
{
    private static readonly Regex DirtyFilesRegex =
        new(@"would be overwritten by checkout:\s*(?<files>(?:\r?\n\s+.+)+)",
            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

    public static Exception Classify(
        int exitCode,
        string repoPath,
        string branch,
        string command,
        string stdout,
        string stderr)
    {
        var text = (stderr ?? "") + "\n" + (stdout ?? "");
        text = text.Trim();

        // PUSHD/path errors (cmd.exe errors)
        if (text.Contains("The system cannot find the path specified", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("cannot find the path", StringComparison.OrdinalIgnoreCase))
            return new RepoPathNotFoundException(exitCode, repoPath, branch, command, stderr, stdout);

        if (text.Contains("The syntax of the command is incorrect", StringComparison.OrdinalIgnoreCase))
            return new GitCommandException("CMD syntax error (bad quoting or separators).", exitCode, repoPath, branch, command, stderr, stdout);

        // Not a repo
        if (text.Contains("fatal: not a git repository", StringComparison.OrdinalIgnoreCase))
            return new NotAGitRepoException(exitCode, repoPath, branch, command, stderr, stdout);

        // Dirty tree blocks checkout (your example)
        if (text.Contains("would be overwritten by checkout", StringComparison.OrdinalIgnoreCase))
        {
            var files = ExtractDirtyFiles(text);
            return new DirtyWorkingTreeException(exitCode, repoPath, branch, command, stderr, stdout, files);
        }

        // Branch missing
        if (text.Contains("did not match any file(s) known to git", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("pathspec", StringComparison.OrdinalIgnoreCase))
            return new BranchNotFoundException(exitCode, repoPath, branch, command, stderr, stdout);

        // Auth/network
        if (text.Contains("Authentication failed", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("fatal: could not read Username", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("Permission denied (publickey)", StringComparison.OrdinalIgnoreCase))
            return new AuthFailedException(exitCode, repoPath, branch, command, stderr, stdout);

        if (text.Contains("Could not resolve host", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("Failed to connect", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("Connection timed out", StringComparison.OrdinalIgnoreCase))
            return new GitCommandException("Network error talking to remote.", exitCode, repoPath, branch, command, stderr, stdout);

        // Pull/merge conflicts
        if (text.Contains("CONFLICT", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("Automatic merge failed", StringComparison.OrdinalIgnoreCase))
            return new MergeConflictException(exitCode, repoPath, branch, command, stderr, stdout);

        // Divergent branches (git needs pull strategy)
        if (text.Contains("Need to specify how to reconcile divergent branches", StringComparison.OrdinalIgnoreCase))
            return new GitCommandException("Pull strategy not set (rebase/merge/ff-only).", exitCode, repoPath, branch, command, stderr, stdout);

        // Lock file
        if (text.Contains("index.lock", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("Another git process seems to be running", StringComparison.OrdinalIgnoreCase))
            return new IndexLockException(exitCode, repoPath, branch, command, stderr, stdout);

        // Remote issues
        if (text.Contains("does not appear to be a git repository", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("Could not read from remote repository", StringComparison.OrdinalIgnoreCase))
            return new GitCommandException("Remote repo not accessible or remote misconfigured.", exitCode, repoPath, branch, command, stderr, stdout);

        // Fallback
        return new GitCommandException("Git command failed.", exitCode, repoPath, branch, command, stderr, stdout);
    }

    private static IReadOnlyList<string> ExtractDirtyFiles(string text)
    {
        var match = DirtyFilesRegex.Match(text);
        if (!match.Success) return Array.Empty<string>();

        var block = match.Groups["files"].Value;
        return block
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();
    }
}