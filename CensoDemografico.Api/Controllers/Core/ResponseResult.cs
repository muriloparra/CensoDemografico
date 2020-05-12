namespace CensoDemografico.Api.Controllers.Core
{
    public class ResponseResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
