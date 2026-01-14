namespace GitLogApi.Entities.Config;

public interface IProjectsConfig
{
    IProjectConfig GetProjectConfig(string projectName);
}