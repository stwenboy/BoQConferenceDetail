using BoQConferenceDetail.ConferenceClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoQConferenceDetail.Models
{
    public class Session : BaseModel
    {
        public string SessionId { get; set; }
        public string Title { get; set; }
        public string Slottime { get; set; }
        public string SpeakersName { get; set; }

        public Topics Topics { get; set; }

        public static Session SessionFromPoco(PocoReturnData poco)
        {
            return new Session
            {
                SessionId = new Uri(poco.href).Segments.LastOrDefault(),
                Title = GetPocoValue(poco, "Title"),
                Slottime = GetPocoValue(poco, "Timeslot"),
                SpeakersName = GetPocoValue(poco, "Speaker"),
                Topics = new Topics()
            };
        }    
    }

    public class Sessions
    {
        public List<Session> Items { get; set; }

        public static Sessions SessionsFromPoco(IEnumerable<PocoReturnData> pocoSessions)
        {
            return new Sessions
            {
                Items = pocoSessions.Select(a => Session.SessionFromPoco(a)).ToList()
            };
        }
    }
}
