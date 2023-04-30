namespace TODOAppBackend.Entities
{
	public class Task
	{
		public int taskId { get; set; }
		public string taskTitle { get; set; }
		public bool isCompleted { get; set; }
		public DateTime scheduledTime { get; set; }
		public int userId { get; set; }
	}
}
