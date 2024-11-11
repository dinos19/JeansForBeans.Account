using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiExceptions
{
	/// <summary>
	/// Exception thrown from the API layer
	/// </summary>
	public class AccountApiException : Exception
	{
		/// <summary>
		/// List containing all errors thrown up to the catch point
		/// </summary>
		public List<string> Errors { get; set; } = new List<string>();

		/// <summary>
		/// Constructor with message
		/// </summary>
		/// <param name="message">Message to be displayed with the exception</param>
		public AccountApiException(string message) : base(message) { }

		/// <summary>
		/// Constructor with message and inner exception
		/// </summary>
		/// <param name="message">Message to be displayed with the exception</param>
		/// <param name="inner">Exception that caused this exception to be thrown</param>
		public AccountApiException(string message, Exception inner) : base(message, inner) { }

		/// <summary>
		/// Constructor with message and error list
		/// </summary>
		/// <param name="message">Message to be displayed with the exception</param>
		/// <param name="errors">List of errors coming from the Application Layer</param>
		public AccountApiException(string message, [DisallowNull] List<string> errors) : base(message) { Errors = errors; }

		/// <summary>
		/// Constructor with message, inner exception and error list
		/// </summary>
		/// <param name="message">Message to be displayed with the exception</param>
		/// <param name="inner">Exception that caused this exception to be thrown</param>
		/// <param name="errors">List of errors coming from the Application Layer</param>
		public AccountApiException(string message, Exception inner, [DisallowNull] List<string> errors) : base(message, inner) { Errors = errors; }
	}
}
