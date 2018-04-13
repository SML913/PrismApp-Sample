using System.Collections.Generic;
using Model;

namespace UI.Services
{
    public interface ICommonService
    {
        bool IsCompanyReferenced(int companyId);
        IEnumerable<LookupItem> GetAllCompaniesLookup();
        IEnumerable<LookupItem> GetAllEmployeeLookup();
        LookupItem GetCompanyByIdLookup(int companyId);
        LookupItem GetEmployeeByIdLookup(int employeeId);
    }
}