using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Runtime.Serialization;

namespace CustoomerToken.Services.Exceptios
{
    public class ValidationError
    {
        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                message = value;
            }
        }

        public ValidationError(string message)
        {
                Message = message;
        }
    }

    public class ValidationException : Exception
    {
        private List<ValidationError> Errors { get; set; } = new List<ValidationError>();

        public ValidationException(string message)
        {
            Errors.Add(new ValidationError(message));
        }

        public ValidationException(ValidationError? error) : base(error.Message)
        {
            Errors.Add(error);
        }

        public ValidationException(List<ValidationError> error) : base(error.FirstOrDefault()?.Message)
        {
            Errors.AddRange(error);
        }
    }

    public class EmployeeValidationException : ValidationException
    {
        public EmployeeValidationException(string message) : base(message)
        {
        }

        public EmployeeValidationException(ValidationError? error) : base(error)
        {
        }

        public EmployeeValidationException(List<ValidationError> error) : base(error)
        {
        }
    }
}
