using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> entitySet, 
        ISpecification<TEntity> spec)
        {
            var query = entitySet;

            if(spec.Criteria is not null) query = query.Where(spec.Criteria);
            
            if(spec.OrderBy is not null) query = query.OrderBy(spec.OrderBy);

            if(spec.OrderByDesc is not null) query = query.OrderByDescending(spec.OrderByDesc);

            if(spec.IsPagingEnabled) query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}