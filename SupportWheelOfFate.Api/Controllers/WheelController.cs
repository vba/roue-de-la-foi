using System;
using Microsoft.AspNetCore.Mvc;
using SupportWheelOfFate.Core;
using SupportWheelOfFate.Core.Queries;
using SupportWheelOfFate.Core.Repository;

namespace SupportWheelOfFate.Api.Controllers
{
    [Route("api/[controller]")]
    public class WheelController : Controller
    {
        private readonly IWheelUsherRepository _wheelUsherRepository;

        public WheelController(IWheelUsherRepository wheelUsherRepository)
        {
            _wheelUsherRepository = wheelUsherRepository ?? throw new ArgumentNullException(nameof(wheelUsherRepository));
        }

        [HttpGet("{involvedEngineersCount}")]
        public IActionResult Get(int involvedEngineersCount)
        {
            var turnWheelQuery = new TurnWheelQuery(involvedEngineersCount);
            if (!turnWheelQuery.IsValid())
            {
                return BadRequest($"Received engineers count of {involvedEngineersCount} is out of authorized boundaries, Min = {EngineersBoundary.Min.Value}, Max = {EngineersBoundary.Max.Value}");
            }
            return Ok(_wheelUsherRepository.TurnWheel(turnWheelQuery));
        }
    }
}
