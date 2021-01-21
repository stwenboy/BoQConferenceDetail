
using BoQConferenceDetail.ConferenceClient;
using BoQConferenceDetail.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace BoQConferenceTest
{
    public class BaseModelTests
    {
        [Test]
        public void TestGetPocoValuet()
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
                        {"name", "unexpected" },
                        {"value", "thisisnotthedata" }
                    },
                    new Dictionary<string, string>()
                    {
                        { "name", "Name" },
                        { "value", expectedName }
                    }
                }
            };

            // when
            var actualName = BaseModel.GetPocoValue(poco, "Name");

            // then
            Assert.AreEqual(expectedName, actualName);
        }

    }
}