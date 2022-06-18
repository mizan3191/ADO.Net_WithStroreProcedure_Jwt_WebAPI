using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly string _connectionString;

        public EmployeesService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbConnection");
        }

        public void AddEmployee(Employee employee)
        {
            if(employee == null)
            {
                throw new ArgumentNullException();
            }

            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            var cmdParameter =  new List<SqlCommandParameter>();
            cmdParameter.Add(SqlCommandParameter.AddParameters("@Name", employee.Name));
            cmdParameter.Add(SqlCommandParameter.AddParameters("@Address", employee.Address));

            foreach(var item in cmdParameter.ToList())
            {
                if(item.ParameterValue == DBNull.Value)
                {
                    cmdParameter.Remove(item);
                }
            }

            var execute = ExecuteScalarStoreProcedure("Add_Employee", cmdParameter, sqlConnection);
            employee.Id = Convert.ToInt32(execute);
        }

        public List<Employee> GetEmployees()
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            var paramList = new SqlCommandParameters();
            var resultData = ExecuteDataTableStoreProcedure("GetAll", paramList.List, sqlConnection);
            var jsonResult =  JsonConvert.DeserializeObject<List<Employee>>
                (JsonConvert.SerializeObject(resultData));

            return jsonResult;
        }

        public List<Employee> GetEmployeeById(int id)
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            var paramList = new SqlCommandParameters();

            paramList.Add("@id", id);
            var resultdata = ExecuteDataTableStoreProcedure("GetById", paramList.List, sqlConnection);
            var jsonResult = JsonConvert.DeserializeObject<List<Employee>>
                (JsonConvert.SerializeObject(resultdata));

            return jsonResult;
        }

        public static object ExecuteScalarStoreProcedure(
            string sp_name, List<SqlCommandParameter> paramList, SqlConnection sqlConnection)
        {
            object value = null;
            var cmd = GetSqlCommand(sp_name, paramList, sqlConnection);

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                value = cmd.ExecuteScalar();
            }
            catch(Exception ex)
            {
                value = ex.Message;
            }
            finally
            {
                cmd.Dispose();
                cmd.Connection.Dispose();
            }


            return value;
        }

        public static DataTable ExecuteDataTableStoreProcedure(
            string sp_name, List<SqlCommandParameter> paramList, SqlConnection sqlConnection)
        {
            if(sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }

            var cmd = new SqlCommand(sp_name);
            cmd.Connection = sqlConnection;

            if(paramList != null)
            {
                if(paramList.Count > 0)
                {
                    foreach(var param in paramList)
                    {
                        cmd.Parameters.AddWithValue(param.ParameterName, param.ParameterValue);
                    }
                }
            }

            var dt = new DataTable();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
            }

            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                sqlConnection.Close();
            }

            return dt;
        }

        public static SqlCommand GetSqlCommand(
            string sp_name, List<SqlCommandParameter> paramList, SqlConnection sqlConnection)
        {
            try
            {
                if(sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
            }

            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            SqlCommand sqlCommand = new SqlCommand(sp_name);
            sqlCommand.Connection = sqlConnection;

            if(paramList != null)
            {
                foreach(var param in paramList)
                {
                    sqlCommand.Parameters.AddWithValue(param.ParameterName, param.ParameterValue);
                }
            }

            return sqlCommand;
        }

        public void UpdateEmployee(Employee employee, int id)
        {
            if(employee != null && id>0)
            {
                EditEmployee(employee, "Update_Employee", id);
            }
        }

        public void DeleteEmployeeById(int id)
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            var paramList = new SqlCommandParameters();
            paramList.Add("@id", id);

            if (id > 0)
            {
                var resultdata = ExecuteDataTableStoreProcedure("Delete", paramList.List, sqlConnection);
            }
        }

        public bool EditEmployee(Employee employee, string sp_name, int id)
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            var cmdParamsList = new List<SqlCommandParameter>();

            cmdParamsList.Add(SqlCommandParameter.AddParameters("Id", id));
            cmdParamsList.Add(SqlCommandParameter.AddParameters("@Name", employee.Name));
            cmdParamsList.Add(SqlCommandParameter.AddParameters("@Address", employee.Address));

            foreach(var item in cmdParamsList.ToList())
            {
                if(item.ParameterValue == DBNull.Value)
                {
                    cmdParamsList.Remove(item);
                }
            }
            SqlCommand sqlCommand = new SqlCommand(sp_name);
            sqlCommand.Connection = sqlConnection;

            if(cmdParamsList != null)
            {
                if(cmdParamsList.Count > 0)
                {
                    foreach(var item in cmdParamsList.ToList())
                    {
                        sqlCommand.Parameters.AddWithValue(item.ParameterName, item.ParameterValue);
                    }
                }
            }

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.ExecuteNonQuery();

            return true;
        }
    }
}