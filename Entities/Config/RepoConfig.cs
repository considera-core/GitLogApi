namespace GitLogApi.Entities.Config;

public class RepoConfig
{
    /// <summary>
    /// Key
    /// </summary>
    public string RepoKey { get; set; }

    /// <summary>
    /// Root git branch. Either master or main
    /// </summary>
    public string RootBranch { get; set; }

    /// <summary>
    /// Relative repository path
    /// </summary>
    public string Path { get; set; }

    public BranchOverridesConfig? BranchOverrides { get; set; }

    public string GetBranch(string env) =>
        env.ToLower() switch
        {
            "dev" => BranchOverrides?.Dev ?? RootBranch,
            "ci" => BranchOverrides?.Ci ?? RootBranch,
            "qa" => BranchOverrides?.Qa ?? RootBranch,
            "prod" => BranchOverrides?.Prod ?? RootBranch,
            _ => throw new ArgumentException($"Env '{env}' not found in RepoConfig.")
        };
}

public class BranchOverridesConfig
{
    public string? Dev { get; set; }

    public string? Ci { get; set; }

    public string? Qa { get; set; }

    public string? Prod { get; set; }
}