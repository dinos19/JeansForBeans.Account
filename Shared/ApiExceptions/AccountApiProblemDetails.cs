using Microsoft.AspNetCore.Mvc;

namespace Shared.ApiExceptions
{
	/// <summary>
	/// Dto that holds the error details
	/// </summary>
	public class AccountApiProblemDetails : ProblemDetails
	{
		/// <summary>
		/// List containing all errors thrown by an exception
		/// </summary>
		public IReadOnlyCollection<string> Errors { get; }

		/// <summary>
		/// Constructor with the errors list
		/// </summary>
		/// <param name="errors"></param>
		public AccountApiProblemDetails(IReadOnlyCollection<string> errors)
		{
			Errors = errors;
		}
	}
}