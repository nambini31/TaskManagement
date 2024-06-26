using Application.Interface;
using Domain.DTO;
using Domain.Entity;
using Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _taskRepository;

        public TasksService(ITasksRepository tasksRepository)
        {
            _taskRepository = tasksRepository;
        }

        public async Task<IEnumerable<TasksDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(t => new TasksDto
            {
                taskId = t.taskId,
                name = t.name,
                projectId = t.projectId,
                projectName = t.project?.name
            });
        }

        public async Task<TasksDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return new TasksDto
            {
                taskId = task.taskId,
                name = task.name,
                projectId = task.projectId,
                projectName = task.project?.name
            };
        }

        public async Task CreateTaskAsync(TasksDto taskDto)
        {
            var entity = new Tasks
            {
                name = taskDto.name,
                projectId = taskDto.projectId
            };
            await _taskRepository.CreateAsync(entity);
        }

        public async Task UpdateTaskAsync(TasksDto taskDto, int user_maj)
        {
            var entity = await _taskRepository.GetByIdAsync(taskDto.taskId);
            entity.name = taskDto.name;
            entity.projectId = taskDto.projectId;
            await _taskRepository.UpdateAsync(entity, user_maj);
        }

        public async Task DeleteTaskAsync(int id, int user_maj)
        {
            await _taskRepository.DeleteAsync(id, user_maj);
        }

        public IEnumerable<Tasks> GetTaskByIdProject(int id)
        {

            return _taskRepository.GetTaskByIdProject(id);
        }
    }
}
