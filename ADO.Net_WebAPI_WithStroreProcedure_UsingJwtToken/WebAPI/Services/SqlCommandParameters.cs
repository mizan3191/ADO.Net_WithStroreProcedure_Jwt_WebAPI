namespace WebAPI.Services
{
    public class SqlCommandParameters
    {
        public List<SqlCommandParameter> List { get; set; } = new List<SqlCommandParameter>();

        public void Add(string Name, object value, bool nullable = false)
        {
            if (nullable || value != null)
            {
                List.Add(new SqlCommandParameter(Name, value));
            }
        }

        public void AddAll(List<(string name, object value)> parameter)
        {
            parameter.ForEach(param => Add(param.name, param.value));
        }
    }
}
