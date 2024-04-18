using Moq;
using NUnit.Framework;
using SportsExercise.BLL;
using SportsExercise.Models;
using SportsExercise.DAL;

namespace SportsExerciseUnitTest;

[TestFixture]
public class Tests
{
    IUserDao _userDao;
    IUserManager _userManager;
    
    [SetUp]
    public void Setup()
    {
        _userDao = new Mock<IUserDao>().Object;
        _userManager = new UserManager(_userDao);
    }

    



}