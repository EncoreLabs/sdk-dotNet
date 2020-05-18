@echo off
setlocal enabledelayedexpansion

SET DocFxPath=%USERPROFILE%\.nuget\packages\docfx.console\2.53.1\tools\docfx.exe
SET ConfigFileName=docfx.json
SET DocsFolderPath=docfx_project
SET DocFxOverrideFolderPath=docfx_override

%DocFxPath% init -q
for /R "%DocFxOverrideFolderPath%" %%F IN (*) do (
	SET FileDir=%%~dpF
	SET FileIntermediateDir=!FileDir:%CD%\%DocFxOverrideFolderPath%\=!
	xcopy /E /I /Y "%%F" "%DocsFolderPath%\!FileIntermediateDir!"
)
%DocFxPath% %DocsFolderPath%\%ConfigFileName% --serve