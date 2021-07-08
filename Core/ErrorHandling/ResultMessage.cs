using System.Text.Json.Serialization;

namespace Core.ErrorHandling
{
    /// <summary>
    /// Data class that enables sharing result, status and detailed message of some method 
    /// </summary>
    /// <typeparam name="T">Type of result</typeparam>
    public class ResultMessage<T>
    {
        /// <summary>
        /// Result operation returns
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Status of operation
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OperationStatus Status { get; set; }

        /// <summary>
        /// Check for operation success
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Detailed message of operation success
        /// </summary>
        public string Message { get; set; }

        public ResultMessage(T result, OperationStatus status, string message) : this(result, status)
        {
            Result = result;
            Message = message;
        }

        public ResultMessage(T result, OperationStatus status) : this(status)
        {
            Result = result;
        }

        public ResultMessage((OperationStatus status, string detailedMessage) errorInformation) : this(errorInformation.status, errorInformation.detailedMessage)
        {

        }

        public ResultMessage(OperationStatus status, string detailedMessage) : this(status)
        {
            Message = detailedMessage;
        }

        public ResultMessage(T result) : this(result, OperationStatus.Success) { }

        public ResultMessage(OperationStatus status)
        {
            Status = status;
            IsSuccess = Status == OperationStatus.Success;
        }

        /// <summary>
        /// Creates successful ResultMessage object with no Result object
        /// </summary>
        public ResultMessage() => (IsSuccess, Status) = (true, OperationStatus.Success);

        public static implicit operator T(ResultMessage<T> resultMessage) => resultMessage.Result;

        public ResultMessage<T> CloneWithDetails(string details) => new ResultMessage<T>(Result, Status, details);
    }
}
