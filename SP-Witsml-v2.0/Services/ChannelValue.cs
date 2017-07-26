using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SP_Witsml_v2._0.Services
{
    public class ChannelValue
    {
        public int Index { get; set; }
        public string Mnemonic { get; set; }
        public string Description { get; set; }
        public string LastDataValue { get; set; }
        public List<string> DataValues { get; set; }
    }
}