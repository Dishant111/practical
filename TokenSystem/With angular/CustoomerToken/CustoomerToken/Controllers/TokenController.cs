using AutoMapper;
using CustoomerToken.Models;
using CustoomerToken.Services;
using CustoomerToken.Services.Exceptios;
using CustoomerToken.Services.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustoomerToken.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TokenController : Controller
    {
        private readonly NotifyServices _notifyServices;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public TokenController(NotifyServices notifyServices, TokenService tokenService, IMapper mapper)
        {
            this._notifyServices = notifyServices;
            this._tokenService = tokenService;
            this._mapper = mapper;
        }

        // GET: TokenController
        public ActionResult Index([FromQuery] int pageSize = 10, [FromQuery] int pageNo = 1)
        {
            var tokensResult = _tokenService.GetUnResolved(pageSize, pageNo);

            var result = _mapper.Map<PaginationResult<Token>, PaginationResult<TokenListModel>>(tokensResult);

            _notifyServices.Notifications.Add(new Notification(NotificationType.Sucess, "this is sucess", "Header"));

            return View(result);
        }

        [Authorize(policy: "enployeePolicy")]
        [HttpGet("dynamic/details")]
        public ActionResult Details()
        {
            return View();
        }

        [HttpGet("dynamic/dashBoard")]
        public IActionResult DashBoard([FromQuery] int pageSize = 10, [FromQuery] int pageNo = 1)
        {
            var tokensResult = _tokenService.GetUnResolved(pageSize, pageNo);

            var result = _mapper.Map<PaginationResult<Token>, PaginationResult<TokenListModel>>(tokensResult);
            return Ok(result);
        }

        [Authorize(policy: "enployeePolicy")]
        [HttpPut("Process/{id:int}")]
        public IActionResult Processing([FromRoute] int id)
        {
            try
            {
                _tokenService.UpdateProcessing(id);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize(policy: "enployeePolicy")]
        [HttpPut("Pending/{id:int}")]
        public IActionResult Pending([FromRoute] int id)
        {
            try
            {
                _tokenService.UpdatePendig(id);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize(policy: "enployeePolicy")]
        [HttpPost("Resolve")]
        public IActionResult Resolve([FromBody] ResolveToken model)
        {
            try
            {
                _tokenService.UpdateResolve(model);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize(policy: "enployeePolicy")]
        [HttpGet("Token/{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            Token token = null;
            try
            {
                token = _tokenService.GetByID(id);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(token);
        }

        // GET: TokenController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("CreateToken")]
        public IActionResult CreateToken([FromBody] TokenCreateModel model)
        {
            try
            {
                var token = new Token()
                {
                    QueryId = model.Query,
                    StatusId = Domain.Tokens.QueryStatus.Pending,
                };

                var tokensResult = _tokenService.Create(token);

                return Ok(tokensResult);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: TokenController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TokenController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
