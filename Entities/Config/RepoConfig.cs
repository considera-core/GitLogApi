namespace GitLogApi.Entities.Config;

public class RepoConfig
{
    /// <summary>
    /// Repo key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Root git branch. Either master or main
    /// </summary>
    public string RootBranch { get; set; }

    /// <summary>
    /// Relative repository path
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// If true, do not include in any queries
    /// </summary>
    public bool? Disabled { get; set; }

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