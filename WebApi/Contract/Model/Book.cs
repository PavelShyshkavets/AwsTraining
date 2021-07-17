using Amazon.DynamoDBv2.DataModel;
using System;

namespace Contract.Model
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