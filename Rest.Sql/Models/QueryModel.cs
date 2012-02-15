namespace Rest.Sql.Models
{
    public class QueryModel
    {
        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public string Where { get; set; }

        public string OrderBy { get; set; }
    }
}