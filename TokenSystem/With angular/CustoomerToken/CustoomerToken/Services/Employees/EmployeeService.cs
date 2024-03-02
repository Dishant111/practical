﻿using CustoomerToken.Domain.Employees;
using CustoomerToken.Infrastructure.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CustoomerToken.Services.Employees
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        public Employee GetById(int id)
        {
            return _employeeRepository.GetById(id);
        }
        
        public EmployeeCredential GetUserCredential(string userName)
        {
            return _employeeRepository.GetUserCredential(userName);
        }

        public Employee Create(EmployeeCredential token)
        {
            return _employeeRepository.Create(token);
        }

        public void Delete(int id)
        {
            _employeeRepository.Delete(id);
        }
    }
}
