namespace GitLogApi.Entities.Config;

public interface IProjectsConfig
{
    string Key { get; set; }

    IProjectConfig GetProjectConfig(string projectName);
}