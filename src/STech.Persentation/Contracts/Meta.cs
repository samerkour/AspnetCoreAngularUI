namespace STech.Presentation.Api.Contracts
{
    public class Meta
    {
        public int statusCode { get; set; }
        public bool isError { get; set; }

        public ResponseException ResponseException { get; set; }
    }



    public class ResponseException
    {
        public string message { get; set; }
        public byte code { get; set; }
    }
}
