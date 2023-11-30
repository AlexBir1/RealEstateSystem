using DwellingAPI.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.xUnit.DbContext
{
    public static class TestDBContext
    {
        public static AppDBContext GetTestDBContext()
        {
            return new AppDBContext(new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options, true);
        }
    }
}
