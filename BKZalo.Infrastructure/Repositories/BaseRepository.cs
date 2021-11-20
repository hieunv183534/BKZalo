using BKZalo.Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKZalo.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {

        #region Declare

        string _tableName;

        #endregion

        #region Consrtuctor

        public BaseRepository()
        {
            _tableName = typeof(TEntity).Name;
        }

        #endregion

        /// <summary>
        /// insert 1 bản ghi vào database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(TEntity entity)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var props = entity.GetType().GetProperties();
                DynamicParameters parameters = new DynamicParameters();
                foreach (var prop in props)
                {
                    var propName = prop.Name;
                    var propValue = prop.GetValue(entity);
                    parameters.Add($"@{propName}", propValue);
                }
                var procName = $"Proc_Insert{_tableName}";
                var rowAffect = dbConnection.Execute(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return rowAffect;
            }
        }

        /// <summary>
        /// Xóa một bản ghi theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public int Delete(Guid entityId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var procName = $"Proc_Delete{_tableName}ById";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_tableName}Id", entityId);
                var rowAffect = dbConnection.Execute(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return rowAffect;
            }
        }

        public int DeleteByProp(string propName, object propValue)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{propName}", propValue.ToString());
            var sql = $"delete from {_tableName} where {propName} = @{propName} ";
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var rowAffect = dbConnection.Execute(sql, param: parameters);
                return rowAffect;
            }
        }

        /// <summary>
        /// lấy toàn bộ các bản ghi
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll()
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var procName = $"Proc_Get{_tableName}s";
                var entities = dbConnection.Query<TEntity>(procName, commandType: CommandType.StoredProcedure);
                return (List<TEntity>)entities;
            }
        }

        /// <summary>
        /// lấy bản ghi thao id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public TEntity GetById(Guid entityId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var procName = $"Proc_Get{_tableName}ById";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_tableName}Id", entityId);
                var entity = dbConnection.QueryFirstOrDefault<TEntity>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return entity;
            }
        }

        /// <summary>
        /// Lấy theo 1 prop
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public TEntity GetByProp(string propName, object propValue)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{propName}", propValue.ToString());
            var sql = $"select * from {_tableName} where {propName} = @{propName} ";
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var entity = dbConnection.QueryFirstOrDefault<TEntity>(sql, param: parameters);
                return entity;
            }
        }

        /// <summary>
        /// cập nhật bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public int Update(TEntity entity, Guid entityId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                var props = entity.GetType().GetProperties();
                DynamicParameters parameters = new DynamicParameters();
                foreach (var prop in props)
                {
                    var propName = prop.Name;
                    var propValue = prop.GetValue(entity);
                    if (propName == $"{_tableName}Id")
                    {
                        propValue = entityId;
                    }
                    parameters.Add($"@{propName}", propValue);
                }
                var procName = $"Proc_Update{_tableName}";
                var rowAffect = dbConnection.Execute(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return rowAffect;
            }
        }
    }
}
