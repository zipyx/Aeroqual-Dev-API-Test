using ApiLibrary.Data;
using ApiLibrary.Interfaces;
using Microsoft.Extensions.Logging;

namespace ApiLibrary.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext _context;
        private readonly ILogger<ApiRepository> _logger;

        public UnitOfWork(ApiContext context, ILogger<ApiRepository> logger)
        {
            _context = context;
            _logger = logger;
            ApiRepository = new ApiRepository(context, logger);
        }

        public IApiRepository ApiRepository { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
