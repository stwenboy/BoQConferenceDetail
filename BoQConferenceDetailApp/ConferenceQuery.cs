using BoQConferenceDetail.ConferenceClient;
using BoQConferenceDetail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace BoQConferenceDetail
{
    public class ConferenceQuery : IConferenceQuery
    {
        private readonly IConferenceClient _confClient;
        public ConferenceQuery(IConferenceClient confClient)
        {
            _confClient = confClient;
        }


        public async Task<List<WireSessionAndTopics>> GetSessionAndTopicSpecialAsync(string speakerName, DateTime sessionDate, string timeSlot)
        {
            // outline to get Sessions and Topics
            // check passed in data
            // Get the speaker or speakers
            // Get the Sessions for the Speaker(s)
            // Filter Sessions by Date and Time slot
            // Get Topics for the Sessions

            if (InvalidRequestParams(speakerName, sessionDate, timeSlot))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            var speaker = await GetSpeakerAsync(speakerName);

            if (speaker is null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            var speakerSessions = await GetSpeakerSessionsAsync(speaker, sessionDate, timeSlot);

            await FillSessionTopics(speakerSessions);

            return MakeWireResponse(speakerSessions);
        }

        public bool InvalidRequestParams(string speakerName, DateTime sessionDate, string timeSlot)
        {
            // do some general sanity checks on data input before going to APIs- should be expanded
            return (String.IsNullOrEmpty(speakerName) || String.IsNullOrEmpty(timeSlot));
        }


        internal List<WireSessionAndTopics> MakeWireResponse(Sessions speakerSessions)
        {
            return
                speakerSessions.Items.Select(x => new WireSessionAndTopics
                {
                    Title = x.Title,
                    TimeSlot = x.Slottime,
                    Speaker = x.SpeakersName,
                    Topics = x.Topics!.Items!.Select(y => y.Title).ToList()
                }).ToList();
        }

        private async Task FillSessionTopics(Sessions speakerSessions)
        {
            foreach (var session in speakerSessions.Items)
            {
                var pocoTopics = await _confClient.GetSessionTopicsAsync(session.SessionId);

                if (pocoTopics is null)
                    return;

                var topics = Topics.TopicsFromPoco(pocoTopics);

                session.Topics = topics;
            }
        }

        private async Task<Sessions> GetSpeakerSessionsAsync(Speaker speaker, DateTime sessionDate, string timeSlot)
        {
            var pocoSession = await _confClient.GetSpeakerSessionsAsync(speaker.SpeakerId);

            var sessions = Sessions.SessionsFromPoco(pocoSession);

            // filter sessions by date and time slot
            sessions.Items.Where(x => ModelUtils.GetStartTimesfromTimeSlot(x.Slottime).Date.Equals(sessionDate.Date)
                                    && (ModelUtils.GetTimeSlotString(x.Slottime) == timeSlot));

            return sessions;
        }

        private async Task<Speaker> GetSpeakerAsync(string speakerName)
        {
            var pocospeakers = await _confClient.GetSpeakersAsync(speakerName);

            var speakers = Speakers.SpeakersFromPoco(pocospeakers);

            // found the API didn't filter by name - so make sure the list is filtered
            speakers.FilterByName(speakerName);

            return speakers.Items.FirstOrDefault(); // take the first in the list
        }
    }
}
