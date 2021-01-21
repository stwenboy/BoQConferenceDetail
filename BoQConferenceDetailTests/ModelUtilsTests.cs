using BoQConferenceDetail;
using NUnit.Framework;
using System;

namespace BoQConferenceDetailTests
{
    public class ModelUtilsTests
    {

        [Test]
        public void TestParsingTimeSlot()
        {
            // given
            var timeslotstring = "22 July 2033 11:40 - 12:40";
            var expectedStart = new DateTime(2033, 7, 22, 11, 40, 0);
            
            // when
            var actualstart = ModelUtils.GetStartTimesfromTimeSlot(timeslotstring);

            // then
            Assert.AreEqual(expectedStart, actualstart);
        }

        [Test]
        public void TestGetTimeSlotString()
        {
            // given 
            var timeslotstring = "22 July 2033 1:40 - 14:40";
            var expectedSlot = "1:40 - 14:40";

            // when
            var actualSlot = ModelUtils.GetTimeSlotString(timeslotstring);

            // then
            Assert.AreEqual(expectedSlot, actualSlot);
        }
    }
}
