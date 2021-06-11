using FitnessTrainer.DataAccess.DbContexts;
using FitnessTrainer.Services;
using FitnessTrainer.Services.Interfaces;
using FitnessTrainer.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace FitnessTrainer.ControllerTests
{
    public class ExerciseServiceTests
    {
        private readonly IExerciseService _excerciseService;
        private readonly ApplicationDbContext _context;

        public ExerciseServiceTests()
        {
            var testHelper = new TestHelper();
            var DbContext = testHelper.GetInMemoryRepo();
            _context = DbContext;
            _excerciseService = new ExerciseService(_context);
        }


        [Fact]
        public async Task ExerciseServiceCreateExerciseWithValidData()
        {
            // Arrange 
            var exerciseId = 600;

            ExerciseViewModel excercise = new ExerciseViewModel()
            {
                Id = exerciseId,
                Name = "Otjimaniya",
                Description = "test"
            };
            //Act
            await _excerciseService.CreateExercise(excercise, "uniq");
            ExerciseViewModel res = await _excerciseService.GetExerciseById(exerciseId);
            //Assert

            Assert.Equal(excercise.Id, res.Id);
            Assert.Equal(excercise.Name, res.Name);
            Assert.Equal(excercise.Description, res.Description);
        }

        [Fact]
        public async Task ExerciseServiceEditExerciseWithValidData()
        {
            // Arrange 
            var exerciseId = 601;

            ExerciseViewModel excercise = new ExerciseViewModel()
            {
                Id = exerciseId,
                Name = "UpdatedOtjimaniya",
                Description = "test"
            };
            //Act
            await _excerciseService.CreateExercise(excercise, "uniq");
            _context.ChangeTracker.Clear();
            await _excerciseService.UpdateExercise(excercise, "newUniq");
            ExerciseViewModel res = await _excerciseService.GetExerciseById(exerciseId);
            //Assert

            Assert.Equal(excercise.Id, res.Id);
            Assert.Equal(excercise.Name, res.Name);
            Assert.Equal(excercise.Description, res.Description);
        }

        [Fact]
        public async Task ExerciseServiceDeleteFoodWithValidData()
        {
            // Arrange 
            var exerciseId = 602;

            ExerciseViewModel excercise = new ExerciseViewModel()
            {
                Id = exerciseId,
                Name = "Otjimaniya",
                Description = "test"
            };

            //Act
            await _excerciseService.CreateExercise(excercise, "uniq");
            ExerciseViewModel res = await _excerciseService.GetExerciseById(exerciseId);
            await _excerciseService.DeleteExercise(exerciseId);
            ExerciseViewModel deletedValue = await _excerciseService.GetExerciseById(exerciseId);
            //Assert

            Assert.Null(deletedValue);
            Assert.NotNull(res);
        }

        [Fact]
        public async Task ExerciseServiceCreateExcerciseWithSameId()
        {
            // Arrange 
            var exerciseId = 603;

            ExerciseViewModel excercise = new ExerciseViewModel()
            {
                Id = exerciseId,
                Name = "Otjimaniya",
                Description = "test"
            };

            //Act
            await _excerciseService.CreateExercise(excercise, "uniq");

            ExerciseViewModel res = await _excerciseService.GetExerciseById(exerciseId);
            //Assert
            await Assert.ThrowsAsync<System.InvalidOperationException>(async () => await _excerciseService.CreateExercise(excercise, "uniq"));
        }
    }

}