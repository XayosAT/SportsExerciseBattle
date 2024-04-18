using SportsExercise.DAL;
using SportsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExercise.BLL;

internal class ScoreboardManager : IScoreboardManager
{
    
    private readonly IScoreboardDao _scoreboardDao;
    
    public ScoreboardManager(IScoreboardDao scoreboardDao)
    {
        _scoreboardDao = scoreboardDao;
    }
    
    public Stats[] GetScoreboard()
    {
        return _scoreboardDao.FetchScoreboard();
    }
}