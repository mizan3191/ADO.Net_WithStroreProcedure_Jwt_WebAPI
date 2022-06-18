namespace WebAPI.Services
{
    public class SqlCommandParameter
    {
        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }

        public SqlCommandParameter(string name, object value)
        {
            ParameterName = name;
            ParameterValue = value == null ? DBNull.Value : value;
        }

        public static SqlCommandParameter AddParameters(string parameterName, object parameterValue)
        {

            return new SqlCommandParameter(parameterName, parameterValue);
        }
    }
}
