using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ITasksRepository
    {
        IEnumerable<Tasks> GetAll();
        IEnumerable<Tasks> GetTaskByIdProject(int id);
        Tasks GetById(int id);
        void Create(Tasks tasks);
        void Update(Tasks tasks);
        void Delete(int id);
    }
}
