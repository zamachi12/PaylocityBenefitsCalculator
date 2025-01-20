using Api.Data;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context){
        _context= context;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Dependents)
            .Where(e => e.Id == id)
            .Select(e => new GetEmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Salary = e.Salary,
                DateOfBirth = e.DateOfBirth,
                Dependents = e.Dependents.Select(d => new GetDependentDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Relationship = Enum.Parse<Relationship>(d.Relationship.ToString()),
                    DateOfBirth = d.DateOfBirth
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (employee == null)
        {
            return NotFound(new ApiResponse<GetEmployeeDto>
            {
                Success = false,
                Message = "Employee not found"
            });
        }

        return new ApiResponse<GetEmployeeDto>
        {
            Data = employee,
            Success = true
        };
    }

        [SwaggerOperation(Summary = "Populate the database with sample data")]
    [HttpPost("populate")]
    public async Task<IActionResult> PopulateDatabase()
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new Employee
            {
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<Dependent>
                {
                    new Dependent
                    {
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Enum.Parse<Relationship>("Spouse"),
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new Dependent
                    {
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Enum.Parse<Relationship>("Child"),
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new Dependent
                    {
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Enum.Parse<Relationship>("Child"),
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new Employee
            {
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<Dependent>
                {
                    new Dependent
                    {
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Enum.Parse<Relationship>("DomesticPartner"),
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };

        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();

        return Ok("Database seeded successfully");
    }


    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
      public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _context.Employees
            .Include(e => e.Dependents)
            .Select(e => new GetEmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Salary = e.Salary,
                DateOfBirth = e.DateOfBirth,
                Dependents = e.Dependents.Select(d => new GetDependentDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Relationship = Enum.Parse<Relationship>(d.Relationship.ToString()),
                    DateOfBirth = d.DateOfBirth
                }).ToList()
            })
            .ToListAsync();

        return new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };
    }
}
