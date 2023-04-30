using TODOAppBackend.Models;

namespace TODOAppBackend.Services
{
	public interface ITaskMapperService
	{
		TaskResponseModel Map(Entities.Task task);
		Entities.Task Map(TaskEditRequest task);
		Entities.Task Map(int userId, TaskEditRequest task);
		Entities.Task MapCombined(Entities.Task source1, TaskEditRequest source2);
	}
}