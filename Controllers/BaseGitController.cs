using System.Text;
using System.Text.RegularExpressions;
using GitLogApi.Entities.Config;
using GitLogApi.Entities.DTO;
using GitLogApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GitLogApi.Controllers;

public abstract class BaseGitController : Controller, IGitController
{
    private readonly ILogger _logger;
    private readonly IAppConfig _appConfig;

    public BaseGitController(
        IAppConfig appConfig,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<BaseGitController>();
        _appConfig = appConfig;
    }

    [HttpGet]
    [Route("GetLogsFromBranch/{app}/{repo}/{branch}")]
    [ProducesResponseType(typeof(IEnumerable<LogEntryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetLogsFromBranch(string app, string repo, string branch)
    {
        try
        {
            var config = _appConfig.Projects
                .GetProjectConfig(app)
                .GetApp(repo);
            var repoPath = Path.Join(_appConfig.RepositoryRoot, config.Path);
            return Ok(await GetAllLogs(repoPath, branch));
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("GetLogsFromRoot/{app}/{repo}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<LogEntryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetLogsFromRoot(string app, string repo)
    {
        var rootBranch = _appConfig.Projects.GetProjectConfig(app).GetApp(repo).RootBranch;
        return await GetLogsFromBranch(app, repo, rootBranch);
    }

    public static async Task<IEnumerable<LogEntryDTO>> GetAllLogs(string repoPath, string branch)
    {
        var commandBuilder = new StringBuilder();
        commandBuilder.Append($"PUSHD \"{repoPath}\" && ");
        commandBuilder.Append($"git checkout \"{branch}\" && ");
        commandBuilder.Append("git fetch && ");
        commandBuilder.Append("git pull && ");
        commandBuilder.Append("git log --no-decorate --oneline && ");
        commandBuilder.Append("POPD");
        var command = commandBuilder.ToString();
        var res = (await command.Execute()).Assert(repoPath, branch, command);
        return res.StdOut
            .Split('\n')
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Distinct()
            .Select((x, i) => new LogEntryDTO { Index = i, Message = x });
    }

    public static IEnumerable<LogEntryDTO> FilterLogs(Regex filter, IEnumerable<LogEntryDTO> logs) =>
        logs.Where(l => filter.IsMatch(l.Message)).OrderBy(x => x.Index);
}