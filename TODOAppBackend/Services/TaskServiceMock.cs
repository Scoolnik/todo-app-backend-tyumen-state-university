using System.Globalization;
using TODOAppBackend.Models;
using Task = TODOAppBackend.Entities.Task;

namespace TODOAppBackend.Services;

public class TaskServiceMock : ITaskService
{
	private List<Task> _tasks = new()
	{
		new Task() { taskId = 1, scheduledTime = DateTime.Now.AddDays(1), taskTitle = "Title1", userId = 1, isCompleted = true, },
		new Task() { taskId = 2, scheduledTime = DateTime.Now.AddDays(1).AddHours(1), taskTitle = "Title2", userId = 1, isCompleted = false, },
		new Task() { taskId = 3, scheduledTime = DateTime.Now.AddDays(1).AddHours(2), taskTitle = "Title3", userId = 1, isCompleted = false, },
		new Task() { taskId = 4, scheduledTime = DateTime.Now.AddDays(1).AddHours(3), taskTitle = "Title4", userId = 2, isCompleted = false, },
		new Task() { taskId = 5, scheduledTime = DateTime.Now.AddDays(1).AddHours(4), taskTitle = "Title5", userId = 1, isCompleted = false, },
	};

	private ITaskMapperService _taskMapperService;

	public TaskServiceMock(ITaskMapperService taskMapperService)
	{
		_taskMapperService = taskMapperService;
	}

	public TaskResponseModel[] GetAllByUser(int userId)
	{
		return _tasks
			.Where(x => x.userId == userId)
			.Select(_taskMapperService.Map)
			.ToArray();
	}

	public TaskResponseModel[] GetAllByDayAndUser(int userId, DateTime date)
	{
		return _tasks
			.Where(x => x.userId == userId && x.scheduledTime.Date == date.Date)
			.Select(_taskMapperService.Map)
			.ToArray();
	}

	public Dictionary<DateOnly, TaskResponseModel[]> GetAllByWeekAndUser(int userId, DateTime date)
	{
		return _tasks
			.Where(x => x.userId == userId && DatesAreInTheSameWeek(x.scheduledTime, date.Date))
			.Select(_taskMapperService.Map)
			.GroupBy(x => x.scheduledTime.Day)
			.ToDictionary(x => DateOnly.FromDateTime(x.First().scheduledTime), x => x.ToArray());
	}

	public void AddTask(int userId, TaskEditRequest task)
	{
		var id = _tasks.Last().taskId + 1;
		var taskInDb = _taskMapperService.Map(userId, task);
		taskInDb.taskId = id; // should be assigned by the db
		_tasks.Add(taskInDb);
	}

	public bool TryUpdateTask(int userId, int taskId, TaskEditRequest task)
	{
		var taskInDb = _tasks.FirstOrDefault(x => x.taskId == taskId && x.userId == userId);
		if (taskInDb == null)
		{
			return false;
		}
		var newTaskInDb = _taskMapperService.MapCombined(taskInDb, task);
		_tasks.Remove(taskInDb);
		_tasks.Add(newTaskInDb);
		return true;
	}

	public bool TryRemoveTask(int userId, int taskId)
	{
		var taskInDb = _tasks.FirstOrDefault(x => x.taskId == taskId && x.userId == userId);
		if (taskInDb == null)
		{
			return false;
		}
		_tasks.Remove(taskInDb);
		return true;
	}

	//stolen from https://www.iditect.com/faq/csharp/check-if-a-datetime-is-in-same-week-as-other-datetime-in-c.html XD
	private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
	{
		var calendar = DateTimeFormatInfo.CurrentInfo.Calendar;
		return date1.Year == date2.Year &&
		   calendar.GetWeekOfYear(date1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) ==
		   calendar.GetWeekOfYear(date2, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
	}
}

