using DbController;
using System.Globalization;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class TemplateService : IModelService<Template, int, TemplateFilter>
    {
        private readonly UnitService _unitService;

        public TemplateService(UnitService unitService)
        {
            _unitService = unitService;
        }

        public async Task CreateAsync(Template input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO `tabletop`.`templates` 
                (
                `user_id`,
                `fraction_id`,
                `name`,
                `force`
                )
                VALUES
                (
                @USER_ID,
                @FRACTION_ID,
                @NAME,
                @FORCE
                ); {dbController.GetLastIdSql()}";

            input.TemplateId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            foreach(var unit in input.Units)
            {
                await _unitService.CreateTemplateUnitAsync(input, unit, dbController, cancellationToken);
            }
        }

        public async Task DeleteAsync(Template input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _unitService.DeleteTemplateUnitsAsync(input.TemplateId, dbController, cancellationToken);

            string sql = "DELETE FROM `tabletop`.`templates` WHERE `template_id` = @TEMPLATE_ID";

            await dbController.QueryAsync(sql, new
            {
                TEMPLATE_ID = input.TemplateId,
            }, cancellationToken);
            
        }

        public async Task<Template?> GetAsync(int templateId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM `tabletop`.`templates`  WHERE `template_id` = @TEMPLATE_ID";

            var template = await dbController.GetFirstAsync<Template>(sql, new
            {
                TEMPLATE_ID = templateId
            }, cancellationToken);

            return template;
        }

        public async Task<List<Template>> GetAsync(TemplateFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT * FROM `tabletop`.`templates`  WHERE `user_id` = @USER_ID");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$"  ORDER BY `name` ASC");
            sb.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sb.ToString();

            List<Template> list = await dbController.SelectDataAsync<Template>(sql, GetFilterParameter(filter), cancellationToken);

            foreach(var item in list)
            {
                item.Units = await _unitService.GetTemplateUnitsAsync(item.TemplateId, dbController, cancellationToken);
            }

            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(TemplateFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" },
                { "USER_ID", filter.UserId }
            };
        }

        public string GetFilterWhere(TemplateFilter filter)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sb.AppendLine(@" AND (UPPER(`name`) LIKE @SEARCHPHRASE)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(TemplateFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM `tabletop`.`templates`  WHERE `user_id` = @USER_ID");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public async Task UpdateAsync(Template input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _unitService.DeleteTemplateUnitsAsync(input.TemplateId, dbController, cancellationToken);

            string sql = @"UPDATE `tabletop`.`templates` SET
                `fraction_id` = @FRACTION_ID,
                `name` = @NAME,
                `force` = @FORCE
                WHERE `template_id` = @TEMPLATE_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var unit in input.Units)
            {
                await _unitService.CreateTemplateUnitAsync(input, unit, dbController, cancellationToken);
            }
        }
    }
}