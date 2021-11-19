using ApiTest.Controllers;
using ApiLibrary.Interfaces;
using ApiLibrary.Models;
using System.Linq;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI
{
    public class PeopleControllerTests : ControllerBase
    {

        [Fact]
        public void TestGetPeopleCount_GetACountOfRecordsStored_ReturnAMatchingCount()
        {
            // Arrange
            var mockRepository = new Mock<IUnitOfWork>();
            mockRepository.Setup(repo => repo.ApiRepository.GetPeople())
                .Returns(TestGetPeople);
            var controller = new PeopleController(mockRepository.Object, logger: null);

            // Act
            var result = controller.GetPeople();

            //Assert (expected, output)
            Assert.NotEqual(17, result.people.Count);
            
        }

        [Fact]
        public void GetPeople_QueryThePeopleTableStoredInJsonFile_ReturnsMatchingRecordsFromDifferentMethods()
        {
            // Arrange
            var mockRepository = new Mock<IUnitOfWork>();
            mockRepository.Setup(repo => repo.ApiRepository.GetPeople())
                .Returns(TestGetPeople);
            var controller = new PeopleController(mockRepository.Object, logger: null);

            // Act
            var people = controller.GetPeople();

            //Assert
            Assert.IsType<People>(people);
            
        }

        [Fact]
        public void GetPerson_GetAPersonWithExistingID_ReturnsMatchingRecordByName()
        {
            // Arrange
            int id = 10;
            var mockRepository = new Mock<IUnitOfWork>();
            mockRepository.Setup(repo => repo.ApiRepository.GetPerson(id))
                .Returns(TestGetPerson);
            var controller = new PeopleController(mockRepository.Object, logger: null);

            // Act
            var result = controller.GetPerson(id);

            // Assert
            var typeResult = Assert.IsType<Person>(result);
            Assert.Equal("Robert", typeResult.Name);
        }

        [Fact]
        public void GetSearchPeople_FindARecordIfGivenSubstring_ReturnsAMatchingRecordByName()
        {
            // Arrange
            string name = "roo";
            var mockRepository = new Mock<IUnitOfWork>();
            mockRepository.Setup(repo => repo.ApiRepository.GetSearchPeople(name))
                .Returns(TestGetSearchPeople);
            var controller = new PeopleController(mockRepository.Object, logger: null);

            // Act
            var testName = "Roopesh";
            People people = new People();
            Person expectedResult = new Person()
            {
                Id = 2,
                Name = testName,
                Age = 13
            };
            people.people.Add(expectedResult);
            var person = controller.GetSearchPeople(name);

            // Assert
            var result = Assert.IsType<People>(person);
            Assert.Equal(people.people.Any<Person>(x => x.Name == testName), 
                result.people.Any<Person>(x => x.Name == testName));
        }

        [Fact]
        public async void CreatePerson_MockCreatePersonWithoutAddingID_ReturnsEqualIfRecordMatchesJsonObjectOutput()
        {
            // Arrange
            Person person = new Person()
            {
                Name = "Breznk",
                Age = 55
            };

            var mockRepository = new Mock<IUnitOfWork>();
            mockRepository.Setup(repo => repo.ApiRepository.CreatePerson(person))
                .Callback(() => { })
                .Returns(TestCreatePerson);
            var controller = new PeopleController(mockRepository.Object, logger: null);

            // Act
            await controller.CreatePerson(person);
            var output = TestGetPerson(21);

            // Assert
            Assert.Equal(person.Name, output.Name); 

        }

        [Fact]
        public async void DeletePerson_MockPersonDelete_ReturnsFailedDueToRecordBeingDeleted()
        {
            // Arrange
            int id = 20;
            var mockRepository = new Mock<IUnitOfWork>();
            var mockPerson = new Mock<Person>();
            var mockLogger = new Mock<ILogger<PeopleController>>();
            mockRepository.Setup(repo => repo.ApiRepository.DeletePerson(id))
                .Callback(() => { })
                .Returns(TestDeletePerson);
            var controller = new PeopleController(mockRepository.Object, mockLogger.Object);

            // Act
            var personExists = TestGetPerson(id);
            await controller.DeletePerson(id);
            var personDoesNotExists = TestGetPerson(id);

            // Assert
            Assert.NotEqual(personExists, personDoesNotExists);

        }

        [Fact]
        public async void UpdatePerson_MockDataUpdate_ReturnsMatchingDataRecordFromInputAndOutput()
        {
            // Arrange
            int id = 20;
            Person person = new Person()
            {
                Id = id,
                Name = "Jack and Jill",
                Age = 14
            };
            var mockRepository = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILogger<PeopleController>>();
            mockRepository.Setup(repo => repo.ApiRepository.UpdatePerson(person, id))
                .Callback(() => { })
                .Returns(TestUpdatePerson);
            var controller = new PeopleController(mockRepository.Object, mockLogger.Object);

            // Act
            await controller.UpdatePerson(person, id);
            var result = TestGetPerson(id);

            // Assert
            Assert.Equal(person.Name, result.Name);

        }

        private People TestGetPeople()
        {
            People people = TestGetJsonData();

            return people;
        }

        private Person TestGetPerson(int id)
        {

            var people = TestGetPeople();

            foreach (Person person in people.people)
            {
                if (person.Id == id)
                {
                    return person;
                }
            }

            return new Person()
            {
                Id = id,
                Name = "Failed",
                Age = 55
            };
        }

        private People TestGetSearchPeople(string name="")
        {

            People result = new People();
            People people = TestGetJsonData();

            foreach (Person person in people.people)
            {
                if (person.Name.ToLower().Contains(name.ToLower()))
                {
                    result.people.Add(person);
                }
            }

            return result;
        }

        private async Task<IActionResult> TestCreatePerson(Person person)
        {

            var result = TestGetJsonData();
            person.Id = IdAutoIncrementer();
            result.people.Add(person);
            TestWriteJsonData(result);
            return Ok(result);
        }

        private async Task<IActionResult> TestDeletePerson(int id)
        {

            var people = TestGetJsonData();
            people.people.RemoveWhere(person => person.Id == id);
            TestWriteJsonData(people);
            return Ok(people);
        }

        private async Task<IActionResult> TestUpdatePerson(Person person, int id)
        {

            var people = TestGetJsonData();
            foreach (Person profile in people.people)
            {
                if (profile.Id == id)
                {
                    profile.Name = person.Name;
                    profile.Age = person.Age;
                    TestWriteJsonData(people);
                    return Ok();
                }
            }
            return BadRequest();
        }

        private People TestGetJsonData()
        {
            string fileName = "mock_data.json";
            string jsonString = System.IO.File.ReadAllText(fileName);
            var people = JsonConvert.DeserializeObject<People>(jsonString);

            return people;
        }

        private void TestWriteJsonData(People deserializedObject)
        {
            string output = JsonConvert.SerializeObject(deserializedObject, Formatting.Indented);
            System.IO.File.WriteAllText("mock_data.json", output);
        }

        private int IdAutoIncrementer()
        {
            return TestGetPeople().people.Max(x => x.Id) + 1;
        }

        private People TestData()
        {

            People people = new People();
            people.people.Add(new Person() {
                Id = 10,
                Name = "Robert",
                Age = 58
            });
            people.people.Add(new Person() {
                Id = 11,
                Name = "Steven",
                Age = 22
            });
            people.people.Add(new Person() {
                Id = 12,
                Name = "Abe",
                Age = 24
            });
            people.people.Add(new Person() {
                Id = 13,
                Name = "Phillips",
                Age = 26
            });
            people.people.Add(new Person() {
                Id = 14,
                Name = "Bob",
                Age = 28
            });
            people.people.Add(new Person() {
                Id = 14,
                Name = "Mark",
                Age = 30
            });
            people.people.Add(new Person() {
                Id = 15,
                Name = "Luke",
                Age = 32
            });
            people.people.Add(new Person() {
                Id = 16,
                Name = "John",
                Age = 34
            });
            people.people.Add(new Person() {
                Id = 17,
                Name = "Romans",
                Age = 36
            });
            people.people.Add(new Person() {
                Id = 18,
                Name = "Penelope",
                Age = 38
            });
            people.people.Add(new Person() {
                Id = 19,
                Name = "Michael",
                Age = 40
            });
            people.people.Add(new Person() {
                Id = 20,
                Name = "Rebecca",
                Age = 42
            });
            people.people.Add(new Person() {
                Id = 21,
                Name = "Ben",
                Age = 44
            });
            people.people.Add(new Person() {
                Id = 22,
                Name = "Rachel",
                Age = 46
            });
            people.people.Add(new Person() {
                Id = 23,
                Name = "Grace",
                Age = 48
            });
            people.people.Add(new Person() {
                Id = 24,
                Name = "Jacinda",
                Age = 50
            });
            people.people.Add(new Person() {
                Id = 25,
                Name = "Carly",
                Age = 52
            });

            return people;
        }
    }
}