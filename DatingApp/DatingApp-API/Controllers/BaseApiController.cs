using DatingApp_API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp_API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}
