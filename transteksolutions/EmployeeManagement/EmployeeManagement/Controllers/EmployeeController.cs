using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IDesignationRepository _designationRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper, IDesignationRepository designationRepository)
        {
            this._employeeRepository = employeeRepository;
            this._mapper = mapper;
            this._designationRepository = designationRepository;
        }
        // GET: EmployeeController
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var employeelist = _employeeRepository.GetEmployeeListing();
            var result = _mapper.Map<List<EmployeeDetail>, List<EmployeeListModel>>(employeelist);
            return new JsonResult(result);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        [HttpGet]
        public ActionResult Create()
        {
            EmployeeCerateModel model = new EmployeeCerateModel();
            var designations = _designationRepository.GetAllDesignations();
            model.Designations = _mapper.Map<List<Designation>, List<DesignationModel>>(designations);

            return View(model);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeCerateModel model)
        {
            try
            {
                var employeelist = _employeeRepository.CreateEmployee(model.EmployeeName, model.Salary, model.DesignationId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            EmployeeCerateModel model = new EmployeeCerateModel();
            var employeeDetail = _employeeRepository.GetEmployeeById(id);
            model.Id = id;
            model.EmployeeName = employeeDetail.EmployeeName;
            model.Salary = employeeDetail.Salary;
            model.DesignationId = employeeDetail.DesignationId;

            var designations = _designationRepository.GetAllDesignations();
            model.Designations = _mapper.Map<List<Designation>, List<DesignationModel>>(designations);

            return View(model);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeCerateModel model)
        {
            try
            {
                _employeeRepository.UpdateEmployee(id, model.EmployeeName, model.Salary, model.DesignationId);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _employeeRepository.DeleteEmployee(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
