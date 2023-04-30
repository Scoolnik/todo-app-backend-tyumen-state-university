using System.ComponentModel.DataAnnotations;

namespace TODOAppBackend.Models;

public class LoginRequest
{
	[Required]
	public string? login { get; set; }

	[Required]
	public string? password { get; set; }
}
