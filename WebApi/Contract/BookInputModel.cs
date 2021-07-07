using Contract.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
    public class BookInputModel
    {
        public string ISBN { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public InputType InputType { get; set; }
    }
}
