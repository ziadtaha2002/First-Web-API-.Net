namespace WebApi.DTO
{
    public class General_Response<T>
    {
        public bool isSuccess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        //public static implicit operator General_Response<T>(General_Response<List<object>> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
