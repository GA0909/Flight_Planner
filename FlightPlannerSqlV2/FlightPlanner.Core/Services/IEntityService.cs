using FlightPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Core.Services
{
    public interface IEntityService<T> where T : Entity
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Create(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
