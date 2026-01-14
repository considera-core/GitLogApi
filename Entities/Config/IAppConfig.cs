namespace GitLogApi.Entities.Config;

public interface IAppConfig
{
    string RepositoryRoot { get; set; }

    IProjectsConfig Projects { get; }
}