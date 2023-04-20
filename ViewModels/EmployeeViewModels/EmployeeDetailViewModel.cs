using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.ViewModels.EmployeeViewModels;

public class EmployeeDetailViewModel : Employee
{
    public List<KeyValuePair<int,int>> Score { get; set; }
}