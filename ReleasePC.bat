chcp 65001
@echo 开始自动化发布
cd /d %~dp0
dotnet publish PC\CandySugar.Entry\CandySugar.Entry.csproj -c Release -o ..\CandySugarII\1\Release -f net7.0-windows10.0.17763.0 --sc true -r win-x64
dotnet publish PC\CandySugar.Advance\CandySugar.Advance.csproj -c Release -o ..\CandySugarII\1\Release -f net7.0-windows10.0.17763.0 --sc true -r win-x64

rd /S /Q PC\CandySugar.Entry\obj PC\CandySugar.Entry\bin\Release
rd /S /Q PC\CandySugar.Logic\obj PC\CandySugar.Logic\bin\Release
rd /S /Q PC\CandySugar.Library\obj PC\CandySugar.Library\bin\Release
rd /S /Q PC\CandySugar.Controls\obj PC\CandySugar.Controls\bin\Release
rd /S /Q PC\CandySugar.Resource\obj PC\CandySugar.Resource\bin\Release
rd /S /Q PC\CandySugar.Advance\obj PC\CandySugar.Advance\bin\Release
::xcopy PC\CandySugar.WPF\bin\Debug\net7.0-windows10.0.17763.0\Plugins Release\Plugins /e /s

cd Release
del *.pdb
ren CandySugar.Entry.exe CandySugar.exe
@echo 已完成处理
pause
