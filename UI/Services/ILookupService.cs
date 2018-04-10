using System.Collections.Generic;
using Model;

namespace UI.Services
{
    public interface ILookupService
    {
        IEnumerable<LookupItem> GetAllCompaniesLookup();
        IEnumerable<LookupItem> GetAllEmployeeLookup();
        LookupItem GetCompanyByIdLookup(int companyId);
        LookupItem GetEmployeeByIdLookup(int employeeId);
    }
}