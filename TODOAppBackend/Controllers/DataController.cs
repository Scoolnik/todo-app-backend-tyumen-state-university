using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODOAppBackend.Models;
using TODOAppBackend.Services;

namespace TODOAppBackend.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class DataController : ControllerBase
	{
		private readonly ITaskService _taskService;
		private readonly IJWTService _jwtService;

		public DataController(ITaskService taskService, IJWTService jwtService)
		{
			_taskService = taskService;
			_jwtService = jwtService;
		}

		[HttpGet("allTasks/{userId}")]
		public TasksListResponse GetAll(int userId)
		{
			var tasks = _taskService.GetAllByUser(userId);
			return new TasksListResponse() { tasks = tasks };
		}


		[HttpGet("allTasksByDay/{userId}/{dateString}")]
		public TasksListResponse GetAllByDay(int userId, string dateString)
		{
			var date = ParseDate(dateString);
			var tasks = _taskService.GetAllByDayAndUser(userId, date);
			return new TasksListResponse() { tasks = tasks };
		}

		[HttpGet("allTasksByWeek/{userId}/{firstDayOfTheWeekString}")]
		public Dictionary<DateOnly, TaskResponseModel[]> GetAllByWeek(int userId, string firstDayOfTheWeekString)
		{
			var firstDayOfTheWeek = ParseDate(firstDayOfTheWeekString);
			return _taskService.GetAllByWeekAndUser(userId, firstDayOfTheWeek); //TODO: add missed days
		}


		[HttpPost("newTask/{userId}")]
		public OperationResultResponse CreateTask(int userId, [FromBody] TaskEditRequest request)
		{
			_taskService.AddTask(userId, request);
			return OperationResultResponse.Success();
		}

		[HttpPut("updateTask/{userId}/{taskId}")]
		public OperationResultResponse UpdateTask([FromHeader] string authorization, int taskId, [FromBody] TaskEditRequest request)
		{
			if (!_jwtService.TryGetUserId(authorization, out int userId))
			{
				return OperationResultResponse.Fail();
			}
			var result = _taskService.TryUpdateTask(userId, taskId, request);
			return result ? OperationResultResponse.Success() : OperationResultResponse.Fail();
		}

		[HttpDelete("deleteTask/{userId}/{taskId}")]
		public OperationResultResponse RemoveTask([FromHeader] string authorization, int taskId)
		{
			if (!_jwtService.TryGetUserId(authorization, out int userId))
			{
				return OperationResultResponse.Fail();
			}
			var result = _taskService.TryRemoveTask(userId, taskId);
			return result ? OperationResultResponse.Success() : OperationResultResponse.Fail();
		}

		private DateTime ParseDate(string date)
		{
			return DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture);
		}
	}
}
