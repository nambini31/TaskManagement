using Application.Interface;
using Domain.DTO;
using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public TasksService(ITasksRepository tasksRepository, IProjectRepository projectRepository)
        {
            _taskRepository = tasksRepository;
            _projectRepository = projectRepository;
        }

        public IEnumerable<TasksDto> GetAllTasks()
        {
            return _taskRepository.GetAll().Select(t => new TasksDto
            {
                taskId = t.taskId,
                name = t.name,
                projectId = t.projectId,
                projectName = t.project.name
            });
        }

        public TasksDto GetTaskById(int id)
        {
            var task = _taskRepository.GetById(id);
            return new TasksDto
            {
                taskId = task.taskId,
                name = task.name,
                projectId = task.projectId,
                projectName = task.project.name
            };
        }

        public void CreateTask(TasksDto task)
        {
            var entity = new Tasks
            {
                name = task.name,
                projectId = task.projectId
            };
            _taskRepository.Create(entity);
        }

        public void UpdateTask(TasksDto task)
        {
            var entity = _taskRepository.GetById(task.taskId);
            entity.name = task.name;
            entity.projectId = task.projectId;
            _taskRepository.Update(entity);
        }

        public void DeleteTask(int id)
        {
            _taskRepository.Delete(id);
        }

        public IEnumerable<Tasks> GetTaskByIdProject(int id)
        {

           return _taskRepository.GetTaskByIdProject(id);
        }
    }
}
