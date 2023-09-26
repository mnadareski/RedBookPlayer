@echo OFF

REM This batch file assumes the following:
REM - .NET 7.0 (or newer) SDK is installed and in PATH
REM - 7-zip commandline (7z.exe) is installed and in PATH
REM - The relevant commandline programs are already downloaded
REM   and put into their respective folders
REM
REM If any of these are not satisfied, the operation may fail
REM in an unpredictable way and result in an incomplete output.

REM Set the current directory as a variable
set BUILD_FOLDER=%~dp0

REM Restore Nuget packages for all builds
echo Restoring Nuget packages
dotnet restore

REM .NET 6.0
echo Building .NET 6.0 releases
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net6.0 -r win-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net6.0 -r win-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net6.0 -r linux-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net6.0 -r linux-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true

REM .NET 7.0
echo Building .NET 7.0 releases
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net7.0 -r win-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net7.0 -r win-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net7.0 -r linux-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net7.0 -r linux-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true

REM Create archives
cd %BUILD_FOLDER%\RedBookPlayer.GUI\bin\Debug\net6.0\win-x64\publish\
7z a -tzip %BUILD_FOLDER%\RedBookPlayer-dbg_net6.0_win-x64.zip *
cd %BUILD_FOLDER%\RedBookPlayer.GUI\bin\Release\net6.0\win-x64\publish\
7z a -tzip %BUILD_FOLDER%\RedBookPlayer_net6.0_win-x64.zip *
cd %BUILD_FOLDER%\RedBookPlayer.GUI\bin\Debug\net6.0\linux-x64\publish\
7z a -tzip %BUILD_FOLDER%\RedBookPlayer-dbg_net6.0_linux-x64.zip *
cd %BUILD_FOLDER%\RedBookPlayer.GUI\bin\Release\net6.0\linux-x64\publish\
7z a -tzip %BUILD_FOLDER%\RedBookPlayer_net6.0_linux-x64.zip *

cd %BUILD_FOLDER%\RedBookPlayer.GUI\bin\Debug\net7.0\win-x64\publish\
7z a -tzip %BUILD_FOLDER%\RedBookPlayer-dbg_net7.0_win-x64.zip *
cd %BUILD_FOLDER%\RedBookPlayer.GUI\bin\Release\net7.0\win-x64\publish\
7z a -tzip %BUILD_FOLDER%\RedBookPlayer_net7.0_win-x64.zip *
cd %BUILD_FOLDER%\RedBookPlayer.GUI\bin\Debug\net7.0\linux-x64\publish\
7z a -tzip %BUILD_FOLDER%\RedBookPlayer-dbg_net7.0_linux-x64.zip *
cd %BUILD_FOLDER%\RedBookPlayer.GUI\bin\Release\net7.0\linux-x64\publish\
7z a -tzip %BUILD_FOLDER%\RedBookPlayer_net7.0_linux-x64.zip *