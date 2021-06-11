using FitnessTrainer.DataAccess.DbContexts;
using FitnessTrainer.DomainEntities.Entity;
using FitnessTrainer.Services;
using FitnessTrainer.Services.Interfaces;
using FitnessTrainer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FitnessTrainer.ControllerTests
{
    public class FoodsServiceTests
    {
        private readonly IFoodService _foodService;
        private readonly ApplicationDbContext _context;

        public FoodsServiceTests()
        {
            var testHelper = new TestHelper();
            var DbContext = testHelper.GetInMemoryRepo();
            _context = DbContext;
            _foodService = new FoodService(_context);
        }


        [Fact]
        public async Task FoodServiceCreateFoodWithValidData()
        {
            // Arrange 
            var foodId = 500;

            var food = new Food()
            {
                Id = foodId,
                Calories = 100,
                Carbohydrates = 100,
                Name = "Petrushka"
            };
            //Act
            await _foodService.CreateFood(food);

            var res = await _foodService.GetFoodById(foodId);
            //Assert

            Assert.Equal(food.Id, res.Id);
            Assert.Equal(food.Name, res.Name);
            Assert.Equal(food.Calories, res.Calories);
        }

        [Fact]
        public async Task FoodServiceEditFoodWithValidData()
        {
            // Arrange 
            var foodId = 502;

            Food food = new Food()
            {
                Id = foodId,
                Calories = 100,
                Carbohydrates = 100,
                Name = "Petrushka"
            };

            //Act
            await _foodService.CreateFood(food);

            food.Name = "PetrushkaUpdated";
            food.Calories = 200;

            await _foodService.UpdateFood(food);
            FoodViewModel res = await _foodService.GetFoodById(foodId);
            //Assert

            Assert.Equal(food.Id, res.Id);
            Assert.Equal("PetrushkaUpdated", res.Name);
            Assert.Equal(200, res.Calories);
        }

        [Fact]
        public async Task FoodServiceDeleteFoodWithValidData()
        {
            // Arrange 
            var foodId = 503;

            Food food = new Food()
            {
                Id = foodId,
                Calories = 100,
                Carbohydrates = 100,
                Name = "Petrushka"
            };

            //Act
            await _foodService.CreateFood(food);
            FoodViewModel res = await _foodService.GetFoodById(foodId);
            await _foodService.DeleteFood(foodId);
            FoodViewModel deletedValue = await _foodService.GetFoodById(foodId);
            //Assert

            Assert.Null(deletedValue);
            Assert.NotNull(res);
        }

        [Fact]
        public async Task FoodServiceCreateFoodWithSameId()
        {
            // Arrange 
            var foodId = 504;

            Food food = new Food()
            {
                Id = foodId,
                Calories = 100,
                Carbohydrates = 100,
                Name = "Petrushka"
            };

            //Act
            await _foodService.CreateFood(food);


            FoodViewModel res = await _foodService.GetFoodById(foodId);
            //Assert
            await Assert.ThrowsAsync<System.ArgumentException>(async () => await _foodService.CreateFood(food));
        }
    }

    public class TestHelper
    {
        private readonly ApplicationDbContext _context;

        public TestHelper()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "InMemoryDb");

            var dbContextOptions = builder.Options;
            _context = new ApplicationDbContext(dbContextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public ApplicationDbContext GetInMemoryRepo()
        {
            return _context;
        }

        public IFormFile GetFormFileFromFile(string ImagePath)
        {

            IFormFile formFile;
            using (FileStream streamReader = File.OpenRead(ImagePath))
            {
                var mstream = new MemoryStream();
                streamReader.CopyTo(mstream);
                formFile = new FormFile(mstream, 0, mstream.Length, null, "TestFileName_" + Path.GetFileName(ImagePath));
            }
            return formFile;
        }
    }

}