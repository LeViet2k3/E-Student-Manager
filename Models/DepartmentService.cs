namespace StudentApp.Models;

public interface IDepartmentService
{
    List<Department> GetDepartments();
}

public class DepartmentService(DataContext dataContext) : IDepartmentService
{
    private readonly DataContext _dataContext = dataContext;

    public List<Department> GetDepartments()
    {
        return _dataContext.Departments.ToList();
    }
}
