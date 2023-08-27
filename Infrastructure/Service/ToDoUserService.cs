using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IToDoUserService{
        Task<bool> AddToDoUser(ToDoUser toDoUser);
        Task<bool> RemoveToDoUser(ToDoUser toDoUser);
        Task<List<ToDoUser>> GetAllUsersOfTask(long taskId);
        Task<List<ToDoUser>> GetAlltodoTask();
    }
    public class ToDoUserService : IToDoUserService
    {
        IToDoUserRepository _toDoUserRepo;
        public ToDoUserService(IToDoUserRepository toDoUserRepository)
        {
            _toDoUserRepo = toDoUserRepository;
        }
        public async Task<bool> AddToDoUser(ToDoUser toDoUser)
        {
            return await _toDoUserRepo.Add(toDoUser);
        }

        public async Task<List<ToDoUser>> GetAllUsersOfTask(long taskId)
        {
            return await _toDoUserRepo.GetAll().Where(x=>x.TaskId.Equals(taskId)).Select(x=>x).ToListAsync();
        }

        public async Task<bool> RemoveToDoUser(ToDoUser toDoUser)
        {
            return await _toDoUserRepo.Delete(toDoUser);
        }
        public async Task<List<ToDoUser>> GetAlltodoTask(){
            return await _toDoUserRepo.GetAll().ToListAsync();
        }
    }
}
