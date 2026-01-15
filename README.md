# GitLogAPI
Zach Champeau - Considera Core LLC

## Overview
GitLogAPI is a .NET 9 Web API designed to retrieve and manage Git commit logs across multiple repositories within various projects. This is intended to be used locally, because it requires running git commands in the terminal. You have the option to implement sempaphores for extra safety, but git already has locking mechanisms in place.

You are required to implement the base controllers. Make a copy of the `appsettings.projects.json` and configure your projects.
1 App can have Many Repos (1:M)

## Configuration
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
            "someproject" => SomeProject, // someproject is the Key
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
        Api
    ];

    public RepoConfig GetApp(string app) =>
        app.ToLower() switch
        {
            "api" => Api, // api is the Key in RepoConfig
            _ => throw new ArgumentException($"App '{app}' not found in SomeProjectConfig.")
        };

    public List<RepoConfig> GetApps() => Apps;
}
```

## Controllers
You can add onto the existing base controllers, but you will need a class to implement it (for your routing).

Example Implementation:
```csharp
using Microsoft.Extensions.Options;
using GitLogApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatchReport.Api.Entities.Config;

namespace PatchReport.Api.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
public class GitController : BaseGitController
{
    public GitController(
        IOptionsSnapshot<AppConfig> appConfig,
        ILoggerFactory loggerFactory)
    : base(appConfig.Value, loggerFactory) {}

    // override is purely optional. the route and response types already exist in the base controller.
    public override async Task<IActionResult> GetLogsFromBranch(string app, string repo, string branch)
    {
        Console.WriteLine("Jump up and get down!");
        return await base.GetLogsFromBranch(app, repo, branch);
    }
}
```

## Semaphores
I use semaphores for generating reports, to prevent conflicting git commands.
Here's an example:
```csharp
private static readonly SemaphoreSlim _semaphore = new(1, 1);

[HttpGet]
[Route("GetReport")]
[ProducesResponseType(typeof(ReportDTO), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public async Task<IActionResult> GetReport()
{
    // Test this in postman with 2 tabs, it's pretty neat!
    if (!await _semaphore.WaitAsync(0))
        return StatusCode(StatusCodes.Status409Conflict, "Another report is already running.");

    try
    {
        var report = await _service.GetReport();
        return Ok(report);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        _logger.LogError(ex, "Error in GetReport");
        return NotFound("Not found");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error in GetReport");
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
    finally
    {
        _semaphore.Release();
    }
}
```

## Revisions
### 1.0.6
- Added semaphores example to README.
- Added Disabled property to RepoConfig.
- Added BasePath and Disabled to IProjectConfig.
### 1.0.5
- Added a key string to IProjectsConfig. Use this when implementing GetProjectConfig(). I will improve this in future versions.
## Notes
- I need to change some of the lingo regarding the AppDTO and ProjectDTO. Technically it should be ProjectDTO and RepoDTO respectively. I will add a new cumulative AppDTO when I add more features (such as Jira story formatting).
- It is strongly encouraged you read the source code because running commands are involved, it is important for you to understand how it works. This should be a common practice when using third-party libraries. 
- In some cases, extra git response lines are being included in the GitController. I will fix this in 1.0.7.
- You can find a WIP postman collection in the source repo: https://github.com/considera-core/GitLogApi