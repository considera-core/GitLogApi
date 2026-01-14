namespace GitLogApi.Entities.Config;

public interface IProjectConfig
{
    RepoConfig GetApp(string app);

    List<RepoConfig> GetApps();
}