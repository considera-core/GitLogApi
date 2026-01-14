using GitLogApi.Entities.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GitLogApi.Controllers;

public abstract class BaseAppConfigController : Controller, IAppConfigController
{
    private readonly ILogger _logger;

    private readonly IAppConfig _appConfig;

    public BaseAppConfigController(
        IAppConfig appConfig,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<BaseAppConfigController>();
        _appConfig = appConfig;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IAppConfig), StatusCodes.Status200OK)]
    public IActionResult Get() =>
        Ok(_appConfig);
}