using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustoomerToken.Domain.Tokens;
using CustoomerToken.Infrastructure.Repositories;
using CustoomerToken.Models;
using CustoomerToken.Services.Employees;
using CustoomerToken.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CustoomerToken.Domain.Employees;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly TokenRepository _tokenRepository;
    private readonly IMapper _mapper;
    private readonly EmployeeService _employeeService;
    private readonly IPasswordHasher<Employee> _passwordHasher;

    public EmployeeController(TokenRepository tokenRepository, IMapper mapper,EmployeeService employeeService,IPasswordHasher<Employee> passwordHasher)
    {

        this._tokenRepository = tokenRepository;
        this._mapper = mapper;
        this._employeeService = employeeService;
        this._passwordHasher = passwordHasher;
    }
    [Authorize()]
    [HttpGet("user")]
    public IActionResult GetAsync()
    {
        var id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var employee = _employeeService.GetById(id);

        return Ok(employee);
    }

    [Authorize(policy: "enployeePolicy")]
    [HttpGet("token/list")]
    public IActionResult ListAsync([FromQuery] int pageSize = 10, [FromQuery] int pageNo = 1)
    {
        // var queryId = (QueryType)Convert.ToInt32(User.FindFirstValue("QueryId"));

        var tokensResult = _tokenRepository.GetUnResoved(QueryType.General, pageSize, pageNo);

        var result = _mapper.Map<PaginationResult<Token>, PaginationResult<TokenListModel>>(tokensResult);

        return Ok(result);
    }
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(EmployeeLoginModel employeeLoginModel)
    {
        if (ModelState.IsValid)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            var employee = _employeeService.GetUserCredential(employeeLoginModel.UserName);

            if (employee is not null && employee.Id > 0)
            {
                var passwordHashResult = _passwordHasher.VerifyHashedPassword(employee, employee.Password, employeeLoginModel.Password);

                if (passwordHashResult == PasswordVerificationResult.Success || passwordHashResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    LoginEmployee(employee);

                    //do some thing
                    return Ok();
                }
            }
        }

        return BadRequest();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register(RegistrationModel model)
    {
        if (ModelState.IsValid)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            var existinguser = _employeeService.GetUserCredential(model.UserName);

            if (existinguser is not null && existinguser?.Id > 0)
            {
                ModelState.AddModelError("UserError", $"User already exist with user name {model.UserName}");
                return BadRequest();
            }

            EmployeeCredential employeeCredential = new EmployeeCredential()
            {
                UserName = model.UserName,
                QueryId = model.Query
            };

            employeeCredential.Password = _passwordHasher.HashPassword(employeeCredential, model.Password);

            var employee = _employeeService.Create(employeeCredential);

            LoginEmployee(employee);

            return BadRequest();
        }

        return BadRequest();
    }


    private void LoginEmployee(Employee employee)
    {
        var principle = new ClaimsPrincipal();
        var identiity = new ClaimsIdentity(
            new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier,employee.Id.ToString()),
                        new Claim(ClaimTypes.Name,employee.UserName),
                        new Claim("QueryId",((int)employee.QueryId).ToString()),
                        new Claim(ClaimTypes.Role,"emp") }
            , "custome");

        principle.AddIdentity(identiity);

        HttpContext.SignInAsync(
            "CookieSchema",
            principle).GetAwaiter().GetResult();

    }
}