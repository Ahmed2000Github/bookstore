using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{

    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly DataContext _dataContext;
        private IGenericRepository<T> _entity = null;
        public UnitOfWork(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }


        public IGenericRepository<T> entity
        {
            get
            {
                return _entity ?? (_entity = new GenericRepository<T>(_dataContext));
            }
        }

        public void save()
        {
            _dataContext.SaveChanges();
        }
    }
}
