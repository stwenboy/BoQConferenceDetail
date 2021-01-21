using System;
using System.Collections.Generic;
using System.Linq;

namespace BoQConferenceDetail.ConferenceClient
{
    public class PocoReturnData
    {
        public string href { get; set; }
        public IList<Dictionary<string, string>> data { get; set; }        
    }

}