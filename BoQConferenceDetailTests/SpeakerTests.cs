
using BoQConferenceDetail.ConferenceClient;
using BoQConferenceDetail.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace BoQConferenceTest
{
    public class SpeakerTests
    {
        [Test]
        public void TestSpeakerTransformFromPoco()
        {
            // given
            var expectedId = "1";
            var expectedhref = "https://test.com/TestHREF/" + expectedId;
            var expectedName = "Scott Guthrie";
            var poco = new PocoReturnData
            {
                href = expectedhref,
                data = new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>()
                    {
                        { "name", "Name" },
                        { "value", expectedName }
                    }
                }
            };

            // when
            var result = Speaker.SpeakerFromPoco(poco);

            // then
            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(expectedId, result.SpeakerId);
            Assert.AreEqual(expectedName, result.Name);
        }

        [Test]
        public void TestSpeakersTransformFromPoco()
        {
            // given
            var expectedName = "Scott Guthrie";
            var poco1 = new PocoReturnData
            {
                href = "https://test.com/TestHREF/1",
                data = new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>()
                    {
                        { "name", "Name" },
                        { "value", expectedName }
                    }
                }
            };
            var poco2 = new PocoReturnData
            {
                href = "https://test.com/TestHREF/2",
                data = new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>()
                    {
                        { "name", "Name" },
                        { "value", "John Jones" }
                    }
                }
            };
            var pocos = new List<PocoReturnData>
            {
                poco1,
                poco2
            };

            // when
            var result = Speakers.SpeakersFromPoco(pocos);

            // then
            Assert.IsNotNull(result.Items, "Items is null");
            Assert.AreEqual(2, result.Items.Count);            
        }
    }
}