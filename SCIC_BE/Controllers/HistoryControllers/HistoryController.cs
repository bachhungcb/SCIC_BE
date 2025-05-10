using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SCIC_BE.Controllers.HistoryControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController:ControllerBase
    {

    }
}
