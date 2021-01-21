using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoQConferenceDetail.Controllers
{
    [ApiController]
    [Route("api/ConferenceDetail")]
    public class ConferenceDetailController : ControllerBase
    {

        private readonly ILogger<ConferenceDetailController> _logger;
        private readonly IConferenceQuery _confQuery;

        public ConferenceDetailController(ILogger<ConferenceDetailController> logger, IConferenceQuery confQuery)
        {
            _logger = logger;
            _confQuery = confQuery;
        }


        [HttpGet]
        public async Task<List<WireSessionAndTopics>> Get(string speakerName, DateTime sessionDate, string timeSlot)
        {            
            return await _confQuery.GetSessionAndTopicSpecialAsync(speakerName, sessionDate, timeSlot);
        }

        
    }
}
