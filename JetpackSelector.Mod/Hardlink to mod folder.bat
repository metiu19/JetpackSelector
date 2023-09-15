@ECHO off

set /p modFolder="Please enter the folder location of the correct script folder of your mod: "

for %%f in (.\*.cs) do (
cmd /c mklink /H "%modFolder%\%%~nxf" "%%~ff"
if errorlevel 1 goto Error
)

for /f "tokens=*" %%a in ('dir .\ /b /a /ad ^|find ^"bin^" /v /i ^|find ^"obj^" /v /i ^|find ^"properties^" /v /i') do (
cmd /c mkdir "%modFolder%\%%a"
for %%f in (%%a\*.cs) do (
cmd /c mklink /H "%modFolder%\%%a\%%~nxf" "%%~ff"
if errorlevel 1 goto Error
)
)
goto End

:Error
echo An error occured!
:End
pause