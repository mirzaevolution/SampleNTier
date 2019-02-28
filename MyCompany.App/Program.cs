using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyCompany.BusinessProcess.Repository;
using MyCompany.Models.DBModels;
using MyCompany.Models.ViewModels;
namespace MyCompany.App
{
    public class MyCompanyOperations
    {
        MyCompanyContext _context = new MyCompanyContext();


        public void AddEmployee()
        {
            EmployeeViewModel employeeViewModel = new EmployeeViewModel
            {
                FirstName = "Darren",
                LastName = "FX",
                Job = "Programmer",
                Salary = 1234
            };

            //map ke employee
            Employee employee = Mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);
            
            bool success = _context.AddEmployee(employee);
            Console.WriteLine($"Success ?  {success}");
        }
        public void GetAllEmployees()
        {
            List<Employee> employeesFromDB = _context.GetEmployees();

            List<EmployeeViewModel> employees = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(employeesFromDB);
            
            foreach(EmployeeViewModel employee in employees)
            {
                Console.WriteLine($"ID: {employee.ID}, First Name: {employee.FirstName}, Last Name: {employee.LastName}, Full Name: {employee.FullName}, Salary: {employee.Salary}, Total Salary/Year: {employee.TotalSalaryPerYear}, Job: {employee.Job}");
            }
        }
        public void GetEmployeeByName(string name)
        {

            List<Employee> employeesFromDB = _context.GetEmployeesByName(name);

            List<EmployeeViewModel> employees = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(employeesFromDB);

            foreach (EmployeeViewModel employee in employees)
            {
                Console.WriteLine($"ID: {employee.ID}, First Name: {employee.FirstName}, Last Name: {employee.LastName}, Full Name: {employee.FullName}, Salary: {employee.Salary}, Total Salary/Year: {employee.TotalSalaryPerYear}, Job: {employee.Job}");
            }
        }
        public void GetEmployeeId(int id)
        {

            Employee employeeFromDB = _context.GetEmployeeById(id);

            EmployeeViewModel employee = Mapper.Map<Employee, EmployeeViewModel>(employeeFromDB);
            if(employee==null)
            {
                Console.WriteLine("Data not found");
            }
            else
            {
                Console.WriteLine($"ID: {employee.ID}, First Name: {employee.FirstName}, Last Name: {employee.LastName}, Full Name: {employee.FullName}, Salary: {employee.Salary}, Total Salary/Year: {employee.TotalSalaryPerYear}, Job: {employee.Job}");
            }
        }
        public void UpdateEmployee(EmployeeViewModel employeeViewModel)
        {
            Employee employee = Mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);
            
            if(_context.UpdateEmployee(employee))
            {
                Console.WriteLine("Data has been updated successfully");
            }
            else
            {
                Console.WriteLine("Data failed to update");
            }
        }
    }


    class Program
    {
        public static void RegisterMapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Employee, EmployeeViewModel>()
                .ForMember(member => member.FullName, (options) =>
                {
                    options.ResolveUsing<string>(emp => emp.FirstName + " " + emp.LastName);
                })
                .ForMember(member => member.TotalSalaryPerYear, (options) =>
                {
                    options.ResolveUsing<decimal>(emp => emp.Salary * 12);
                });

                config.CreateMap<EmployeeViewModel, Employee>();
            });
        }
      
        static void Main(string[] args)
        {
            RegisterMapper();

            MyCompanyOperations myCompanyOperations = new MyCompanyOperations();

            //Get All Data
            myCompanyOperations.GetAllEmployees();
            //myCompanyOperations.AddEmployee();

        }
    }
}
