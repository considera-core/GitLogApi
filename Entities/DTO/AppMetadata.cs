using GitLogApi.Entities.Enums;

namespace GitLogApi.Entities.DTO;

public struct AppMetadata
{
    private readonly AppDTO _app;

    public AppMetadata(AppDTO app) =>
        _app = app;

    public ProjectStatus Status =>
        _app.Projects
            .Select(p => p.Status)
            .Aggregate((a, b) => a | b);

    public string StatusString =>
        Status.ToString().Replace(", ", " | ");

    public string ProjectsString =>
        string.Join("\n", _app.Projects.Select(p => p.AsDisplay()));

    public string AsDisplay() =>
        $"# {_app.Name} @ {StatusString}\n{ProjectsString}";
}