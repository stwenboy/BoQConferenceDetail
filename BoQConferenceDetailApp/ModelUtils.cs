using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoQConferenceDetail
{
    public static class ModelUtils
    {

        public static DateTime GetStartTimesfromTimeSlot(string timeSlot)
        {
            // assumption is the time slot is in the format -> "<day> <month> <year> <starttime> - <endtime>"

            DateTime.TryParse(timeSlot.Substring(0, timeSlot.IndexOf("-") - 1), out var startTime);

            return startTime;

            // I agree this is horrible!
        }

        public static string GetTimeSlotString(string slottime)
        {
            // assumption is the time slot is in the format -> "<day> <month> <year> <starttime> - <endtime>"
            return String.Join(" ", slottime.Split(' ').Skip(3));
        }
    }
}
