using ApiLibrary.Data;
using ApiLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiLibrary.Interfaces
{
    public interface IApiRepository : IRepository<Person>
    {
        // Sync Methods
        People GetPeople();
        People GetSearchPeople(string name);
        Person GetPerson(int id);
        Task CreatePerson(Person person);
        Task DeletePerson(int id);
        Task UpdatePerson(Person person, int id);

    }
}
