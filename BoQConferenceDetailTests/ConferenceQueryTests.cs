using BoQConferenceDetail;
using BoQConferenceDetail.ConferenceClient;
using BoQConferenceDetail.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoQConferenceDetailTests
{
    class ConferenceQueryTests
    {
        [TestCase("George Jones", "5-6", false)]
        [TestCase("George Jones", "", true)]
        [TestCase("", "6-7", true)]
        public void TestCheckRequestParams(string name, string timeslot, bool expected)
        {
            // given
            var mockClient = new Mock<IConferenceClient>();
            var confquery = new ConferenceQuery(mockClient.Object);

            // when
            var actualResult = confquery.InvalidRequestParams(name, new DateTime(), timeslot);

            // then
            Assert.AreEqual(expected, actualResult);

        }


        [Test]
        public void TestMakeWireResponse()
        {
            // given
            var mockClient = new Mock<IConferenceClient>();
            var confquery = new ConferenceQuery(mockClient.Object);
            var expectedSlotTime = "555";
            var expectedSpeaker = "Jeremy";
            var expectedTitle = "The Are of Speling";
            var expectedTopicTitle = "Topic 111";

            var sessions = new Sessions
            {
                Items = new List<Session>
                {
                    new Session
                    {
                        SessionId = "555",
                        Slottime = expectedSlotTime,
                        SpeakersName = expectedSpeaker,
                        Title = expectedTitle,
                        Topics = new Topics
                        {
                            Items = new List<Topic>
                            {
                                new Topic
                                {
                                    Title = expectedTopicTitle,
                                    TopicId = "556"
                                }
                            }
                        }
                    }
                }
            };

            // when
            var actual = confquery.MakeWireResponse(sessions);

            // then
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedTitle, actual[0].Title);
            Assert.AreEqual(expectedSpeaker, actual[0].Speaker);
            Assert.AreEqual(expectedTopicTitle, actual[0].Topics[0]);
        }

        [Test]
        public async Task TestNoException_AllGetSessionAndTopicSpecialAsync()
        {
            // large test shouldn't be done like this,  
            // I should test each method that makes up this bigger 
            // I've spent enough time so this is showing async testing + mocking

            // given
            var mockClient = new Mock<IConferenceClient>();
            var confquery = new ConferenceQuery(mockClient.Object);

            var expectedSpeaker = "Kit";

            var speakerReturn = new PocoReturnData
            {
                href = "https://test.com/value/1",
                data = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "name", "Name" },
                        { "value", expectedSpeaker}
                    }
                }
            };
            var speakersReturn = new List<PocoReturnData> { speakerReturn };

            mockClient
                .Setup(x => x.GetSpeakersAsync(It.IsAny<string>()))
                .ReturnsAsync(speakersReturn);

            var sessionReturn = new PocoReturnData
            {
                href = "https://test.com/value/11",
                data = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "name", "Title" },
                        { "value", "Knight Rider" }
                    },
                    new Dictionary<string, string>
                    {
                        { "name", "Timeslot" },
                        { "value", "04 December 2013 11:40 - 12:40"}
                    },
                    new Dictionary<string, string>
                    {
                        { "name", "Speaker" },
                        { "value", expectedSpeaker }
                    }
                }
            };
            var sessionsReturn = new List<PocoReturnData> { sessionReturn };
            mockClient
                .Setup(x => x.GetSpeakerSessionsAsync(It.IsAny<string>()))
                .ReturnsAsync(sessionsReturn);

            var topicsReturn = new List<PocoReturnData>();
            mockClient
                .Setup(x => x.GetSessionTopicsAsync(It.IsAny<string>()))
                .ReturnsAsync(topicsReturn);


            // when -> then
            Assert.DoesNotThrowAsync(async () => await confquery.GetSessionAndTopicSpecialAsync(expectedSpeaker, new DateTime(), "5-6"));
        }
    }
}
