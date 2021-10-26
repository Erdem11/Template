namespace Template.Contracts.V1
{
    public class Response<T> //: Response
    {
        public T Data { get; set; }
        public Response() {}

        public Response(T data)
        {
            Data = data;
        }
    }

    public class StringResponse
    {
        public string Data { get; set; }
    }
}