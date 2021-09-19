dotnet build --configuration Release

mkdir installer 2> NUL
copy ZipBackup\bin\Release\net5.0-windows10.0.17763.0\ZipBackup.exe installer/ZipBackup.exe

Inno\iscc Inno\installer.iss