namespace TODOAppBackend.Entities
{
	public class TM_User : BaseEntity
	{
		public string UserLogin { get; set; }
		public string UserPassword { get; set; }
		public string? UserToken { get; set; }
	}
}
