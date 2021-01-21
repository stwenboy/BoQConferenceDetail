using BoQConferenceDetail.ConferenceClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoQConferenceDetail.Models
{
    public class Topic : BaseModel
    {
        public string TopicId { get; set; }
        public string Title { get; set; }

        public static Topic TopicFromPoco(PocoReturnData poco)
        {
            return new Topic
            {
                TopicId = new Uri(poco.href).Segments.LastOrDefault(),
                Title = GetPocoValue(poco, "Title")
            };
        }
    }

    public class Topics
    {
        public List<Topic> Items { get; set; }

        public static Topics TopicsFromPoco(IEnumerable<PocoReturnData> pocoSessions)
        {
            return new Topics
            {
                Items = pocoSessions.Select(a => Topic.TopicFromPoco(a)).ToList()
            };
        }
    }
}