"D:\Soft\.NET Reactor\dotNET_Reactor.Console.exe" -satellite_assemblies "D:\Project\CandySugarII\2\Release\CandySugar.Com.Controls.dll;D:\Project\CandySugarII\2\Release\CandySugar.Com.Library.dll;D:\Project\CandySugarII\2\Release\CandySugar.Com.Options.dll;D:\Project\CandySugarII\2\Release\CandySugar.Com.Style.dll;D:\Project\CandySugarII\2\Release\CandySugar.EntryUI.dll;D:\Project\CandySugarII\2\Release\CandySugar.LightNovel.dll;D:\Project\CandySugarII\2\Release\CandySugar.Music.dll;D:\Project\CandySugarII\2\Release\CandySugar.WallPaper.dll;D:\Project\CandySugarII\2\Release\Sdk.Component.dll;D:\Project\CandySugarII\2\Release\Sdk.Core.dll" -antitamp 1 -anti_debug 1 -hide_calls 1 -stringencryption 0 -virtualization 1 -necrobit 1 -obfuscation 0 control_flow_obfuscation 1 -flow_level 9

chcp 65001
@echo 开始覆盖
xcopy 2\Release\CandySugar.Com.Controls_Secure\CandySugar.Com.Controls.dll 2\Release /e /s /y
xcopy 2\Release\CandySugar.Com.Library_Secure\CandySugar.Com.Library.dll 2\Release /e /s /y
xcopy 2\Release\CandySugar.Com.Options_Secure\CandySugar.Com.Options.dll 2\Release /e /s /y
xcopy 2\Release\CandySugar.Com.Style_Secure\CandySugar.Com.Style.dll 2\Release /e /s /y
xcopy 2\Release\CandySugar.EntryUI_Secure\CandySugar.EntryUI.dll 2\Release /e /s /y
xcopy 2\Release\CandySugar.LightNovel_Secure\CandySugar.LightNovel.dll 2\Release /e /s /y
xcopy 2\Release\CandySugar.Music_Secure\CandySugar.Music.dll 2\Release /e /s /y
xcopy 2\Release\CandySugar.WallPaper_Secure\CandySugar.WallPaper.dll 2\Release /e /s /y
xcopy 2\Release\Sdk.Component_Secure\Sdk.Component.dll 2\Release /e /s /y
xcopy 2\Release\Sdk.Core_Secure\Sdk.Core.dll 2\Release /e /s /y
@echo 覆盖完成
@echo 执行删除
rd /S /Q 2\Release\CandySugar.Com.Controls_Secure
rd /S /Q 2\Release\CandySugar.Com.Library_Secure
rd /S /Q 2\Release\CandySugar.Com.Options_Secure
rd /S /Q 2\Release\CandySugar.Com.Style_Secure
rd /S /Q 2\Release\CandySugar.EntryUI_Secure
rd /S /Q 2\Release\CandySugar.LightNovel_Secure
rd /S /Q 2\Release\CandySugar.Music_Secure
rd /S /Q 2\Release\CandySugar.WallPaper_Secure
rd /S /Q 2\Release\Sdk.Component_Secure
rd /S /Q 2\Release\Sdk.Core_Secure
@echo 删除完成
pause