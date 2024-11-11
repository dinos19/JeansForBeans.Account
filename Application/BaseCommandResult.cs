using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
	public class BaseCommandResult
	{
		/// <summary>
		/// Boolean parameter to determine if a result is successful
		/// </summary>
		public bool IsSuccessful { get; }

		/// <summary>
		/// List of errors
		/// </summary>
		public List<string> Errors { get; } = new List<string>();

		/// <summary>
		/// Data PayloadRequest
		/// </summary>
		public object? Payload { get; }

		/// <summary>
		/// Class constructor
		/// </summary>
		public BaseCommandResult()
		{ }

		/// <summary>
		/// Returns if a result is successful and the errors if any exist
		/// </summary>
		/// <param name="successful"></param>
		/// <param name="errors"></param>
		private BaseCommandResult(bool successful, List<string>? errors)
			: this(successful, errors, null!) { }

		/// <summary>
		/// Returns if a result is successful and the errors if any exist and payloadRequest
		/// </summary>
		/// <param name="successful"></param>
		/// <param name="errors"></param>
		/// <param name="payload"></param>
		private BaseCommandResult(bool successful, List<string>? errors, object? payload)
		{
			IsSuccessful = successful;
			Payload = payload;

			if (errors != null && errors.Any())
				Errors.AddRange(errors);
		}

		/// <summary>
		/// Returns the BaseCommandResult with a list of errors
		/// </summary>
		/// <param name="message"></param>
		/// <returns><see cref="BaseCommandResult"/></returns>
		public static BaseCommandResult WithError(string message)
		{
			var errors = new List<string> { message };

			return new BaseCommandResult(false, errors);
		}

		/// <summary>
		/// Returns the BaseCommandResult with a list of errors
		/// </summary>
		/// <param name="messages"></param>
		/// <returns><see cref="BaseCommandResult"/></returns>
		public static BaseCommandResult WithErrors(List<string> messages)
			=> new BaseCommandResult(false, messages);

		/// <summary>
		/// Returns the successful BaseCommandResult
		/// </summary>
		/// <returns><see cref="BaseCommandResult"/></returns>
		public static BaseCommandResult Success()
			=> new BaseCommandResult(true, null);

		/// <summary>
		/// Returns the successful BaseCommandResult with payloadRequest
		/// </summary>
		/// <returns><see cref="BaseCommandResult"/></returns>
		public static BaseCommandResult SuccessWithPayload<T>(T payload)
			=> new BaseCommandResult(true, null, payload!);
	}

}
