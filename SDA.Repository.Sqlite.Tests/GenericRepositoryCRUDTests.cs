using SQLite;
using Xunit.Abstractions;

namespace SDA.Repository.Sqlite.Tests
{
    [Table("category")]
    public class TestingModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Info { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TestingModel testingModel && Info == testingModel.Info && Id == testingModel.Id;
        }
    }

    public class GenericRepositoryCRUDTests
    {
        private readonly ITestOutputHelper _output;

        public GenericRepositoryCRUDTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async void AddSingleItemTest()
        {
            // Initialization
            TestingModel test1 = new TestingModel() { Info = "test1" };
            GenericRepository repository = new GenericRepository("testing.db3");

            //Testing
            int result = await repository.AddItem(test1);
            _output.WriteLine(repository.StatusMessage);
            Assert.Equal(1, result);

            var items = await repository.GetAllItems<TestingModel>();
            Assert.Contains(test1, items);

            // Cleanup
            await repository.DropItemTable<TestingModel>();
        }
    }
}