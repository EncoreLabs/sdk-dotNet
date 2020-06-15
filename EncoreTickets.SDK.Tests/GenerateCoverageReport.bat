@echo off
REM ** All paths should be entered without quotes

SET TestResultsFileProjectName=SdkResults

SET DLLToTestRelativePath=bin\Debug\netcoreapp2.1\EncoreTickets.SDK.Tests.dll

REM ** Filters Wiki https://github.com/opencover/opencover/wiki/Usage
SET Filters=+[EncoreTickets.SDK]* -[EncoreTickets.SDK.Tests*]*

SET OpenCoverFolderName=opencover\4.7.922
SET ReportGeneratorFolderName=reportgenerator\4.5.8

if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"

IF EXIST "%~dp0%TestResultsFileProjectName%.trx" del "%~dp0%TestResultsFileProjectName%.trx%"

CD %~dp0
FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"

call :RunOpenCoverUnitTestMetrics

if %errorlevel% equ 0 (
 call :RunReportGeneratorOutput
)

if %errorlevel% equ 0 (
 call :RunLaunchReport
)
exit /b %errorlevel%

:RunOpenCoverUnitTestMetrics
"%USERPROFILE%\.nuget\packages\%OpenCoverFolderName%\tools\OpenCover.Console.exe" ^
-register:user ^
-target:"dotnet.exe" ^
-targetargs:"test --filter FullyQualifiedName~UnitTests" ^
-filter:"%Filters%" ^
-mergebyhash ^
-skipautoprops ^
-excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" ^
-output:"%~dp0GeneratedReports\%TestResultsFileProjectName%.xml" ^
-oldStyle
exit /b %errorlevel%

:RunReportGeneratorOutput
"%USERPROFILE%\.nuget\packages\%ReportGeneratorFolderName%\tools\net47\ReportGenerator.exe" ^
-reports:"%~dp0GeneratedReports\%TestResultsFileProjectName%.xml" ^
-targetdir:"%~dp0GeneratedReports\ReportGenerator Output"
exit /b %errorlevel%

:RunLaunchReport
start "report" "%~dp0GeneratedReports\ReportGenerator Output\index.htm"
exit /b %errorlevel%