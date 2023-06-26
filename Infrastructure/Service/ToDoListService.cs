using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IToDoListService
    {
        Task<ToDoList> GetToDoList(long id);
        Task<List<ToDoList>> GetAll();

        Task<List<ToDoList>> GetAllTasksAreDone();
        Task<bool> AddToDoList(ToDoList toDoList,List<String> userToDoTasks);
        Task<bool> RemoveToDoList(ToDoList toDoList);
        Task<bool> UpdateRToDoList(ToDoList toDoList);


    }
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _toDoListRepo;
        private readonly IToDoUserRepository _userRepo;
        public ToDoListService(IToDoListRepository toDoListRepository,IToDoUserRepository toDoUserRepository)
        {
            _toDoListRepo = toDoListRepository;
            _userRepo = toDoUserRepository;
        }
        public async Task<bool> AddToDoList(ToDoList toDoList, List<string> userToDoTasks)
        {
            var transaction = await _toDoListRepo.BeginTransaction();

            bool result =  await _toDoListRepo.Add(toDoList);
            if (result)
            {
                long taskId = toDoList.Id;
                foreach(string userId in  userToDoTasks)
                {
                    ToDoUser toDoUser = new ToDoUser() {
                        TaskId = taskId,
                        UserDes = userId,
                        Status = 0
                    };
                    await _userRepo.Add(toDoUser);
                }
            }

            if (!result)
            {
                transaction.Rollback();
                transaction.Dispose();
                return false;
            }

            transaction.Commit();
            transaction.Dispose();
            return result;
             
        }

        public async Task<List<ToDoList>> GetAll()
        {
            return await _toDoListRepo.GetAll().ToListAsync();
        }

        public async Task<List<ToDoList>> GetAllTasksAreDone()
        {
            return await _toDoListRepo.GetAll().Where(x=>x.IsDone.Equals(true)).Select(x=>x).ToListAsync();
        }

        public async Task<ToDoList> GetToDoList(long id)
        {
           return await _toDoListRepo.GetById(id);
        }

        public async Task<bool> RemoveToDoList(ToDoList toDoList)
        {
            return await _toDoListRepo.Delete(toDoList);
        }

        public async Task<bool> UpdateRToDoList(ToDoList toDoList)
        {
            return await _toDoListRepo.Update(toDoList);
        }
    }
}
