using Domain.Infrastructure.Api;
using Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Results.Account
{
	/// <summary>
	/// This class represents the user registration Response
	/// </summary>
	public class RegisterUserResponse : ApiResult
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
