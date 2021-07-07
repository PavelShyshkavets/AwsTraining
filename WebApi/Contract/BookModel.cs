using Contract.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
    public class BookModel
    {
        public IEnumerable<Book> Books { get; set; }

        public ResultsType ResultsType { get; set; }

        public string PaginationToken { get; set; }
    }
}
