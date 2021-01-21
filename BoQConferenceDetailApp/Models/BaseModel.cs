using BoQConferenceDetail.ConferenceClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoQConferenceDetail.Models
{
    public class BaseModel
    {
        public static string GetPocoValue(PocoReturnData poco, string valueName)
        {
            var item = poco.data.FirstOrDefault(w => w.ContainsValue(valueName));
            var value = "unknown";
            if (!(item is null))
                value = item.FirstOrDefault(x => x.Key.Equals("value")).Value;

            return value;
        }
    }

}
