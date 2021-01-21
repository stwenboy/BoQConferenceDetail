using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoQConferenceDetail
{
    public interface IConferenceQuery
    {
        Task<List<WireSessionAndTopics>> GetSessionAndTopicSpecialAsync(string speakerName, DateTime sessionDate, string timeSlot);
    }
}
