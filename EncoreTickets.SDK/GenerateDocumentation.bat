@echo off

SET DocFxPath=%USERPROFILE%\.nuget\packages\docfx.console\2.53.1\tools\docfx.exe
SET ConfigFileName=docfx.json
SET DocsFolderPath=docfx_project

%DocFxPath% init -q
copy %ConfigFileName% %DocsFolderPath%\%ConfigFileName%
%DocFxPath% %DocsFolderPath%\%ConfigFileName% --serve