namespace GitLogApi.Entities.POCO;

public abstract class BaseProjectData
{
    public List<string> Stories { get; set; } = [];

    public List<string> DistinctStories =>
        Stories.Distinct().ToList();

    public List<string> DistinctFormattedStories =>
        Stories.Distinct().Select(x =>
        {
            var s = x.Trim();
            var idx = s.IndexOf("EV-", StringComparison.OrdinalIgnoreCase);
            return idx >= 0 ? s.Substring(idx) : s;
        }).Distinct().ToList();

    public string JQLQuery =>
        $"key in ({string.Join(", ", DistinctFormattedStories)})";

    public virtual string AsDisplay() =>
        Stories.Count != 0
            ? $"- [ ] {string.Join("\n- [ ] ", Stories)}"
            : "";
}