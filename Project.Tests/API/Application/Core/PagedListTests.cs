using Application.Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.Application.Core
{
    public class PagedListTests : IDisposable
    {
        private readonly DataContext _context;

        public PagedListTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            // הוספת 50 משתמשים לבדיקה
            for (int i = 1; i <= 50; i++)
            {
                _context.Users.Add(new AppUser
                {
                    Id = $"user{i}", // שימוש במחרוזת למניעת התנגשויות
                    UserName = $"user{i}"
                });
            }

            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateAsync_Should_Return_Correct_Page()
        {
            // Arrange
            var query = _context.Users.AsQueryable();
            int pageNumber = 2;
            int pageSize = 10;

            // Act
            var result = await PagedList<AppUser>.CreateAsync(query, pageNumber, pageSize);

            // Assert
            Assert.Equal(10, result.Count);
            Assert.Equal(2, result.CurrentPage);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(50, result.TotalCount);
            Assert.Equal(5, result.TotalPages);
        }

        [Fact]
        public async Task CreateAsync_Should_Return_Empty_Page_When_PageNumber_Exceeds_TotalPages()
        {
            // Arrange – מחיקת המשתמשים הקיימים
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();

            var users = Enumerable.Range(101, 100)
                .Select(i => new AppUser { Id = $"user{i}", UserName = $"User{i}" });

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            var query = _context.Users.AsQueryable();

            // Act
            var pagedList = await PagedList<AppUser>.CreateAsync(query, pageNumber: 20, pageSize: 10);

            // Assert
            Assert.Empty(pagedList); // רשימה ריקה
            Assert.Equal(100, pagedList.TotalCount);
            Assert.Equal(20, pagedList.CurrentPage);
            Assert.Equal(10, pagedList.TotalPages);
        }

        [Fact]
public async Task CreateAsync_Should_Return_Correct_Items_For_Second_Page()
{
    // Arrange
    _context.Users.RemoveRange(_context.Users);
    await _context.SaveChangesAsync();

    var users = Enumerable.Range(1, 30)
        .Select(i => new AppUser { Id = Guid.NewGuid().ToString(), UserName = $"User{i:D2}" })
        .ToList();

    await _context.Users.AddRangeAsync(users);
    await _context.SaveChangesAsync();

    var query = _context.Users
        .OrderBy(u => u.UserName)
        .AsQueryable();

    // Act
    var result = await PagedList<AppUser>.CreateAsync(query, pageNumber: 2, pageSize: 10);

    // Assert
    Assert.Equal(10, result.Count);
    Assert.Equal("User11", result[0].UserName);
    Assert.Equal("User20", result[9].UserName);
}


        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
