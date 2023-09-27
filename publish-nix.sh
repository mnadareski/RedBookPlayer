#!/usr/bin/env bash

# This batch file assumes the following:
# - .NET 7.0 (or newer) SDK is installed and in PATH
# - zip is installed and in PATH
# - The relevant commandline programs are already downloaded
#   and put into their respective folders
#
# If any of these are not satisfied, the operation may fail
# in an unpredictable way and result in an incomplete output.

# Set the current directory as a variable
BUILD_FOLDER=$PWD

# Restore Nuget packages for all builds
echo "Restoring Nuget packages"
dotnet restore

# .NET 6.0
echo "Building .NET 6.0 releases"
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net6.0 -r win-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net6.0 -r win-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net6.0 -r linux-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net6.0 -r linux-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true

# .NET 7.0
echo "Building .NET 7.0 releases"
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net7.0 -r win-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net7.0 -r win-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net7.0 -r linux-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net7.0 -r linux-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true

# .NET 8.0
echo "Building .NET 8.0 releases"
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net8.0 -r win-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net8.0 -r win-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net8.0 -r linux-x64 -c Debug --self-contained true -p:PublishSingleFile=true
dotnet publish RedBookPlayer.GUI/RedBookPlayer.GUI.csproj -f net8.0 -r linux-x64 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true

# Create archives
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Debug/net6.0/win-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer-dbg_net6.0_win-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Release/net6.0/win-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer_net6.0_win-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Debug/net6.0/linux-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer-dbg_net6.0_linux-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Release/net6.0/linux-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer_net6.0_linux-x64.zip .

cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Debug/net7.0/win-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer-dbg_net7.0_win-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Release/net7.0/win-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer_net7.0_win-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Debug/net7.0/linux-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer-dbg_net7.0_linux-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Release/net7.0/linux-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer_net7.0_linux-x64.zip .

cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Debug/net8.0/win-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer-dbg_net8.0_win-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Release/net8.0/win-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer_net8.0_win-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Debug/net8.0/linux-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer-dbg_net8.0_linux-x64.zip .
cd $BUILD_FOLDER/RedBookPlayer.GUI/bin/Release/net8.0/linux-x64/publish/
zip -r $BUILD_FOLDER/RedBookPlayer_net8.0_linux-x64.zip .