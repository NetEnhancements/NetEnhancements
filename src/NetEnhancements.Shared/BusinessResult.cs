using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetEnhancements.Shared
{
    /// <summary>
    /// A wrapper class for calls into the business layer, so controllers don't have to do their own error handling.
    /// </summary>
    public class BusinessResult
    {
        /// <summary>
        /// Returns a succesful <see cref="BusinessResult"/>.
        /// </summary>
        public static BusinessResult Succeeded { get; } = new BusinessResult();

        /// <summary>
        /// Indicates whether the call was successful.
        /// </summary>
        [MemberNotNullWhen(false, nameof(StatusMessage))]
        public bool Success { get; }

        /// <summary>
        /// Must contain an error message when <see cref="Success"/> is <see langword="false"/>.
        /// </summary>
        public string? StatusMessage { get; }

        /// <summary>
        /// Something (usually an entity) was not found.
        /// </summary>
        public bool NotFound { get; }

        /// <summary>
        /// Returns an error result.
        /// </summary>
        public static BusinessResult Error(string statusMessage = "Unspecified error") => new(otherError: true, statusMessage: statusMessage);

        /// <summary>
        /// Indicates that an unknown error occurred.
        /// </summary>
        public bool OtherError { get; }

        /// <summary>
        /// A failed result with another result's errors.
        /// </summary>
        public BusinessResult(BusinessResult other)
            : this(other.NotFound, other.OtherError, other.StatusMessage)
        {
        }

        public BusinessResult(bool notFound = false, bool otherError = false, string? statusMessage = null)
        {
            // This should be an AND (||) between all current and future error bools. Inline assignment ftw.
            Success = !(
                (NotFound = notFound) ||
                (OtherError = otherError)
            );

            StatusMessage = statusMessage;
        }
    }

    /// <summary>
    /// A wrapper class for calls into the business layer, so controllers don't have to do their own error handling.
    /// 
    /// Contains data when successful.
    /// </summary>
    public class BusinessResult<T> : BusinessResult
    {
        /// <summary>
        /// Indicates whether the call was successful.
        /// When true, <see cref="Data"/> is not null.
        /// </summary>
        [MemberNotNullWhen(true, nameof(Data))]
        public new bool Success { get; }

        /// <summary>
        /// The data of the result. Not null when <see cref="Success"/> is true.
        /// </summary>
        public T? Data { get; }

        /// <summary>
        /// A successful result with data.
        /// </summary>
        /// <param name="data"></param>
        public BusinessResult(T data)
        {
            Data = data;
            Success = true;
        }

        /// <inheritdoc/>
        public BusinessResult(BusinessResult other) : base(other) { }

        /// <summary>
        /// A failed result without data.
        /// </summary>
        public BusinessResult(bool notFound = false, bool otherError = false, string? statusMessage = null)
            : base(notFound, otherError, statusMessage)
        {
        }

        /// <summary>
        /// Returns an error result.
        /// </summary>
        public static BusinessResult<T> FromError(string statusMessage = "Unspecified error") => new(otherError: true, statusMessage: statusMessage);

        /// <summary>
        /// Returns a result indicating something was not found.
        /// </summary>
        /// <returns></returns>
        public static BusinessResult<T> FromNotFound(string? statusMessage = "Not found") => new(notFound: true, statusMessage: statusMessage);

        /// <summary>
        /// Convert a <typeparamref name="T"/> to a successful <see cref="BusinessResult{T}"/>.
        /// </summary>
        public static implicit operator BusinessResult<T>(T instance)
        {
            // TODO: TOther?
            if (instance is BusinessResult<T> other)
            {
                Debugger.Break();
                return other;
            }

            return new BusinessResult<T>(instance);
        }
    }
}
