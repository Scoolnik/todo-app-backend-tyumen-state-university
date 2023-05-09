using TODOAppBackend.Models;

namespace TODOAppBackend.Services
{
	public interface ITaskMapperService
	{
		TaskResponseModel Map(Entities.TM_Task task);
		Entities.TM_Task Map(TaskEditRequest task);
		Entities.TM_Task Map(int userId, TaskEditRequest task);
		Entities.TM_Task MapCombined(Entities.TM_Task source1, TaskEditRequest source2);
	}
}