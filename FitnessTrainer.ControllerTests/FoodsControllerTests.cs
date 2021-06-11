using FitnessTrainer.DataAccess.DbContexts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FitnessTrainer.ControllerTests
{
    public class FoodsControllerTests
    {
        private readonly ApplicationDbContext _context;

        public FoodsControllerTests(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
