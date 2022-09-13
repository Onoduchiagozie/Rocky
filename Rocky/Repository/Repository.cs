using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class Repository<T>:IRepository<T> where T:class
{
    private readonly RockyDbContext _db;
    internal DbSet<T> dbSet;

    public Repository(RockyDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
    }
    
    public T Find(int id)
    {
        return dbSet.Find(id);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = null,
        bool isTracking = true)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split( new Char[] {','},StringSplitOptions.RemoveEmptyEntries ))
            {
                query = query.Include(includeProp);
            }
        }

        if (orderby != null)
        {
            query = orderby(query);
        }

        if (!isTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToList();
    }

    public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
    {

    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split( new Char[] {','},StringSplitOptions.RemoveEmptyEntries ))
            {
                query = query.Include(includeProp);
            }
        }
        if (!isTracking)
        {
            query = query.AsNoTracking();
        }

        return query.FirstOrDefault();
    }
    
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}