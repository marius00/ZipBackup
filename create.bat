@echo off
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"

set "datestamp=1.0.%YY%%MM%.%DD%%HH%"
@echo on


dotnet build -p:Version=%datestamp% --configuration Release

mkdir installer 2> NUL
copy ZipBackup\bin\Release\net5.0-windows10.0.17763.0\ZipBackup.exe installer/ZipBackup.exe

Inno\iscc Inno\installer.iss