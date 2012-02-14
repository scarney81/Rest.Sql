using System.Linq;
using System.Web.Mvc;
using Rest.Sql.Data;

namespace Rest.Sql.Controllers
{
    public class QueryController : Controller
    {
        private const string ConnectionStringName = "SqlStore";

        [HttpGet]
        public JsonResult Index(string table, int pageSize = 25, int page = 1, string where = null, string orderBy = null)
        {
            var model = new DynamicModel(ConnectionStringName, table, "Id");
            var results = model.All(where, orderBy).Skip((page - 1) * pageSize).Take(pageSize);

            var response = new
            {
                page,
                pageSize,
                query = new { table, where, orderBy },
                results
            };

            return JsonGet(response);
        }

        private JsonResult JsonGet(object model)
        {
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}