namespace WebApplicationLR7.Models
{
    public class ResponseModel<T> 
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public ResponseModel(T data, string message)
        {
            Data = data;
            Message = message;
        }
    }
}