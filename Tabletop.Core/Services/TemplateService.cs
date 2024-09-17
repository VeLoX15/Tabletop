using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class TemplateService : IModelService<Template, int, TemplateFilter>
    {
        public async Task CreateAsync(Template input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO Templates 
                (
                UserId,
                FractionId,
                Name,
                Force,
                UsedForce
                )
                VALUES
                (
                @USER_ID,
                @FRACTION_ID,
                @NAME,
                @FORCE,
                @USED_FORCE
                ); {dbController.GetLastIdSql()}";

            input.TemplateId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            foreach(var unit in input.Units)
            {
                await UnitService.CreateTemplateUnitAsync(input, unit, dbController, cancellationToken);
            }
        }

        public async Task DeleteAsync(Template input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await UnitService.DeleteTemplateUnitsAsync(input.TemplateId, dbController, cancellationToken);

            string sql = "DELETE FROM Templates WHERE TemplateId = @TEMPLATE_ID";

            await dbController.QueryAsync(sql, new
            {
                TEMPLATE_ID = input.TemplateId
            }, cancellationToken);
            
        }

        public async Task<Template?> GetAsync(int templateId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM Templates  WHERE TemplateId = @TEMPLATE_ID";

            var template = await dbController.GetFirstAsync<Template>(sql, new
            {
                TEMPLATE_ID = templateId
            }, cancellationToken);

            return template;
        }

        public static async Task<List<Template>> GetTemplateOnForceAsync(int user_id, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "SELECT * FROM Templates WHERE UserId = @USER_ID";

            var list = await dbController.SelectDataAsync<Template>(sql, new
            {
                USER_ID = user_id
            }, cancellationToken);

            foreach (var item in list)
            {
                item.Units = await UnitService.GetTemplateUnitsAsync(item.TemplateId, dbController, cancellationToken);
            }

            return list;
        }

        public async Task<List<Template>> GetAsync(TemplateFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT * FROM Templates  WHERE UserId = @USER_ID");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$"  ORDER BY Name ASC");
            sb.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sb.ToString();

            List<Template> list = await dbController.SelectDataAsync<Template>(sql, GetFilterParameter(filter), cancellationToken);

            foreach(var item in list)
            {
                item.Units = await UnitService.GetTemplateUnitsAsync(item.TemplateId, dbController, cancellationToken);
            }

            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(TemplateFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" },
                { "USER_ID", filter.UserId },
                { "FRACTION_ID", filter.FractionId },
                { "FORCE", filter.Force }
            };
        }

        public string GetFilterWhere(TemplateFilter filter)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sb.AppendLine(@" AND (UPPER(Name) LIKE @SEARCHPHRASE)");
            }
            if (filter.FractionId != 0)
            {
                sb.AppendLine(@" AND FractionId = @FRACTION_ID");
            }
            if (filter.Force != 0)
            {
                sb.AppendLine(@" AND Force = @FORCE");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(TemplateFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM Templates WHERE UserId = @USER_ID");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public async Task UpdateAsync(Template input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await UnitService.DeleteTemplateUnitsAsync(input.TemplateId, dbController, cancellationToken);

            string sql = @"UPDATE Templates SET
                FractionId = @FRACTION_ID,
                Name = @NAME,
                Force = @FORCE,
                UsedForce = @USED_FORCE
                WHERE TemplateId = @TEMPLATE_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var unit in input.Units)
            {
                await UnitService.CreateTemplateUnitAsync(input, unit, dbController, cancellationToken);
            }
        }
    }
}