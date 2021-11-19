using ApiLibrary.Data;
using ApiLibrary.Interfaces;
using ApiLibrary.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ApiLibrary.Repositories
{
    public class ApiRepository : Repository<Person>, IApiRepository
    {

        private readonly ILogger<ApiRepository> _logger;

        public ApiRepository(ApiContext context, ILogger<ApiRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public People GetPeople()
        {
            return GetJsonData();
        }

        public People GetSearchPeople(string name)
        {
            var people = GetPeople();
            People peopleFound = new People();
            foreach (Person person in people.people)
            {
                if (person.Name.ToLower().Contains(name.ToLower()))
                {
                    peopleFound.people.Add(person);
                    _logger.LogInformation(
                        string.Format("Search name : {0} is a substring of person {1}",
                        name, person.Name));
                }
            }
            return peopleFound;
        }

        public Person GetPerson(int id)
        {
            var jsonData = GetJsonData();
            foreach (var person in jsonData.people)
            {
                if (person.Id == id)
                {
                    return person;
                }
            }
            _logger.LogInformation(string.Format(
                "Cannot locate person with ID : {0}", id));
            return null;
        }

        public Task CreatePerson(Person person)
        {
            var people = GetPeople();
            var incrementId = IdAutoIncrementer();

            if (people.people.Any(x => x.Id == person.Id))
            {
                _logger.LogInformation(string.Format(
                    "ID : {0} already exists/not provided. Assigning ID {1} to Person.",
                    person.Id, incrementId));
            }

            person.Id = incrementId;
            people.people.Add(person);
            WriteJsonData(people);

            return Task.CompletedTask;
        }

        public Task DeletePerson(int id)
        {
            People people = GetPeople();
            var person = GetPerson(id);
            people.people.RemoveWhere(x => x.Id == person.Id);
            _logger.LogInformation(string.Format(
                "Person : {0} successfully removed.",
                person.Name));
            WriteJsonData(people);

            return Task.CompletedTask;
        }

        public Task UpdatePerson(Person person, int id)
        {

            var people = GetPeople();

            foreach (var profile in people.people)
            {
                // Existing ID matches request ID to update
                if (profile.Id == id)
                {
                    // Exising ID matches Object ID to change
                    if (profile.Id == person.Id)
                    {
                        // Increment ID to avoid duplicates
                        profile.Name = person.Name;
                        profile.Age = person.Age;

                        _logger.LogInformation(string.Format(
                            "Update successful, ID : {0} matches existing record.",
                            person.Id));

                        WriteJsonData(people);
                    }

                    // Object ID not provided
                    // Object ID request not existing in database.
                    profile.Name = person.Name;
                    profile.Age = person.Age;

                    _logger.LogInformation(string.Format(
                        "Update successful, ID : {0}.",
                        person.Id));

                    WriteJsonData(people);
                }
            }

            return Task.CompletedTask;
        }

        private People GetJsonData()
        {
            string fileName = "data.json";
            string jsonString = System.IO.File.ReadAllText(fileName);
            People people = JsonConvert.DeserializeObject<People>(jsonString);

            return people;
        }

        private void WriteJsonData(People deserializedObject)
        {
            string output = JsonConvert.SerializeObject(deserializedObject, Formatting.Indented);
            System.IO.File.WriteAllText("data.json", output);
        }

        private int IdAutoIncrementer()
        {
            return GetPeople().people.Max(x => x.Id) + 1;
        }
    }
}
