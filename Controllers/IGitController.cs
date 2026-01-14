using Microsoft.AspNetCore.Mvc;

namespace GitLogApi.Controllers;

public interface IGitController
{
    Task<IActionResult> GetLogsFromBranch(string app, string repo, string branch);
    Task<IActionResult> GetLogsFromRoot(string app, string repo);
}