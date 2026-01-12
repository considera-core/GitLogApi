namespace GitLogApi.Entities.Config;

public abstract class BaseAppConfig
{
    public abstract string RepositoryRoot { get; set; }

    public abstract BaseProjectsConfig Projects { get; set; }
}