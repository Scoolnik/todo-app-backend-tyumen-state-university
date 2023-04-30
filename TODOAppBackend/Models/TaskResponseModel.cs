namespace TODOAppBackend.Models;
public class TaskResponseModel
{
	public string id { get; set; }
	public string taskTitle { get; set; }
	public bool isCompleted { get; set; }
	public DateTime scheduledTime { get; set; }
}
