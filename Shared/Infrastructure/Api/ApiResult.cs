using Shared.Infrastructure;

namespace Domain.Infrastructure.Api
{
	/// <summary>
	/// Types of Api Results
	/// </summary>
	public static class TypeResult
	{
		/// <summary>
		/// When the api results is succefull
		/// </summary>
		public const string SUCCESS = "SUCCESS";

		/// <summary>
		/// When the api results has dubug info
		/// </summary>
		public const string DEBUG = "DEBUG";

		/// <summary>
		/// When the api results has additional info
		/// </summary>
		public const string INFO = "INFO";

		/// <summary>
		/// When the api results has warnings
		/// </summary>
		public const string WARNING = "WARNING";

		/// <summary>
		/// When the api results has errors
		/// </summary>
		public const string ERROR = "ERROR";

		/// <summary>
		/// When the api results is Failed
		/// </summary>
		public const string FAILED = "FAILED";
	}

	/// <summary>
	/// Base Helper Class for Api Results
	/// </summary>
	public class ApiResult : IApiResult
	{
		/// <summary>
		/// <see cref="TypeResult"/>
		/// </summary>
		public string ApiTypeResult { get; set; } = TypeResult.SUCCESS;

		/// <summary>
		/// Additional message informations of the api results
		/// </summary>
		public string Message { get; set; } = default!;
	}
}