using Microsoft.Extensions.Options;
using TODOAppBackend.Controllers;
using TODOAppBackend.Services;

namespace TODOAppBackend.Tests
{
	public class Tests
	{
		private IJWTService _jwtService;
		private IUserService _userService;
		private ILoginService _loginService;
		private ITaskMapperService _taskMapperService;
		private ITaskService _taskService;
		private AuthController _loginController;
		private DataController _dataController;

		private Random _rand = new Random();

		[SetUp]
		public void Setup()
		{
			var appSettings = new AppAuthSettings()
			{
				Secret = "TempSuperSecretStringBlahBlah",
				TokenLifetimeValue = "1:0:0:0"
			};
			_jwtService = new JWTService(new OptionsWrapper<AppAuthSettings>(appSettings));
			_userService = new UserServiceMock();
			_loginService = new LoginService(_userService, _jwtService);
			_taskMapperService = new TaskMapperServiceMock();
			_taskService = new TaskServiceMock(_taskMapperService);
			_loginController = new AuthController(_loginService);
			_dataController = new DataController(_taskService, _jwtService);
		}

		[Test]
		public void TestJwtService()
		{
			var userId = 10843783;
			var token = _jwtService.CreateToken(userId);
			var tokenParseResult = _jwtService.TryGetUserId(token, out int tokenUserId);
			if (!tokenParseResult)
			{
				Assert.Fail();
				return;
			}
			Assert.AreEqual(userId, tokenUserId);
		}

		[Test]
		public void TestRequestMapping()
		{
			var date = DateTime.UtcNow.AddMinutes(1);
			var taskRequest = new Models.TaskEditRequest() { isCompleted = false, scheduledTime = date, taskTitle = "TaskTitke" };
			var mappedTask = _taskMapperService.Map(taskRequest);

			Assert.That(() =>
			{
				return taskRequest.taskTitle == mappedTask.taskTitle
				&& taskRequest.isCompleted == mappedTask.isCompleted
				&& taskRequest.scheduledTime == mappedTask.scheduledTime;
			});
		}

		[Test]
		public void TestTaskCreate()
		{
			var userId = _rand.Next();
			var authorisation = GetAuthHeader(userId);
			for (var i = 0; i < 10; i++)
			{
				var date = DateTime.UtcNow.Date.AddMinutes(i);
				var taskRequest = new Models.TaskEditRequest() { isCompleted = false, scheduledTime = date, taskTitle = "TaskTitke" };
				_dataController.CreateTask(userId, taskRequest);
				var tasks = _dataController.GetAllByDay(userId, date.ToShortDateString());
				Assert.AreEqual(i + 1, tasks.tasks.Length);
				var task = tasks.tasks.First(x => x.scheduledTime == date);

				Assert.That(() =>
				{
					return taskRequest.taskTitle == task.taskTitle
						&& taskRequest.isCompleted == task.isCompleted
						&& taskRequest.scheduledTime == task.scheduledTime;
				}, "Created task data not equals to request data");
			}
		}

		[Test]
		public void TestTaskRemove()
		{
			var userId = _rand.Next();
			var authorisation = GetAuthHeader(userId);
			for (var i = 0; i < 10; i++)
			{
				var date = DateTime.UtcNow.Date.AddMinutes(i);
				var taskRequest = new Models.TaskEditRequest() { isCompleted = false, scheduledTime = date, taskTitle = "TaskTitke" };
				_dataController.CreateTask(userId, taskRequest);
				var tasks = _dataController.GetAllByDay(userId, date.ToShortDateString());
				Assert.AreEqual(1, tasks.tasks.Length);
				var task = tasks.tasks.First(x => x.scheduledTime == date);
				var removeResult = _dataController.RemoveTask(authorisation, int.Parse(task.id));
				Assert.That(removeResult, Is.Not.Null);
				Assert.AreEqual("success", removeResult.result);
				Assert.AreEqual(0, _dataController.GetAllByDay(userId, date.ToShortDateString()).tasks.Length);
			}
		}

		[Test]
		public void TestTaskUpdate()
		{
			var userId = _rand.Next();
			var authorisation = GetAuthHeader(userId);
			var date = DateTime.UtcNow.Date;
			var title = "TaskTitke";
			var taskRequest = new Models.TaskEditRequest() { isCompleted = false, scheduledTime = date, taskTitle = title };
			_dataController.CreateTask(userId, taskRequest);
			for (var i = 0; i < 10; i++)
			{
				var tasks = _dataController.GetAll(userId);
				Assert.AreEqual(tasks.tasks.Length, 1);
				var task = tasks.tasks.First(x => x.scheduledTime == date);
				Assert.That(task, Is.Not.Null);
				Assert.AreEqual(title, task.taskTitle);
				
				title += i;

				var updateResult = _dataController.UpdateTask(
					authorisation, 
					int.Parse(task.id), 
					new Models.TaskEditRequest() { taskTitle = title, isCompleted = false, scheduledTime = date }
				);
				Assert.AreEqual("success", updateResult.result);
			}
		}

		private string GetAuthHeader(int userId)
		{
			return _jwtService.CreateToken(userId);
		}
	}
}
