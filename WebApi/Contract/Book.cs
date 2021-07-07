using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
    [DynamoDBTable("Book")]
    public class Book
    {
        [DynamoDBProperty("Id")]
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        [DynamoDBProperty("ISBN")]
        public string ISBN { get; set; }

        [DynamoDBProperty("Description")]
        public string Description { get; set; }

        [DynamoDBProperty("Name")]
        public string Name { get; set; }
    }
}