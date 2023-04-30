namespace TODOAppBackend.Models;
public class LoginResponse
{
	public string? token { get; private set; }

    public string result { get; private set; }

	public static LoginResponse Fail() => new() { result = "fail" };

	public static LoginResponse Success(string token) => new() { result = "success", token = token };
}
