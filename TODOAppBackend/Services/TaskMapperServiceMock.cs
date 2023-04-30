using TODOAppBackend.Models;

namespace TODOAppBackend.Services;

public class TaskMapperServiceMock : ITaskMapperService
{
	public TaskResponseModel Map(Entities.Task task)
	{
		return new TaskResponseModel()
		{
			id = task.taskId.ToString(),
			taskTitle = task.taskTitle,
			isCompleted = task.isCompleted,
			scheduledTime = task.scheduledTime,
		};
	}

	public Entities.Task Map(TaskEditRequest task)
	{
		return new Entities.Task()
		{
			taskTitle = task.taskTitle,
			isCompleted = task.isCompleted,
			scheduledTime = task.scheduledTime,
		};
	}

	public Entities.Task Map(int userId, TaskEditRequest task)
	{
		return new Entities.Task()
		{
			userId = userId,
			taskTitle = task.taskTitle,
			isCompleted = task.isCompleted,
			scheduledTime = task.scheduledTime,
		};
	}

	//TODO: think about better naming
	public Entities.Task MapCombined(Entities.Task source1, TaskEditRequest source2)
	{
		return new Entities.Task()
		{
			taskId = source1.taskId,
			taskTitle = source2.taskTitle,
			isCompleted = source2.isCompleted,
			scheduledTime = source2.scheduledTime,
			userId = source1.userId,
		};
	}
}
