using Domain.Entity;
using System;
using System.Collections.Generic;


namespace Domain.Interface
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAll();
        Project GetById(int id);
        void Create(Project project);
        void Update(Project project);
        void Delete(int id);
    }
}

  