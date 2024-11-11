using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiExceptions
{
	/// <summary>
	/// Validation error
	/// </summary>
	public class ValidationError
	{
		/// <summary>
		/// Message
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Errors
		/// </summary>
		public IReadOnlyCollection<string> Errors { get; }

		/// <summary>
		/// Error messages
		/// </summary>
		public string ErrorMessages { get; }

		/// <summary>
		/// Class constructor
		/// </summary>
		public ValidationError(string message, string[] errors)
		{
			Message = message;
			Errors = errors;
			ErrorMessages = string.Join(", ", errors);
		}
	}
}
