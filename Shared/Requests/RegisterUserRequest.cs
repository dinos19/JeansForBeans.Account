using Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Requests
{
	/// <summary>
	/// request for Authenticate Agent
	/// </summary>
	public class RegisterUserRequest : IApiRequest
	{
		/// <summary>
		/// Username
		/// </summary>
		public string Username { get; set; } = default!;

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; } = default!;

		/// <summary>
		/// Email
		/// </summary>
		public string Email { get; set; } = default!;
	}
}
