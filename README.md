You are required to implement the base controllers. Make a copy of the `appsettings.projects.json` and configure your projects.
1 App can have Many Repos (1:M)

Example AppConfig:
```csharp
public class AppConfig : IAppConfig
{
    public required string RepositoryRoot { get; set; }
    public required ProjectsConfig Projects { get; set; }
    IProjectsConfig IAppConfig.Projects => Projects;
}
```

Example ProjectsConfig:
```csharp
public class ProjectsConfig : IProjectsConfig
{
    public SomeProjectConfig SomeProject { get; set; }

    public IProjectConfig GetProjectConfig(string projectName) =>
        projectName.ToLower() switch
        {
            "someproject" => SomeProject,
            _ => throw new ArgumentException($"Unknown project name: {projectName}")
        };
}
```

Example ProjectConfig:
```csharp
public class SomeProjectConfig : IProjectConfig
{
    private List<RepoConfig>? _apps;
    public RepoConfig Api { get; set; }

    private List<RepoConfig> Apps => _apps ??=
    [
        Api,
    ];

    public RepoConfig GetApp(string app) =>
        app.ToLower() switch
        {
            "api" => Api, // api is the RepoKey
            _ => throw new ArgumentException($"App '{app}' not found in SomeProjectConfig.")
        };

    public List<RepoConfig> GetApps() => Apps;
}
```