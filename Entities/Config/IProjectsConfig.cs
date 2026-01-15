namespace GitLogApi.Entities.Config;

public interface IProjectsConfig
{
    /// <summary>
    /// App key
    /// </summary>
    string Key { get; set; }

    IProjectConfig GetProjectConfig(string projectName);
}