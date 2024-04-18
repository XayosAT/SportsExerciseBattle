using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExercise.Models;


namespace SportsExercise.DAL;

internal interface IScoreboardDao
{
    Stats[] FetchScoreboard();
}