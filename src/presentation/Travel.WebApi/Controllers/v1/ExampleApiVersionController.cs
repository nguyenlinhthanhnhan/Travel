using Microsoft.AspNetCore.Mvc;

namespace Travel.WebApi.Controllers.v1;

[ApiVersion("1.0", Deprecated = true)]
public class ExampleApiVersionController:ApiController
{
    [HttpGet]
    public ActionResult<string> Get() => "Hello, version 1";
}