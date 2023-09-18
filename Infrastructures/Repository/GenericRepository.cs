using Application.Contracts;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _ctx;
        private DbSet<T> _entity = null;
        public GenericRepository (ApplicationDbContext ctx)
        {
            _ctx = ctx;
            _entity = _ctx.Set<T>();
        }
        public bool Add(T entity)
        {
            try
            {
                _entity.Add(entity);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                _entity.Remove(entity);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { 
                return false;
            }
        }

        public List<T> GetAll()
        {
            return _entity.ToList();
        }



        public bool Update(T entity)
        {
            try
            {
                _entity.Update(entity);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
