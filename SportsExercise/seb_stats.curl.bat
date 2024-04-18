@echo off

REM --------------------------------------------------
echo 4) stats (get my elo value and count of pushups overall; startup value e.g. 100)
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-sebToken"
echo.

pause

curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 5) scoreboard (compare elo values and count of pushups accross all users)
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-sebToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 6) history (count and duration; currently empty)
curl -X GET http://localhost:10001/history --header "Authorization: Basic kienboec-sebToken"
echo.
curl -X GET http://localhost:10001/history --header "Authorization: Basic altenhof-sebToken"
echo.
echo.

pause
