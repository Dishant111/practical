using AutoMapper;
using CustoomerToken.Domain.Employees;
using CustoomerToken.Domain.Tokens;
using CustoomerToken.Infrastructure.Repositories;
using CustoomerToken.Models;
using CustoomerToken.Services;
using CustoomerToken.Services.Employees;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustoomerToken.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EmployeeController : Controller
    {
        private readonly NotifyServices _notifyServices;
        private readonly EmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Employee> _passwordHasher;
        private readonly TokenRepository _tokenRepository;

        public EmployeeController(
            NotifyServices notifyServices,
            IMapper mapper,
            IPasswordHasher<Employee> passwordHasher,
            EmployeeService employeeService,
            TokenRepository tokenRepository)
        {
            this._notifyServices = notifyServices;
            this._mapper = mapper;
            this._passwordHasher = passwordHasher;
            _employeeService = employeeService;
            _tokenRepository = tokenRepository;
        }

        [Authorize(policy: "enployeePolicy")]
        [HttpGet()]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("Employee/dynamic/dashBoard")]
        public IActionResult DashBoard([FromQuery] int pageSize = 10, [FromQuery] int pageNo = 1)
        {
            var queryId = (QueryType)Convert.ToInt32(User.FindFirstValue("QueryId"));

            var tokensResult = _tokenRepository.GetUnResoved(queryId, pageSize, pageNo);

            var result = _mapper.Map<PaginationResult<Token>, PaginationResult<TokenListModel>>(tokensResult);

            return Ok(result);
        }

        [HttpGet()]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Employee");
            }

            return View();
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeLoginModel employeeLoginModel)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Employee");
                }

                var employee = _employeeService.GetUserCredential(employeeLoginModel.UserName);

                if (employee is not null && employee.Id > 0)
                {
                    var passwordHashResult = _passwordHasher.VerifyHashedPassword(employee, employee.Password, employeeLoginModel.Password);

                    if (passwordHashResult == PasswordVerificationResult.Success || passwordHashResult == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        LoginEmployee(employee);

                        //do some thing
                        return RedirectToAction("Index", "Employee");
                    }
                }
                _notifyServices.NotifyError("userame or passowrd is invalid");

                ModelState.AddModelError("UserName", "userame or passowrd is invalid");

            }

            return View();
        }

        [HttpGet()]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Employee");
            }

            return View();
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Employee");
                }

                var existinguser = _employeeService.GetUserCredential(model.UserName);

                if (existinguser is not null && existinguser?.Id > 0)
                {
                    _notifyServices.NotifyError($"User already exist with user name {model.UserName}");

                    ModelState.AddModelError("UserError", $"User already exist with user name {model.UserName}");
                    return View();
                }
                EmployeeCredential employeeCredential = new EmployeeCredential()
                {
                    UserName = model.UserName,
                    QueryId = model.Query
                };

                employeeCredential.Password = _passwordHasher.HashPassword(employeeCredential, model.Password);

                var employee = _employeeService.Create(employeeCredential);

                LoginEmployee(employee);

                return RedirectToAction("Index", "Employee");
            }

            return View();
        }

        private void LoginEmployee(Employee employee)
        {
            var principle = new ClaimsPrincipal();
            var identiity = new ClaimsIdentity(
                new List<Claim>() {
                        new Claim(ClaimTypes.Name,employee.UserName),
                        new Claim("QueryId",((int)employee.QueryId).ToString()),
                        new Claim(ClaimTypes.Role,"emp") }
                , "custome");

            principle.AddIdentity(identiity);

            HttpContext.SignInAsync(
                "CookieSchema",
                principle).GetAwaiter().GetResult();

            _notifyServices.NotifySucess("Login Successfull");
        }

    }
}
