using CustoomerToken.Infrastructure.Repositories;
using CustoomerToken.Models;
using CustoomerToken.Services.Exceptios;
using NuGet.Packaging;

namespace CustoomerToken.Services.Tokens
{

    public class TokenService
    {
        private readonly TokenRepository _tokenRepository;

        public TokenService(TokenRepository tokenRepository)
        {
            this._tokenRepository = tokenRepository;
        }

        public Token GetByID(int id)
        {
            return _tokenRepository.GetById(id);
        }

        public Token Create(Token token)
        {
            return _tokenRepository.Create(token);
        }

        public Token Update(Token token)
        {
            return _tokenRepository.Update(token);
        }

        public void Delete(int id)
        {
            _tokenRepository.Delete(id);
        }

        public PaginationResult<Token> GetUnResolved(int pageSize, int pageNo)
        {
            return _tokenRepository.GetUnResoved(pageSize, pageNo);
        }

        public void UpdateProcessing(int id)
        {
            var token = _tokenRepository.GetById(id);

            if (token is null || token.Id <= 0)
            {
                throw new EmployeeValidationException("employee does ot exists");
            }

            if (token.StatusId == Domain.Tokens.QueryStatus.Pending)
            {
                token.StatusId = Domain.Tokens.QueryStatus.Processing;
                _tokenRepository.Update(token);
            }
        }

        public void UpdatePendig(int id)
        {
            var token = _tokenRepository.GetById(id);

            if (token is null || token.Id <= 0)
            {
                throw new EmployeeValidationException("employee does ot exists");
            }

            if (token.StatusId == Domain.Tokens.QueryStatus.Processing)
            {
                token.StatusId = Domain.Tokens.QueryStatus.Pending;
                _tokenRepository.Update(token);
            }
        }

        internal void UpdateResolve(ResolveToken model)
        {
            var token = _tokenRepository.GetById(model.Id);

            if (token is null || token.Id <= 0)
            {
                throw new EmployeeValidationException("employee does ot exists");
            }

            if (token.StatusId == Domain.Tokens.QueryStatus.Processing 
                || token.StatusId == Domain.Tokens.QueryStatus.Pending)
            {
                token.StatusId = Domain.Tokens.QueryStatus.Resolved;
                token.Phone = model.PhoneNumber;
                token.Address = model.Address;

                _tokenRepository.Update(token);
            }

        }
    }
}
