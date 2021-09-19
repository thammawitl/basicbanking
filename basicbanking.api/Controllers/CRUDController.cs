using basicbanking.api.Data;
using basicbanking.api.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basicbanking.api.Controllers
{
    public abstract class CRUDController<T> : ControllerBase where T : EntityBase
    {
        protected IRepository<T> _repo;

        public CRUDController(IRepository<T> repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        [Route("")]
        public virtual IEnumerable<T> GetItems()
        {
            return this._repo.Find(i => true);
        }

        [HttpPost]
        [Route("")]
        public virtual T Add(T item)
        {
            this._repo.Insert(item);
            return item;
        }

        [HttpPut]
        [Route("{id}")]
        public virtual void UpdateItem(T item)
        {
            this._repo.Update(item);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual T GetItemById(long id)
        {
            return this._repo.GetById(id);
        }
    }
}
