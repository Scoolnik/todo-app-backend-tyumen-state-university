namespace TODOAppBackend.Entities
{
	public class TM_Task : BaseEntity
	{
		public string TaskTitle { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime ScheduledTime { get; set; }
		public int UserId { get; set; }
	}
}
