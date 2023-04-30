using System.ComponentModel.DataAnnotations;

namespace TODOAppBackend.Models;
public class TaskEditRequest
{
	[Required]
	public string taskTitle { get; set; }
	[Required]
	public bool isCompleted { get; set; }
	[Required]
	public DateTime scheduledTime { get; set; }
}
