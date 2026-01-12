namespace GitLogApi.Entities.DTO;

public class LogEntryDTO
{
    public required int Index { get; set; }

    public required string Message { get; set; }
}