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
        IContactService _contactService;
        IMapper _mapper;
        public ToDoListController(IContactService contactService,IToDoListService toDoListService,IToDoUserService toDoUserService,IMapper mapper) { 
            _mapper = mapper;
            _toDoListService = toDoListService;
            _toDoUserService = toDoUserService;
            _contactService = contactService;
        }


        [HttpPost("CreateToDoList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateToDoList(ToDoListModel model)
        {
            ToDoList toDoList = _mapper.Map<ToDoList>(model);
            toDoList.StartDate = DateTime.Now;
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
         [HttpGet("GetAllTasksDoneByUserCreation")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTasksDoneByUserCreation(string userId)
        {
           
            try
            {

                List<ToDoList> toDoLists = await _toDoListService.GetAll();
                toDoLists = toDoLists.Where(e => e.UserSrc == userId && e.IsDone == true).ToList();
                if (toDoLists!=null)
                {
                    return Ok(toDoLists);
                }
                else
                {
                    return BadRequest("cannot get todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
           [HttpGet("GetAllTasksNotDoneByUserCreation")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTasksNotDoneByUserCreation(string userId)
        {
           
            try
            {

                List<ToDoList> toDoLists = await _toDoListService.GetAll();
                toDoLists = toDoLists.Where(e => e.UserSrc == userId && e.IsDone == false).ToList();
                if (toDoLists!=null)
                {
                    return Ok(toDoLists);
                }
                else
                {
                    return BadRequest("cannot get todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

             [HttpGet("GetAllTasksDoneByUserDo")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTasksDoneByUserDo(string userId)
        {
            try
            {

                List<ToDoUser> toDoListsOfUser = await _toDoUserService.GetAlltodoTask();
                toDoListsOfUser = toDoListsOfUser.Where(u => u.UserDes == userId).ToList();
                List<ToDoList> toDoList = new List<ToDoList>();
                foreach (ToDoUser toDoUser in  toDoListsOfUser){
                    ToDoList addToDoList = await _toDoListService.GetToDoList(toDoUser.TaskId);
                    if(addToDoList.IsDone==true){
                          toDoList.Add(addToDoList);
                    }
                
                }
                if (toDoList!=null)
                {
                    return Ok(toDoList);
                }
                else
                {
                    return BadRequest("cannot get todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetAllTasksNotDoneByUserDo")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTasksNotDoneByUserDo(string userId)
        {
            try
            {

                List<ToDoUser> toDoListsOfUser = await _toDoUserService.GetAlltodoTask();
                toDoListsOfUser = toDoListsOfUser.Where(u => u.UserDes == userId).ToList();
                List<ToDoList> toDoList = new List<ToDoList>();
                foreach (ToDoUser toDoUser in  toDoListsOfUser){
                    ToDoList addToDoList = await _toDoListService.GetToDoList(toDoUser.TaskId);
                    if(addToDoList.IsDone==false){
                          toDoList.Add(addToDoList);
                    }
                
                }
                if (toDoList!=null)
                {
                    return Ok(toDoList);
                }
                else
                {
                    return BadRequest("cannot get todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
          [HttpGet("GetAllTasksAndUserNotCompleteOfUserDes")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTasksAndUserNotCompleteOfUserDes(string userId)
        {
            try
            {
                List<ToDoList> toDoLists = await _toDoListService.GetAll();
                toDoLists = toDoLists.Where(u => u.UserSrc == userId && u.IsDone==false).ToList();
                List<UncompleteTaskUser> getUserFollowToTask = new List<UncompleteTaskUser>();
                foreach (ToDoList toDoList in  toDoLists){
                    List<ToDoUser> userUncomplete = await _toDoUserService.GetAllUsersOfTask(toDoList.Id);
                    List<string> idUserNotCompletes = userUncomplete.Where(u => u.Status == 0).Select(e=>e.UserDes).ToList();
                    List<Contact> contactUncomplete = new List<Contact>();
                    foreach (string idUserNotComplete in idUserNotCompletes){
                        Contact contact = await _contactService.GetContactData(idUserNotComplete);
                        contactUncomplete.Add(contact);
                    }
                    // tạo danh sách thông tin những người chưa hoàn thành
                    UncompleteTaskUser userFollowToTask = new UncompleteTaskUser();
                    userFollowToTask.ListUserUncompleteThisTask = contactUncomplete;
                    userFollowToTask.UserSrc = userId;
                    userFollowToTask.IdTask = toDoList.Id;
                    userFollowToTask.Content = toDoList.Content;
                    userFollowToTask.Title = toDoList.Title;
                    userFollowToTask.EndDate = toDoList.EndDate;
                    userFollowToTask.IsDone = toDoList.IsDone;
                    getUserFollowToTask.Add(userFollowToTask);  
                }
                if (getUserFollowToTask!=null)
                {
                    return Ok(getUserFollowToTask);
                }
                else
                {
                    return BadRequest("cannot get todolist");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
