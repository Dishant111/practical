using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CustoomerToken.Models;
using CustoomerToken.Services.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustoomerToken.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public TokenController(TokenService tokenService, IMapper mapper)
        {
            this._tokenService = tokenService;
            this._mapper = mapper;
        }

        [HttpGet("unresolved/list")]
        public IActionResult DashBoard([FromQuery] int pageSize = 10, [FromQuery] int pageNo = 1)
        {
            var tokensResult = _tokenService.GetUnResolved(pageSize, pageNo);

            var result = _mapper.Map<PaginationResult<Token>, PaginationResult<TokenListModel>>(tokensResult);

            return Ok(result);
        }

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
    }
}