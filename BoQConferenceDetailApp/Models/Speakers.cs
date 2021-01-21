using BoQConferenceDetail.ConferenceClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoQConferenceDetail.Models
{
    public class Speaker : BaseModel
    {
        public string SpeakerId { get; set; }
        public string Name { get; set; }

        public static Speaker SpeakerFromPoco(PocoReturnData pocoSpeaker)
        {
            return new Speaker
            {
                SpeakerId = new Uri(pocoSpeaker.href).Segments.LastOrDefault(),
                Name = GetPocoValue(pocoSpeaker, "Name")
            };
        }
    }

    public class Speakers
    {
        public List<Speaker> Items { get; set; }

        public static Speakers SpeakersFromPoco(IEnumerable<PocoReturnData> speakers)
        {
            return new Speakers
            {
                Items = speakers.Select(a => Speaker.SpeakerFromPoco(a)).ToList()
            };
        }

        public void FilterByName(string speakerName)
        {
            Items = Items.Where(x => x.Name.Equals(speakerName, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

}
