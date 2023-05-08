﻿using Microsoft.EntityFrameworkCore;
using P013EStore.Core.Entities;
using P013EStore.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Data.Concrete
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new() // Repository classına gönderilecek T nin şartları: T(brand, category, product vb) bir class olmalı, IEntity den implemente almalı ve new lenebilir olmalı
    {
        internal DatabaseContext _context; // boş bir database oluşturduk 
        internal DbSet<T> _dbSet;//boş bir dbset tanımladık repository e gönderilecek T classını parametre verdik

        public Repository(DatabaseContext context)
        {
            _context = context; // _context 
            _dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }
        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public T Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList(); // eğer sadece listeleme yapacaksak yani liste üzerinde kayıt güncelleme gibi bir işlem yapmayacaksak entity framework deki AsNoTracking yöntemi ile listeyi daha performanslı çekebiliriz.
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.AsNoTracking().Where(expression).ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public int Save()
        {
            return _context.SaveChanges(); // entity framework de savechanges(ekle, güncelle,
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}

