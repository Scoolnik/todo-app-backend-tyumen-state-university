namespace TODOAppBackend.Models;
public class OperationResultResponse
{
	public string result { get; private set; }

	public static OperationResultResponse Fail() => new() { result = "fail" };

	public static OperationResultResponse Success() => new() { result = "success"};
}

