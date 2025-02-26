using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tic_Tac_Toe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Tic_Tac_Toe.Infrastructure.Data
{
    public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
    {
        public DbSet<Game> Games { get; set; }
    }
}
