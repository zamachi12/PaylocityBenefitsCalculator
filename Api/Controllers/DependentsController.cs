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
public class DependentsController : ControllerBase
{
    private readonly AppDbContext _context;
     public DependentsController(AppDbContext context){
        _context= context;
    }
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent= await _context.Dependents.
        Where(d => d.Id == id).Select(d => new GetDependentDto
        {
            Id = d.Id,
            FirstName= d.FirstName,
            LastName= d.LastName,
            DateOfBirth= d.DateOfBirth,
            Relationship= d.Relationship,
            
        }).FirstOrDefaultAsync();

        if (dependent == null)
        {
            return NotFound(new ApiResponse<GetDependentDto>
            {
                Success = false,
                Message = "Employee not found"
            });
        }
        

        return new ApiResponse<GetDependentDto>
        {
            Data = dependent,
            Success = true
        };
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _context.Dependents
        .Select(d=> new GetDependentDto{
            Id = d.Id,
            FirstName= d.FirstName,
            LastName= d.LastName,
            DateOfBirth= d.DateOfBirth,
            Relationship= d.Relationship,

        }).ToListAsync();

        return new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };
    }
}
