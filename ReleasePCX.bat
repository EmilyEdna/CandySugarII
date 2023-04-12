chcp 65001
@echo 开始自动化发布
cd /d %~dp0
dotnet publish PCX\CandySugar.EntryUI\CandySugar.EntryUI.csproj -c Release -o ..\CandySugarII\2\Release -f net7.0-windows --sc true -r win-x64
dotnet publish PCX\Component\CandySugar.LightNovel\CandySugar.LightNovel.csproj -c Release -o ..\CandySugarII\2\Release -f net7.0-windows --sc true -r win-x64
dotnet publish PCX\Component\CandySugar.Music\CandySugar.Music.csproj -c Release -o ..\CandySugarII\2\Release -f net7.0-windows --sc true -r win-x64
dotnet publish PCX\Component\CandySugar.WallPaper\CandySugar.WallPaper.csproj -c Release -o ..\CandySugarII\2\Release -f net7.0-windows --sc true -r win-x64

rd /S /Q PCX\CandySugar.EntryUI\obj PCX\CandySugar.EntryUI\bin\Release

rd /S /Q PCX\CandySugar.Com.Controls\obj PCX\CandySugar.Com.Controls\bin\Release

rd /S /Q PCX\CandySugar.Com.Library\obj PCX\CandySugar.Com.Library\bin\Release

rd /S /Q PCX\CandySugar.Com.Options\obj PCX\CandySugar.Com.Options\bin\Release

rd /S /Q PCX\CandySugar.Com.Style\obj PCX\CandySugar.Com.Style\bin\Release

rd /S /Q PCX\Component\CandySugar.LightNovel\obj PCX\Component\CandySugar.LightNovel\bin\Release

rd /S /Q PCX\Component\CandySugar.Music\obj PCX\Component\CandySugar.Music\bin\Release

rd /S /Q PCX\Component\CandySugar.WallPaper\obj PCX\Component\CandySugar.WallPaper\bin\Release

xcopy PCX\CandySugar.EntryUI\bin\Debug\net7.0-windows\ffmpeg 2\Release\ffmpeg /e /s

cd 2\Release
del *.pdb *.json 
ren CandySugar.EntryUI.exe CandySugar.exe
@echo 已完成处理
pause
