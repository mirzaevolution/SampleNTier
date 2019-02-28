using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using MyCompany.Models.DBModels;
using System.Configuration;

namespace MyCompany.BusinessProcess.Repository
{
    public class MyCompanyContext
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MyCompanyContext"].ConnectionString;
        }
        public List<Employee> GetEmployees()
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                List<Employee> employees = sqlConnection
                    .Query<Employee>("SELECT ID, FirstName, LastName, Job, Salary FROM dbo.Employees").AsList();
                return employees;
            }
        }
        public Employee GetEmployeeById(int id)
        {
            using(SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                Employee employee = sqlConnection
                    .QuerySingleOrDefault<Employee> //bisa gunain QuerySingle tapi gak safe karena klo data empty bisa throw error
                    (
                        "SELECT ID, FirstName, LastName, Job, Salary FROM dbo.Employees " +
                        "WHERE ID=@ID",
                        new 
                        {
                            ID = id //parameternya
                        }
                    );
                return employee;
            }
        }
        public List<Employee> GetEmployeesByName(string name)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                List<Employee> employees = sqlConnection
                    .Query<Employee>
                    (
                        "SELECT ID, FirstName, LastName, Job, Salary FROM dbo.Employees " +
                        "WHERE FirstName LIKE '%@keyword%' OR LastName LIKE '%keyword%'",
                        new
                        {
                            keyword = name //parameternya
                        }
                    ).AsList();
                return employees;
            }
        }
        public bool AddEmployee(Employee employee)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                int addRows =  sqlConnection.Execute(
                    "INSERT INTO dbo.Employees(FirstName,LastName,Job,Salary) " +
                    "VALUES (@FirstName,@LastName,@Job,@Salary);",
                    new
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,    
                        Job = employee.Job,
                        Salary = employee.Salary
                    });
                return addRows > 0 ? true : false;
            }
        }
        public bool UpdateEmployee(Employee employee)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                int updatedRows = sqlConnection.Execute(
                    "UPDATE dbo.Employees " +
                    "SET FirstName=@FirstName, " +
                    "LastName=@LastName, " +
                    "Job=@Job, " +
                    "Salary=@Salary WHERE ID=@ID",
                    new
                    {
                        ID = employee.ID,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Job = employee.Job,
                        Salary = employee.Salary
                    });
                return updatedRows > 0 ? true : false;
            }
        }
        public void DeleteEmployee(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Execute("DELETE FROM dbo.Employees WHERE ID=@ID", new
                {
                    ID = id
                });
            }
        }
    }
}
