using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                throw new HttpException(404, "Table name: " + table + " not found.");

            var page = query.Page ?? 1;
            var pageSize = query.PageSize ?? 25;
            var columns = query.GetColumns();

            var model = CreateDynamicModel(table);

            var everything = model.All(query.Where, query.OrderBy, columns: columns);
            // ReSharper disable PossibleMultipleEnumeration
            var total = everything.Count();
            var results = everything.Skip((page - 1) * pageSize).Take(pageSize);
            // ReSharper restore PossibleMultipleEnumeration

            var response = CreateResponse(table, query, results, columns, page, pageSize, total);
            return JsonContent(response);
        }

        private static object CreateResponse(string table, QueryModel query, IEnumerable<dynamic> results, string columns, int page, int pageSize, int total)
        {
            return new
                       {
                           page,
                           pageSize,
                           total,
                           query = new
                                       {
                                           table,
                                           columns,
                                           where = query.Where ?? string.Empty,
                                           orderby = query.OrderBy ?? string.Empty
                                       },
                           results
                       };
        }

        private ContentResult JsonContent(object model)
        {
            var serialized = JsonConvert.SerializeObject(model, Formatting.None);
            return Content(serialized, "applicaion/javascript");
        }

        private static DynamicModel CreateDynamicModel(string table)
        {
            return new DynamicModel(ConnectionStringName, table);
        }
    }
}