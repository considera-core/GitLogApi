using GitLogApi.Entities.Enums;
using GitLogApi.Entities.POCO;

namespace GitLogApi.Entities.DTO;

public struct ProjectDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ProjectType Type { get; set; }

    public ProjectStatus Status { get; set; }

    public BaseProjectData Data { get; set; }

    public string StatusString =>
        Status.ToString().Replace(", ", " | ");

    public string AsDisplay() =>
        $"## {Name} (v0.0.0 => v0.0.0) @ {StatusString}\n{Data.AsDisplay()}";
}