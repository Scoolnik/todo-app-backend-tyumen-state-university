using TODOAppBackend.Models;

namespace TODOAppBackend.Services;

public class TaskMapperService : ITaskMapperService
{
	public TaskResponseModel Map(Entities.TM_Task task)
	{
		return new TaskResponseModel()
		{
			id = task.ID.ToString(),
			taskTitle = task.TaskTitle,
			isCompleted = task.IsCompleted,
			scheduledTime = task.ScheduledTime,
		};
	}

	public Entities.TM_Task Map(TaskEditRequest task)
	{
		return new Entities.TM_Task()
		{
			TaskTitle = task.taskTitle,
			IsCompleted = task.isCompleted,
			ScheduledTime = task.scheduledTime,
		};
	}

	public Entities.TM_Task Map(int userId, TaskEditRequest task)
	{
		return new Entities.TM_Task()
		{
			UserId = userId,
			TaskTitle = task.taskTitle,
			IsCompleted = task.isCompleted,
			ScheduledTime = task.scheduledTime,
		};
	}

	//TODO: think about better naming
	public Entities.TM_Task MapCombined(Entities.TM_Task source1, TaskEditRequest source2)
	{
		return new Entities.TM_Task()
		{
			ID = source1.ID,
			TaskTitle = source2.taskTitle,
			IsCompleted = source2.isCompleted,
			ScheduledTime = source2.scheduledTime,
			UserId = source1.UserId,
		};
	}
}
