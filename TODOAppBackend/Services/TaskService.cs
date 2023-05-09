using System.Globalization;
using TODOAppBackend.Models;
using TODOAppBackend.Repository;
using TM_Task = TODOAppBackend.Entities.TM_Task;

namespace TODOAppBackend.Services;

public class TaskService : ITaskService
{
	private IRepository<TM_Task> _repository;

	private ITaskMapperService _taskMapperService;

	public TaskService(ITaskMapperService taskMapperService, IRepository<TM_Task> repository)
	{
		_taskMapperService = taskMapperService;
		_repository = repository;
	}

	public TaskResponseModel[] GetAllByUser(int userId)
	{
		var tasks = _repository.GetAll();
		return tasks
			.Where(x => x.UserId == userId)
			.Select(_taskMapperService.Map)
			.ToArray();
	}

	public TaskResponseModel[] GetAllByDayAndUser(int userId, DateTime date)
	{
        var tasks = _repository.GetAll();
        return tasks
			.Where(x => x.UserId == userId && x.ScheduledTime.Date == date.Date)
			.Select(_taskMapperService.Map)
			.ToArray();
	}

	public Dictionary<DateOnly, TaskResponseModel[]> GetAllByWeekAndUser(int userId, DateTime date)
	{
        var tasks = _repository.GetAll();
        return tasks
			.Where(x => x.UserId == userId && DatesAreInTheSameWeek(x.ScheduledTime, date.Date))
			.Select(_taskMapperService.Map)
			.GroupBy(x => x.scheduledTime.Day)
			.ToDictionary(x => DateOnly.FromDateTime(x.First().scheduledTime), x => x.ToArray());
	}

	public void AddTask(int userId, TaskEditRequest task)
	{
		var taskInDb = _taskMapperService.Map(userId, task);
        _repository.Create(taskInDb);
	}

	public bool TryUpdateTask(int userId, int taskId, TaskEditRequest task)
	{
        var tasks = _repository.GetAll();
        var taskInDb = tasks.FirstOrDefault(x => x.ID == taskId && x.UserId == userId);
		if (taskInDb == null)
		{
			return false;
		}
		var newTaskInDb = _taskMapperService.MapCombined(taskInDb, task);
		_repository.Update(newTaskInDb);
		return true;
	}

	public bool TryRemoveTask(int userId, int taskId)
	{
        var tasks = _repository.GetAll();
        var taskInDb = tasks.FirstOrDefault(x => x.ID == taskId);
		if (taskInDb == null)
		{
			return false;
		}
		_repository.Delete(taskId);
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

