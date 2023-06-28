using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zalo_Clone.Models;
using Zalo_Clone.ModelViews;

namespace Zalo_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        IToDoListService _toDoListService;
        IToDoUserService _toDoUserService;
        IMapper _mapper;
        public ToDoListController(IToDoListService toDoListService,IToDoUserService toDoUserService,IMapper mapper) { 
            _mapper = mapper;
            _toDoListService = toDoListService;
            _toDoUserService = toDoUserService;
        }


        [HttpPost("CreateToDoList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateToDoList(ToDoListModel model)
        {
            ToDoList toDoList = _mapper.Map<ToDoList>(model);
            try
            {
                bool result = await _toDoListService.AddToDoList(toDoList,model.UserToDoTask);

                if (result)
                {
                    return Ok("Add todolist chat sussess");
                }
                else
                {
                    return BadRequest("cannot create todolist ");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpDelete("DeleteToDoList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteToDoList(long id)
        {
            
            try
            {
                ToDoList toDoList = await _toDoListService.GetToDoList(id);
                bool result = await _toDoListService.RemoveToDoList(toDoList);

                if (result)
                {
                    return Ok("remove todolist chat sussess");
                }
                else
                {
                    return BadRequest("cannot remove todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPut("UpdateToDoList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateToDoList(ToDoListModel model)
        {
            ToDoList toDoList = _mapper.Map<ToDoList>(model);
            try
            {
                
                bool result = await _toDoListService.UpdateRToDoList(toDoList);

                if (result)
                {
                    return Ok("Update todolist chat sussess");
                }
                else
                {
                    return BadRequest("cannot Update todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetAllTasks")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTask()
        {
           
            try
            {

                List<ToDoList> toDoLists = await _toDoListService.GetAll();

                if (toDoLists!=null)
                {
                    return Ok(toDoLists);
                }
                else
                {
                    return BadRequest("cannot Update todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetTasksAreDone")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTasksAreDone()
        {

            try
            {

                List<ToDoList> toDoLists = await _toDoListService.GetAllTasksAreDone();

                if (toDoLists != null)
                {
                    return Ok(toDoLists);
                }
                else
                {
                    return BadRequest("cannot Update todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        [HttpPost("AddMoreUserDoThisTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMoreUserDoThisTask(long taskId,List<string> userIds)
        {

            try
            {

                 ToDoList toDoList = await _toDoListService.GetToDoList(taskId);

                if (toDoList != null)
                {
                    foreach (string userId in userIds)
                    {
                        ToDoUser toDoUser = new ToDoUser()
                        {
                            TaskId = taskId,
                            UserDes = userId,
                            Status = 0
                        };
                        await _toDoUserService.AddToDoUser(toDoUser);
                    }
                    return Ok("Add user success");
                }        
                else
                {
                    return BadRequest("cannot Update todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetAllUsersDoThisTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsersDoThisTask(long taskId)
        {
            try
            {

                List<ToDoUser> toDoList = await _toDoUserService.GetAllUsersOfTask(taskId);

                if (toDoList != null)
                {
                
                    return Ok(toDoList);
                }
                else
                {
                    return BadRequest("cannot Update todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpDelete("RemoveUserDoThisTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUserDoThisTask(long taskId, List<string> userIds)
        {
            try
            {

                List<ToDoUser> toDoList = await _toDoUserService.GetAllUsersOfTask(taskId);
                List<ToDoUser> usersToRemove = toDoList.Where(x => userIds.Contains(x.UserDes)).ToList();

                if (toDoList != null)
                {
                    foreach(ToDoUser toDoUser in usersToRemove)
                    {
                        _toDoUserService.RemoveToDoUser(toDoUser);
                    }
                    return Ok("Update todolist success");
                }
                else
                {
                    return BadRequest("cannot Update todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
