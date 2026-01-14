namespace GitLogApi.Entities.Config;

public class BranchOverridesConfig
{
    public string? Dev { get; set; }

    public string? Ci { get; set; }

    public string? Qa { get; set; }

    public string? Prod { get; set; }
}