using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoQConferenceDetail.ConferenceClient
{
    public interface IConferenceClient
    {
        Task<IList<PocoReturnData>> GetSpeakersAsync(string speakerName);

        Task<IList<PocoReturnData>> GetSpeakerSessionsAsync(string speakerId);

        Task<IList<PocoReturnData>> GetSessionTopicsAsync(string sessionId);        
    }
}
