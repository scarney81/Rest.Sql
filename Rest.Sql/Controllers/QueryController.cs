using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Rest.Sql.Data;
using Rest.Sql.Models;

namespace Rest.Sql.Controllers
{
    public class QueryController : Controller
    {
        private const string ConnectionStringName = "SqlStore";

        [HttpGet]
        public ContentResult Table(string table, QueryModel query)
        {
            if (string.IsNullOrWhiteSpace(table))
                return JsonContent("no table specified");

            var page = query.Page ?? 1;
            var pageSize = query.PageSize ?? 25;

            var model = CreateDynamicModel(table);

            var everything = model.All(query.Where, query.OrderBy);
            var total = everything.Count();
            var results = everything.Skip((page - 1) * pageSize).Take(pageSize);

            var response = new {page, pageSize, total, query = new {table, query.Where, query.OrderBy}, results};
            return JsonContent(response);
        }

        private ContentResult JsonContent(object model)
        {
            var serialized = JsonConvert.SerializeObject(model, Formatting.None);
            return Content(serialized);
        }

        private static DynamicModel CreateDynamicModel(string table)
        {
            return new DynamicModel(ConnectionStringName, table);
        }
    }
}