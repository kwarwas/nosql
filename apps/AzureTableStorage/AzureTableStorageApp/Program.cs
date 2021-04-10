using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Azure.Cosmos.Table;

namespace AzureTableStorageApp
{
    public class EmployeeEntity : TableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }
        public string Country { get; set; }

        public EmployeeEntity()
        {
        }

        public EmployeeEntity(string rowKey, string firstName, string lastName, DateTime hireDate, string country)
        {
            RowKey = rowKey;
            PartitionKey = country;
            FirstName = firstName;
            LastName = lastName;
            HireDate = hireDate;
            Country = country;
        }

        public override string ToString()
        {
            return $"FirstName: {FirstName}, LastName: {LastName}, HireDate: {HireDate}, Country: {Country}";
        }
    }

    static class Program
    {
        static async Task Main(string[] args)
        {
            var storageAccount = CloudStorageAccount.Parse("from azure portal");

            var client = storageAccount.CreateCloudTableClient();

            var table = client.GetTableReference("employees");

            await table.CreateIfNotExistsAsync();

            await DeleteBatchData(table);

            await InsertData(table);

            await InsertBatchData(table);

            await ReplaceData(table);

            await Select(table);

            await DeleteData(table);
        }

        private static async Task Select(CloudTable table)
        {
            Console.WriteLine("Data selecting...");

            var dateCondition = TableQuery.GenerateFilterConditionForDate(
                "HireDate",
                QueryComparisons.LessThan,
                DateTime.Now);

            var lastNameCondition = TableQuery.GenerateFilterCondition(
                "LastName",
                QueryComparisons.Equal,
                "Kowalski");

            var filters = TableQuery.CombineFilters(dateCondition, TableOperators.And, lastNameCondition);

            var query = new TableQuery<EmployeeEntity>()
                .Where(filters);

            await table.ExecuteAsync(query, async segment =>
            {
                foreach (var employee in segment.Results)
                {
                    await Console.Out.WriteLineAsync(employee.ToString());
                }
            });

            Console.WriteLine("Data selected successfully");
        }

        private static async Task InsertData(CloudTable table)
        {
            Console.WriteLine("Inserting data...");

            var employee = GetData().Generate();

            var insertOp = TableOperation.InsertOrReplace(employee);

            await table.ExecuteAsync(insertOp);

            Console.WriteLine("Data inserted...");
        }

        private static async Task InsertBatchData(CloudTable table)
        {
            Console.WriteLine("Data (batch) inserting...");

            Console.WriteLine(" - generating data...");

            var employees = GetData().Generate(1000);

            Console.WriteLine(" - data generated");

            Console.WriteLine(" - inserting...");

            foreach (var employeesGroup in employees.GroupBy(x => x.PartitionKey))
            {
                var batchOperation = new TableBatchOperation();

                foreach (var employee in employeesGroup)
                {
                    batchOperation.InsertOrReplace(employee);
                }

                await table.ExecuteBatchAsync(batchOperation);
            }

            Console.WriteLine("Data inserted");
        }
        
        private static async Task ReplaceData(CloudTable table)
        {
            Console.WriteLine("Replacing data...");

            var query = await table.ExecuteQuerySegmentedAsync(new TableQuery<EmployeeEntity>(), null);

            var employee = query.Results.FirstOrDefault();

            if (employee == null)
                throw new Exception();

            Console.WriteLine("Employee to update: {0}", employee);

            employee.LastName = "Kowalski";

            await table.ExecuteAsync(TableOperation.Replace(employee));

            Console.WriteLine("Data replaced");
        }

        private static async Task DeleteData(CloudTable table)
        {
            Console.WriteLine("Deleting data...");

            var query = await table.ExecuteQuerySegmentedAsync(new TableQuery<EmployeeEntity>(), null);

            var employee = query.Results.FirstOrDefault();

            if (employee == null)
                throw new Exception();

            Console.WriteLine("Employee to delete: {0}", employee);

            await table.ExecuteAsync(TableOperation.Delete(employee));

            Console.WriteLine("Data deleted");
        }

        private static async Task DeleteBatchData(CloudTable table)
        {
            Console.WriteLine("Deleting data...");

            int segmentNumber = 1;

            await ExecuteAsync(table, new TableQuery<EmployeeEntity>(), async segment =>
            {
                Console.WriteLine(" - segment {0}", segmentNumber++);

                foreach (var employeeGroup in segment.GroupBy(x => x.PartitionKey))
                {
                    var batchOperation = new TableBatchOperation();

                    foreach (var employee in employeeGroup)
                    {
                        batchOperation.Delete(employee);
                    }

                    await table.ExecuteBatchAsync(batchOperation);
                }
            });

            Console.WriteLine("Data deleted");
        }
        
        private static Faker<EmployeeEntity> GetData()
        {
            return new Faker<EmployeeEntity>()
                .CustomInstantiator(faker => new EmployeeEntity
                (
                    faker.Random.Guid().ToString(),
                    faker.Person.FirstName,
                    faker.Person.LastName,
                    faker.Date.Past(5),
                    faker.Address.Country()
                ));
        }

        private static async Task ExecuteAsync<T>(this CloudTable table,
            TableQuery<T> query,
            Func<TableQuerySegment<T>, Task> onProgress = null,
            CancellationToken cancellationToken = default)
            where T : ITableEntity, new()
        {
            TableContinuationToken token = null;

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token, cancellationToken);

                token = segment.ContinuationToken;

                if (onProgress != null)
                {
                    await onProgress.Invoke(segment);
                }
            } while (token != null && !cancellationToken.IsCancellationRequested);
        }
    }
}