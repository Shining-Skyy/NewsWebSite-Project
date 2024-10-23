namespace Application.Dtos
{
    // Generic class BaseDto that can hold any type of data (T)
    public class BaseDto<T>
    {
        public BaseDto(T data, List<string> message, bool isSuccess)
        {
            this.Data = data;
            this.Message = message;
            this.IsSuccess = isSuccess;
        }

        // Property to hold the data of type T
        public T Data { get; private set; }

        // Property to hold a list of messages (e.g., error messages)
        public List<string> Message { get; private set; }

        // Property to indicate whether the operation was successful
        public bool IsSuccess { get; set; }
    }

    // Non-generic class BaseDto that does not hold any specific data type
    public class BaseDto
    {
        public BaseDto(List<string> message, bool isSuccess)
        {
            this.Message = message;
            this.IsSuccess = isSuccess;
        }

        // Property to hold a list of messages (e.g., error messages)
        public List<string> Message { get; private set; }

        // Property to indicate whether the operation was successful
        public bool IsSuccess { get; private set; }
    }
}
