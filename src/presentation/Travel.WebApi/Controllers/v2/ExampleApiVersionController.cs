using Microsoft.AspNetCore.Mvc;

namespace Travel.WebApi.Controllers.v2;

[ApiVersion("2.0")]
public class ExampleApiVersionController:ApiController
{
    [HttpPost]
    public ActionResult<string> Get() => "Hello, version 2";

}