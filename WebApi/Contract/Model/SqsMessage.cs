namespace Contract.Model
{
    public class SqsMessage
    {
        public Book Book { get; set; }

        public MessageType MessageType { get; set;}
    }
}
