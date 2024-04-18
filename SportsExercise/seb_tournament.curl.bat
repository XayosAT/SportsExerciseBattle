@echo off


REM --------------------------------------------------
echo 7) list current tournament info/state (currently none)
pause
curl -X GET http://localhost:10001/tournament --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/tournament --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 8) add entry to history / starts a tournament
curl -X POST http://localhost:10001/history --header "Content-Type: application/json" --header "Authorization: Basic kienboec-sebToken" -d "{\"Name\": \"PushUps\",  \"Count\": 40, \"DurationInSeconds\": 60}"
echo.
curl -X POST http://localhost:10001/history --header "Content-Type: application/json" --header "Authorization: Basic altenhof-sebToken" -d "{\"Name\": \"PushUps\",  \"Count\": 50, \"DurationInSeconds\": 70}"
echo.
echo.

pause

REM --------------------------------------------------
echo 9) list current tournament info/state (tournament started; 2 participants; altenhof in front; write start-time)
curl -X GET http://localhost:10001/tournament --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/tournament --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 10) stats (get my elo value and count of pushups overall; startup value like 100 - no tournament should be finished here)
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 11) scoreboard (compare elo values and count of pushups accross all users; still startup values)
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 12) history (count and duration; 1 entry each)
curl -X GET http://localhost:10001/history --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/history --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 13) add entry to history / continues in tournament
curl -X POST http://localhost:10001/history --header "Content-Type: application/json" --header "Authorization: Basic kienboec-sebToken" -d "{\"Name\": \"PushUps\",  \"Count\": 11, \"DurationInSeconds\": 25}"
echo.
echo.

pause

REM --------------------------------------------------
echo 14) list current tournament info/state 
curl -X GET http://localhost:10001/tournament --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/tournament --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 15) sleep of 2min (afterwards the tournament should be over and elo values need to be updated)
ping localhost -n 120 >NUL 2>NUL
echo.
echo.

pause

REM --------------------------------------------------
echo 16) list current tournament info/state (1 tournament with state ended)
curl -X GET http://localhost:10001/tournament --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/tournament --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 17) stats
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 18) scoreboard 
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 19) history 
curl -X GET http://localhost:10001/history --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/history --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

REM --------------------------------------------------
echo end...


@echo on

pause