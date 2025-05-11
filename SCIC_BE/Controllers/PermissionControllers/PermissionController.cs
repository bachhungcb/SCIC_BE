using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.Services;

namespace SCIC_BE.Controllers.PermissionControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController :ControllerBase
    {
        private readonly RcpService _rcpService;


    }
}
