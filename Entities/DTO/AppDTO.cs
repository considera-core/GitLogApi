namespace GitLogApi.Entities.DTO;

public struct AppDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<ProjectDTO> Projects { get; set; }

    public AppMetadata Metadata =>
        new(this);
}