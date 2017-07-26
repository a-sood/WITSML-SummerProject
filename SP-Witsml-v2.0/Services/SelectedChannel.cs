using SP_Witsml_v2._0.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SP_Witsml_v2._0.Models
{
    public class SelectedChannel
    {
        public List<string> IndexValues { get; set; }
        public List<ChannelValue> Data { get; set; }
    }
}