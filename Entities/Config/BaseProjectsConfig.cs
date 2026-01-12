namespace GitLogApi.Entities.Config;

public abstract class BaseProjectsConfig
{
    public abstract BaseProjectConfig GetProjectConfig(string projectName);
}