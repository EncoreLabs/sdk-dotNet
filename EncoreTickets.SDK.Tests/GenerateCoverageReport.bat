@echo off
REM ** Be sure to install these nugets:
REM ** NUnit.ConsoleRunner
REM ** OpenCover
REM ** ReportGenerator
REM **
REM ** All paths should be entered without quotes

REM ** SET TestResultsFileProjectName=CalculatorResults
SET TestResultsFileProjectName=SdkResults

REM ** SET DLLToTestRelativePath=Calculator\bin\Debug\MyCalc.dll
SET DLLToTestRelativePath=bin\Debug\netcoreapp2.1\EncoreTickets.SDK.Tests.dll

REM ** Filters Wiki https://github.com/opencover/opencover/wiki/Usage
REM ** SET Filters=+[Calculator]* -[Calculator]CalculatorTests.*
SET Filters=+[EncoreTickets.SDK*]* -[EncoreTickets.SDK*Tests*]*

SET OpenCoverFolderName=opencover\4.7.922
SET ReportGeneratorFolderName=reportgenerator\4.5.8

REM *****************************************************************

REM Create a 'GeneratedReports' folder if it does not exist
if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"

REM Remove any previous test execution files to prevent issues overwriting
IF EXIST "%~dp0%TestResultsFileProjectName%.trx" del "%~dp0%TestResultsFileProjectName%.trx%"

REM Remove any previously created test output directories
CD %~dp0
FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"

REM Run the tests against the targeted output
call :RunOpenCoverUnitTestMetrics

REM Generate the report output based on the test results
if %errorlevel% equ 0 (
 call :RunReportGeneratorOutput
)

REM Launch the report
if %errorlevel% equ 0 (
 call :RunLaunchReport
)
exit /b %errorlevel%

:RunOpenCoverUnitTestMetrics
"C:\Users\%USERNAME%\.nuget\packages\%OpenCoverFolderName%\tools\OpenCover.Console.exe" ^
-register:user ^
-target:"dotnet.exe" ^
-targetargs:"test" ^
-filter:"%Filters%" ^
-mergebyhash ^
-skipautoprops ^
-excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" ^
-output:"%~dp0GeneratedReports\%TestResultsFileProjectName%.xml" ^
-oldStyle
exit /b %errorlevel%

:RunReportGeneratorOutput
"C:\Users\%USERNAME%\.nuget\packages\%ReportGeneratorFolderName%\tools\net47\ReportGenerator.exe" ^
-reports:"%~dp0GeneratedReports\%TestResultsFileProjectName%.xml" ^
-targetdir:"%~dp0GeneratedReports\ReportGenerator Output"
exit /b %errorlevel%

:RunLaunchReport
start "report" "%~dp0GeneratedReports\ReportGenerator Output\index.htm"
exit /b %errorlevel%