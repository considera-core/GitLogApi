namespace GitLogApi.Entities.POCO;

public class LambdaProjectData : BaseProjectData
{
    public override string AsDisplay() =>
        $"- [ ] Sam deploy\n\t- [ ] DEV\n\t- [ ] CI\n\t- [ ] QA\n\t- [ ] PROD\n{base.AsDisplay()}";
}