namespace GitLogApi.Entities.Config;

public abstract class BaseProjectConfig
{
    public abstract RepoConfig GetApp(string app);

    public abstract List<RepoConfig> GetApps();
}