using Microsoft.AspNetCore.Mvc;

namespace GitLogApi.Controllers;

public interface IAppConfigController
{
    IActionResult Get();
}