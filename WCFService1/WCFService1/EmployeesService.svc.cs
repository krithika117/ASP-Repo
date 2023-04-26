using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCFService1.Models;

namespace WCFService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeesService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmployeesService.svc or EmployeesService.svc.cs at the Solution Explorer and start debugging.
    public class EmployeesService : IEmployeesService
    {
        public void AddEmployees(int Id, string Name, int deptId)
        {
            employee emp = new employee();
            TestDBEntities testDBEntities = new TestDBEntities();
            testDBEntities.employees.Add(emp);
            testDBEntities.SaveChanges();

        }
    }
}
