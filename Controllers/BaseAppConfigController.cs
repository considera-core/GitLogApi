using GitLogApi.Entities.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GitLogApi.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
public abstract class BaseAppConfigController : Controller, IAppConfigController
{
    private readonly ILogger _logger;
    private readonly BaseAppConfig _appConfig;

    public BaseAppConfigController(
        IOptionsSnapshot<BaseAppConfig> appConfig,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<BaseAppConfigController>();
        _appConfig = appConfig.Value;
    }

    [HttpGet]
    [ProducesResponseType(typeof(BaseAppConfig), StatusCodes.Status200OK)]
    public IActionResult Get() =>
        Ok(_appConfig);
}

public interface IAppConfigController
{
    IActionResult Get();
}