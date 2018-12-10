using Dapper;
using StoneCo.Buy4.Infrastructure;
using StoneCo.Buy4.Infrastructure.PerformanceCounters;
using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Sql;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories
{
    public class AuthenticationRepository : RepositoryBase, IAuthenticationRepository
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="dbSettings"></param>
        /// <param name="logger"></param>
        public AuthenticationRepository(IDatabaseSettings dbSettings, ILogger logger) : base(dbSettings, logger) { }

        /// <inheritdoc />
        public async Task<IList<AuthenticationModel>> GetByFilter(GetAuthenticationsRequest request)
        {
            try
            {
                IList<AuthenticationModel> result = new List<AuthenticationModel>();

                DynamicParameters parameters = new DynamicParameters();
                string sql = OperationTemplateSqlResource.InsertAuthentication;
                string sqlWithFilters = this.BuildGetAuthenticationSqlFilter(sql, request, parameters);
                string sqlWithFiltersAndPaging = this.BuildPagingSql(sqlWithFilters, request.Limit.Value, request.Offset.Value);

                IEnumerable<AuthenticationModel> queryResult = null;

                using (OperationContext.Current.Counters.MeasureTime(AppGlobal.MeasureTimeExternalServicesKey, AppGlobal.DatabaseKey, this.GetType().Name, MethodBase.GetCurrentMethod().Name))
                {
                    using (var connection = new SqlConnection(this.DatabaseSettings.ConnectionString))
                    {
                        connection.Open();
                        queryResult = await connection
                            .QueryAsync<AuthenticationModel>(sqlWithFiltersAndPaging, parameters, commandTimeout: this.DatabaseSettings.QueryTimeoutInSeconds)
                            .ConfigureAwait(false);
                    }
                }

                return queryResult?.ToList();
            }
            catch (Exception ex)
            {
                this.Logger.DatabaseError(ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<AuthenticationModel> Insert(AuthenticationModel authentication)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ApplicationName", new DbString { Value = authentication.ApplicationName, IsAnsi = true, IsFixedLength = false });
                parameters.Add("@ApplicationKey", new DbString { Value = authentication.ApplicationKey, IsAnsi = true, IsFixedLength = true, Length = 32 });
                parameters.Add("@ApplicationToken", new DbString { Value = authentication.ApplicationName, IsAnsi = true, IsFixedLength = false });
                parameters.Add("@IsActive", authentication.IsActive, DbType.Boolean);
                parameters.Add("@CreationDateTime", authentication.CreationDateTime, DbType.DateTimeOffset);

                string sql = OperationTemplateSqlResource.InsertAuthentication;

                using (OperationContext.Current.Counters.MeasureTime(AppGlobal.MeasureTimeExternalServicesKey, AppGlobal.DatabaseKey, this.GetType().Name, MethodBase.GetCurrentMethod().Name))
                {
                    using (var connection = new SqlConnection(this.DatabaseSettings.ConnectionString))
                    {
                        connection.Open();
                        authentication.Id = await connection.ExecuteScalarAsync<int>(sql, parameters);
                    }
                }

                return authentication;
            }
            catch (Exception ex)
            {
                this.Logger.DatabaseError(ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<int> UpdateActivation(string applicationKey, bool activate)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ApplicationKey", new DbString { Value = applicationKey, IsAnsi = true, IsFixedLength = true, Length = 32 });
                parameters.Add("@IsActive", activate, DbType.Boolean);

                string sql = OperationTemplateSqlResource.UpdateAuthenticationActivation;
                int numberOfAffectedRows = 0;

                using (OperationContext.Current.Counters.MeasureTime(AppGlobal.MeasureTimeExternalServicesKey, AppGlobal.DatabaseKey, this.GetType().Name, MethodBase.GetCurrentMethod().Name))
                {
                    using (var connection = new SqlConnection(this.DatabaseSettings.ConnectionString))
                    {
                        connection.Open();
                        numberOfAffectedRows = await connection.ExecuteScalarAsync<int>(sql, parameters);
                    }
                }

                return numberOfAffectedRows;
            }
            catch (Exception ex)
            {
                this.Logger.DatabaseError(ex);
                throw;
            }
        }

        #region Private Methods

        private string BuildGetAuthenticationSqlFilter(string sqlTemplate, GetAuthenticationsRequest request, DynamicParameters parameters)
        {
            StringBuilder sqlWhere = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(request.ApplicationKey))
            {
                sqlWhere.AppendLine("AND (aut.[ApplicationKey] = @ApplicationKey)");
                parameters.Add("@ApplicationKey", new DbString { Value = request.ApplicationKey, IsAnsi = true, IsFixedLength = true, Length = 32 });
            }

            if (!string.IsNullOrWhiteSpace(request.ApplicationName))
            {
                sqlWhere.AppendLine("AND (aut.[ApplicationName] = @ApplicationName)");
                parameters.Add("@ApplicationName", new DbString { Value = request.ApplicationName, IsAnsi = true, IsFixedLength = false });
            }

            if (!string.IsNullOrWhiteSpace(request.ApplicationToken))
            {
                sqlWhere.AppendLine("AND (aut.[ApplicationToken] = @ApplicationToken)");
                parameters.Add("@ApplicationToken", new DbString { Value = request.ApplicationToken, IsAnsi = true, IsFixedLength = false });
            }

            if (request.StartCreationDateTime.HasValue)
            {
                sqlWhere.AppendLine("AND (aut.[CreationDateTime] >= @StartCreationDateTime)");
                parameters.Add("@StartCreationDateTime", request.StartCreationDateTime.Value.ToUniversalTime().DateTime, DbType.DateTime);
            }

            if (request.EndCreationDateTime.HasValue)
            {
                sqlWhere.AppendLine("AND (aut.[CreationDateTime] <= @EndCreationDateTime)");
                parameters.Add("@EndCreationDateTime", request.EndCreationDateTime.Value.ToUniversalTime().DateTime, DbType.DateTime);
            }

            if (request.IsActive.HasValue)
            {
                sqlWhere.AppendLine("AND (aut.[IsActive] = @IsActive)");
                parameters.Add("@IsActive", request.IsActive, DbType.Boolean);
            }

            return sqlTemplate.Replace("#WHERE", sqlWhere.ToString());
        }

        #endregion
    }
}
