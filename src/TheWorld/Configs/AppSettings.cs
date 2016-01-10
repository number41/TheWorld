using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Configs
{
    public class AppSettings
    {
        public string SiteEmailAddress { get; set; }
        public DBSettings Data { get; set; }
        public string BingKey { get; set; }
    }
}
