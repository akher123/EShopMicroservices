namespace BuildingBlocks.Exceptions
{
    public class InvalidServerException:Exception
    {
        public InvalidServerException(string message):base(message)
        {
            
        }

        public InvalidServerException(string mesage, string details):base(mesage)
        {
            this.Details = details;
        }
        public string? Details { get; }
    }
}
