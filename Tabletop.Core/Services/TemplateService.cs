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
        }

        public async Task DeleteAsync(Template input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
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
            string sql = "SELECT * FROM `tabletop`.`templates`";

            var list = await dbController.SelectDataAsync<Template>(sql, cancellationToken: cancellationToken);

            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(TemplateFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" }
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

        public Task<int> GetTotalAsync(TemplateFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Template input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE `tabletop`.`templates` SET
                `name` = @NAME,
                `force` = @FORCE
                WHERE `template_id` = @TEMPLATE_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);
        }
    }
}
