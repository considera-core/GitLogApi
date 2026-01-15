namespace GitLogApi.Entities.Config;

public interface IProjectConfig
{
    /// <summary>
    /// Project key
    /// </summary>
    public string Key { get; set; }

    RepoConfig GetApp(string app);

    List<RepoConfig> GetApps();
}