namespace Application.Dtos
{
    public class BaseDto<T>
    {
        public BaseDto(T data, List<string> message, bool isSuccess)
        {
            this.Data = data;
            this.Message = message;
            this.IsSuccess = isSuccess;
        }

        public T Data { get; private set; }
        public List<string> Message { get; private set; }
        public bool IsSuccess { get; set; }
    }

    public class BaseDto
    {
        public BaseDto(List<string> message, bool isSuccess)
        {
            this.Message = message;
            this.IsSuccess = isSuccess;
        }

        public List<string> Message { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
