using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnetIndexedDb
{
    /// <summary>
    /// Base class creating Repository classes.
    /// </summary>
    /// <typeparam name="TEntity">The table class</typeparam>
    /// <typeparam name="TKey">Primary key data type</typeparam>
    public class RepositoryBase<TEntity, TKey> where TEntity : class
    {
        private readonly IndexedDbInterop db;
        /// <summary>
        /// Type object for the TEntity (Lazy Loaded)
        /// </summary>
        protected Lazy<Type> entityType = new Lazy<Type>(typeof(TEntity));
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db">The inherited IndexedDbInterop class</param>
        protected RepositoryBase(IndexedDbInterop db)
        {
            this.db = db;
        }

        public virtual async ValueTask<string> Add(List<TEntity> items)
        {
            var result = await db.AddItems<TEntity>(entityType.Value.Name, items);
            return result;
        }

        public virtual async ValueTask<string> Replace(List<TEntity> items)
        {
            await DeleteAll();
            var result = await db.AddItems<TEntity>(entityType.Value.Name, items);
            return result;
        }

        public virtual async ValueTask<string> Add(TEntity item)
        {
            var items = new List<TEntity>() { item };
            var result = await Add(items);
            return result;
        }

        public virtual async ValueTask<TEntity> GetByKey(TKey key)
        {
            var result = await db.GetByKey<TKey, TEntity>(entityType.Value.Name, key);
            return result;
        }

        public virtual async ValueTask<string> DeleteByKey(TKey key)
        {
            var result = await db.DeleteByKey<TKey>(entityType.Value.Name, key);
            return result;
        }

        public virtual async ValueTask<List<TEntity>> GetAll()
        {
            var result = await db.GetAll<TEntity>(entityType.Value.Name);
            return result;
        }

        public virtual async ValueTask<List<TEntity>> GetRangeByKey(TKey lowerBound, TKey upperBound)
        {
            var result = await db.GetRange<TKey, TEntity>(entityType.Value.Name, lowerBound, upperBound);
            return result;
        }

        public virtual async ValueTask<List<TEntity>> GetByIndex<TIndex>(TIndex value, string indexName)
        {
            var result = await db.GetByIndex<TIndex, TEntity>(entityType.Value.Name, value, value, indexName.ToCamelCase(), false);
            return result;
        }

        public virtual async ValueTask<List<TEntity>> GetRangeByIndex<TIndex>(TIndex lowerBound, TIndex upperBound, string indexName, bool isRange)
        {
            var result = await db.GetByIndex<TIndex, TEntity>(entityType.Value.Name, lowerBound, upperBound, indexName.ToCamelCase(), isRange);
            return result;
        }

        public virtual async ValueTask<TEntity?> GetFirstOrDefaultByIndex<TIndex>(TIndex value, string indexName)
        {
            var result = await db.GetByIndex<TIndex, TEntity>(entityType.Value.Name, value, value, indexName.ToCamelCase(), false);
            return result?.FirstOrDefault();
        }

        public virtual async ValueTask<TEntity?> GetFirstOrDefaultByKey()
        {
            var minKey = await GetMinKey();
            return minKey == null ? null : await GetByKey(minKey);
        }

        public virtual async ValueTask<TKey> GetMaxKey()
        {
            var result = await db.GetMaxKey<TKey>(entityType.Value.Name);
            return result;
        }

        public virtual async ValueTask<TKey> GetMinKey()
        {
            var result = await db.GetMinKey<TKey>(entityType.Value.Name);
            return result;
        }

        public virtual async ValueTask<TIndex> GetMaxIndex<TIndex>(string indexName)
        {
            var result = await db.GetMaxIndex<TIndex>(entityType.Value.Name, indexName.ToCamelCase());
            return result;
        }

        public virtual async ValueTask<TIndex> GetMinIndex<TIndex>(string indexName)
        {
            var result = await db.GetMinIndex<TIndex>(entityType.Value.Name, indexName.ToCamelCase());
            return result;
        }

        public virtual async ValueTask<string> Update(List<TEntity> items)
        {
            var result = await db.UpdateItems<TEntity>(entityType.Value.Name, items);
            return result;
        }

        public virtual async Task<string> Update(TEntity item)
        {
            var items = new List<TEntity>() { item };
            var result = await Update(items);
            return result;
        }

        public virtual async ValueTask<string> DeleteAll()
        {
            var result = await db.DeleteAll(entityType.Value.Name);
            return result;
        }
    }
}
