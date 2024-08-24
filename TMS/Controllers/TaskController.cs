using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using TMS.Data;
using TMS.Data.ApplicationDbContext;
using TMS.Models;

namespace TMS.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger,ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            var task = _applicationDbContext.TaskDetails.Select(t =>new TaskModel
            { 
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                DueDate = t.DueDate,
                Status=t.Status.ToString(),
            
            }).ToList();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskModel task)
        {
            try
            {
               
                    var taskData = new TaskDetails
                    {
                        Id = Guid.NewGuid(),
                        Name = task.Name,
                        Description = task.Description,
                        DueDate = task.DueDate,
                        Status = Enum.TryParse<TaskActiveStatus>(task.Status, out var status) ? status : TaskActiveStatus.Incomplete,
                    };
                    _applicationDbContext.TaskDetails.Add(taskData);
                    await _applicationDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                
                

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public IActionResult GetTask(Guid id)
        {
            try
            {
                var task = _applicationDbContext.TaskDetails.FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    return NotFound();
                }
                return Json(task);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        [HttpPut]
        public IActionResult EditTask(TaskModel model)
        {
            try
            {
                var task = _applicationDbContext.TaskDetails.FirstOrDefault(t => t.Id == model.Id);
                if (task != null)
                {
                    task.Name = model.Name;
                    task.Description = model.Description;
                    task.DueDate = model.DueDate;
                    task.Status = Enum.TryParse<TaskActiveStatus>(model.Status, out var status) ? status : TaskActiveStatus.Incomplete;
                    _applicationDbContext.SaveChanges();
                }
                return RedirectToAction("Index");

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpDelete]
        public IActionResult DeleteTask(Guid id)
        {
            try
            {
               
                var task = _applicationDbContext.TaskDetails.FirstOrDefault(t => t.Id == id);

                if (task == null)
                {
                    return NotFound(new { message = "Task not found" });
                }

                _applicationDbContext.TaskDetails.Remove(task);
                _applicationDbContext.SaveChanges();

                return Ok();

            }
            catch (Exception)
            {

                throw;
            }
           
        }



    }
}
