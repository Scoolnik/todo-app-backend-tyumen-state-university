using TODOAppBackend.Models;

namespace TODOAppBackend.Services
{
	public interface ITaskService
	{
		void AddTask(int userId, TaskEditRequest task);
		TaskResponseModel[] GetAllByDayAndUser(int userId, DateTime date);
		TaskResponseModel[] GetAllByUser(int userId);
		Dictionary<DateOnly, TaskResponseModel[]> GetAllByWeekAndUser(int userId, DateTime date);
		bool TryRemoveTask(int userId, int taskId);
		bool TryUpdateTask(int userId, int taskId, TaskEditRequest task);
	}
}