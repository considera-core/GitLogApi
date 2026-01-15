namespace GitLogApi.Entities.Config;

public interface IProjectConfig
{
    /// <summary>
    /// Project key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Project base path (optional)
    /// </summary>
    public string? BasePath { get; set; }

    /// <summary>
    /// If true, do not include in any queries
    /// </summary>
    public bool? Disabled { get; set; }

    RepoConfig GetApp(string app);

    List<RepoConfig> GetApps();
}