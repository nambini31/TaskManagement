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
        private readonly SUserTaskRepository _SUserTask;

        public TasksService(ITasksRepository tasksRepository, SUserTaskRepository _SUserTask)
        {
            _taskRepository = tasksRepository;
            this._SUserTask = _SUserTask;
        }

        public async Task<IEnumerable<TasksDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();

            return tasks.Select(t =>
            {
                var timeElapsed = _SUserTask.TotalTimeElapsedByTask(t.taskId);
                var calculatedStatus = (timeElapsed / (double)t.timeTotal) * 100;

                // Crée un nouvel objet TasksDto avec le statut calculé
                return new TasksDto
                {
                    taskId = t.taskId,
                    name = t.name,
                    projectId = t.projectId,
                    projectName = t.project?.name,
                    timeTotal = t.timeTotal,
                    timeElapsed = timeElapsed,
                    status = (int)Math.Min(calculatedStatus, 100), // Assurez-vous que le statut ne dépasse pas 100%
                };
            }).ToList();
        }

        public async Task<TasksDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return new TasksDto
            {
                taskId = task.taskId,
                name = task.name,
                projectId = task.projectId,
                projectName = task.project?.name,
                timeTotal = task.timeTotal,
            };
        }

        public async Task CreateTaskAsync(TasksDto taskDto)
        {
            var timeTotal = taskDto.timeTotal;
            var status = (0 / timeTotal) * 100;
            var entity = new Tasks
            {
                name = taskDto.name,
                projectId = taskDto.projectId,
                timeTotal = timeTotal,
                status = status,
            };
            await _taskRepository.CreateAsync(entity);
        }

        public async Task UpdateTaskAsync(TasksDto taskDto, int user_maj)
        {
            var entity = await _taskRepository.GetByIdAsync(taskDto.taskId);
            entity.name = taskDto.name;
            entity.projectId = taskDto.projectId;
            var timeTotal = taskDto.timeTotal;
            var timeElapsed = _SUserTask.TotalTimeElapsedByTask(taskDto.taskId);
            var calculatedStatus = (timeElapsed / (double)timeTotal) * 100;
            entity.timeTotal = timeTotal;
            entity.status = (int)Math.Min(calculatedStatus, 100);
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

        public async Task<IEnumerable<TasksDto>> ChartProjectProcess()
        {

            // Grouper les tâches par projectId et calculer l'avancement pour chaque projet
            var tasks = await _taskRepository.GetAllAsync();

            // Grouper les tâches par projectId et calculer l'avancement pour chaque projet
            return tasks
                .GroupBy(t => t.projectId)
                .Select(g =>
                {
                    // Calculer le temps total et le temps écoulé pour chaque projet
                    var totalElapsedTime = g.Sum(t => _SUserTask.TotalTimeElapsedByTask(t.taskId));
                    var totalEstimatedTime = g.Sum(t => t.timeTotal);

                    // Calculer l'avancement en pourcentage du projet
                    var calculatedProgress = totalEstimatedTime > 0
                        ? (totalElapsedTime / (double)totalEstimatedTime) * 100
                        : 0;

                    // Créer un objet ProjectProgressDto pour représenter l'avancement du projet
                    return new TasksDto
                    {
                        projectId = g.Key,
                        projectName = g.FirstOrDefault()?.project?.name,
                        timeTotal = totalEstimatedTime,
                        timeElapsed = totalElapsedTime,
                        status = (int)Math.Min(calculatedProgress, 100) // S'assurer que l'avancement ne dépasse pas 100%
                    };
                })
                .ToList();
            //var tasks = await _taskRepository.GetAllAsync();

            //return tasks.Select(t =>
            //{
            //    var timeElapsed = _SUserTask.TotalTimeElapsedByTask(t.taskId);
            //    var calculatedStatus = (timeElapsed / (double)t.timeTotal) * 100;

            //    // Crée un nouvel objet TasksDto avec le statut calculé
            //    return new TasksDto
            //    {
            //        taskId = t.taskId,
            //        name = t.name,
            //        projectId = t.projectId,
            //        projectName = t.project?.name,
            //        timeTotal = t.timeTotal,
            //        timeElapsed = timeElapsed,
            //        status = (int)Math.Min(calculatedStatus, 100), // Assurez-vous que le statut ne dépasse pas 100%
            //    };
            //}).ToList();
        }

        public Task<IEnumerable<TasksDto>> ChartTaskProcessByProject()
        {
            throw new NotImplementedException();
        }

        //public Task<IEnumerable<TasksDto>> ChartTaskProcessByProject()
        //{
        //    var tasks = await _taskRepository.GetAllAsync();

        //    return tasks.Select(t =>
        //    {
        //        var timeElapsed = _SUserTask.TotalTimeElapsedByTask(t.taskId);
        //        var calculatedStatus = (timeElapsed / (double)t.timeTotal) * 100;

        //        // Crée un nouvel objet TasksDto avec le statut calculé
        //        return new TasksDto
        //        {
        //            taskId = t.taskId,
        //            name = t.name,
        //            projectId = t.projectId,
        //            projectName = t.project?.name,
        //            timeTotal = t.timeTotal,
        //            timeElapsed = timeElapsed,
        //            status = (int)Math.Min(calculatedStatus, 100), // Assurez-vous que le statut ne dépasse pas 100%
        //        };
        //    }).ToList();
        //}
    }
}
