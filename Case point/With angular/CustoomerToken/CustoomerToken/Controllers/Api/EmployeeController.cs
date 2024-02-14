using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustoomerToken.Domain.Tokens;
using CustoomerToken.Infrastructure.Repositories;
using CustoomerToken.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class EmployeeController : ControllerBase
{
    private readonly TokenRepository _tokenRepository;
    private readonly IMapper _mapper;

    public EmployeeController(TokenRepository tokenRepository, IMapper mapper)
    {

        this._tokenRepository = tokenRepository;
        this._mapper = mapper;
    }

    [HttpGet("token/list")]
    public IActionResult ListAsync([FromQuery] int pageSize = 10, [FromQuery] int pageNo = 1)
    {
        // var queryId = (QueryType)Convert.ToInt32(User.FindFirstValue("QueryId"));

        var tokensResult = _tokenRepository.GetUnResoved(QueryType.General, pageSize, pageNo);

        var result = _mapper.Map<PaginationResult<Token>, PaginationResult<TokenListModel>>(tokensResult);

        return Ok(result);
    }
}