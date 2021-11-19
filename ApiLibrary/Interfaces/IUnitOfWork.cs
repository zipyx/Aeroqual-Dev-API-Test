using System;

namespace ApiLibrary.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApiRepository ApiRepository { get; }
    }
}
