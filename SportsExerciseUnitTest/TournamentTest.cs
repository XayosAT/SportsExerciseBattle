using Moq;
using NUnit.Framework;
using SportsExercise.BLL;
using SportsExercise.Models;
using SportsExercise.DAL;
namespace SportsExerciseUnitTest;

[TestFixture]
public class TournamentTest
{
    private Mock<IUserManager> _userManagerMock;
    private TournamentManager? _tournamentManager;

    [SetUp]
    public void Setup()
    {
        _userManagerMock = new Mock<IUserManager>();
        _tournamentManager = TournamentManager.GetInstance(_userManagerMock.Object);
    }
 
    
    // Test that the TournamentManager.GetInstance method returns the same instance of TournamentManager
    [Test]
    public void GetInstance_Always_ReturnsSameInstance()
    {
        var instance1 = TournamentManager.GetInstance(_userManagerMock.Object);
        var instance2 = TournamentManager.GetInstance(_userManagerMock.Object);
        Assert.That(instance1, Is.EqualTo(instance2));
    }
    
    //Test that GetTournamentInfo returns "No active tournament" when the tournament is not active
    [Test, Order(1)]
    public void GetTournamentInfo_TournamentNotActive_ReturnsNoActiveTournament()
    {
        Assert.That(_tournamentManager.GetTournamentInfo(), Is.EqualTo("No active tournament"));
        Assert.That(_tournamentManager.IsTournamentActive, Is.False);
    }
    
    // Test that the AddEntry method starts the tournament when the tournament is not active
    [Test, Order(2)]
    public void AddEntry_TournamentNotActive_StartsTournament()
    {
        Assert.That(_tournamentManager.IsTournamentActive, Is.False);
        _tournamentManager.AddEntry("username", new Entry("name", 10, 60));
        Assert.That(_tournamentManager.IsTournamentActive, Is.True);
    }
    
    //Test that log is not empty after the AddEntry method is called
    [Test, Order(3)]
    public void AddEntry_TournamentNotActive_LogNotEmpty()
    {
        var response =_tournamentManager.AddEntry("username", new Entry("name", 10, 60));
        Assert.That(response, Is.Not.Empty);
        //assert that response is not equal to "No active tournament"
        Assert.That(response, Is.Not.EqualTo("No active tournament"));
    }
    
    
    // Test that the AddEntry method throws an exception when the Entry object is invalid
    [Test]
    public void AddEntry_InvalidEntry_ThrowsException()
    {
        Assert.That(() => _tournamentManager.AddEntry("username", new Entry(null,null,null)), Throws.Exception);
    }
    
    //Test that the entry is added to the user pushups after the AddEntry method is called
    [Test]
    public void AddEntry_TournamentActive_AddsEntryToUserPushUps()
    {
        _tournamentManager.AddEntry("username1", new Entry("name", 10, 60));
        
        Assert.That(_tournamentManager.GetPushUps("username1"), Is.EqualTo(10));
    }
    
    //Test that the AddEntry returns "Entry added successfully" when an entry is added
    [Test]
    public void AddEntry_TournamentActive_AddsEntry()
    {
        var response = _tournamentManager.AddEntry("username", new Entry("name", 10, 60));
        
        Assert.That(response, Is.EqualTo("Entry added successfully"));
    }
    
    //Test that the AddEntry method adds multiple entries to the user pushups
    [Test]
    public void AddEntry_TournamentActive_AddsEntryToUserPushUps_WhenMultipleUsers()
    {
        _tournamentManager.AddEntry("username2", new Entry("name", 10, 60));
        _tournamentManager.AddEntry("username3", new Entry("name", 20, 60));
        
        Assert.That(_tournamentManager.GetPushUps("username2"), Is.EqualTo(10));
        Assert.That(_tournamentManager.GetPushUps("username3"), Is.EqualTo(20));
    }
    
    //Test that count is added to the user pushups when the AddEntry method is called multiple times for the same user
    [Test]
    public void AddEntry_TournamentActive_AddsEntryToUserPushUps_WhenConsecutiveEntries()
    {
        _tournamentManager.AddEntry("username4", new Entry("name", 10, 60));
        
        Assert.That(_tournamentManager.GetPushUps("username4"), Is.EqualTo(10));
        _tournamentManager.AddEntry("username4", new Entry("name", 20, 60));
        
        Assert.That(_tournamentManager.GetPushUps("username4"), Is.EqualTo(30));
    }
    
    
    //Test that GetPushUps returns -1 when the user does not exist
    [Test]
    public void GetPushUps_UserDoesNotExist_ReturnsMinusOne()
    {
        Assert.That(_tournamentManager.GetPushUps("no_user"), Is.EqualTo(-1));
    }
    
    //Test that the Tournament is inactive after 20 seconds,
    // IMPORTANT: This test will fail if the timer is set to 2 minutes, should be adjusted if the timer is changed
    [Test]
    public void EndTournament_TournamentActive_After21Seconds()
    {
        _tournamentManager.AddEntry("username5", new Entry("name", 10, 60));
        Assert.That(_tournamentManager.IsTournamentActive, Is.True);
        
        System.Threading.Thread.Sleep(21000);
        
        Assert.That(_tournamentManager.IsTournamentActive, Is.False);
    }
    
    
    
    
    
    
}