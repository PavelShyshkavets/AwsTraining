using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
    public class SearchRequest
    {
        public string ISBN { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }
    }
}
