using System;
using System.IO;
using Bogus;
using Bogus.DataSets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PeopleGenerator
{
    class Person
    {
        [JsonProperty("_id")] public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Name.Gender Gender { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static Faker<Person> GenerateFakeData() =>
            new Faker<Person>()
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Gender, faker => faker.Person.Gender)
                .RuleFor(x => x.FirstName, (faker, person) => faker.Name.FirstName(person.Gender))
                .RuleFor(x => x.LastName, (faker, person) => faker.Name.LastName(person.Gender))
                .RuleFor(x => x.Email, (faker, person) => faker.Internet.Email(person.FirstName, person.LastName))
                .RuleFor(x => x.Age, faker => faker.Random.Number(12, 70));

        static void Main(string[] args)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var data = JsonConvert.SerializeObject(
                GenerateFakeData().Generate(500),
                new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                }
            );

            File.WriteAllText("people.json", data);
        }
    }
}