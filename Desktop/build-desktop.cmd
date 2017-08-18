@echo off

set dir=%~dp0

cd %dir%..\api\BankA.WebApi

dotnet restore -r win10-x64

dotnet publish -r win10-x64 --output ../../desktop/api

cd %dir%..\app

ng build --output-path "../desktop/app" --base-href "#"

cd %dir%src

npm run dist

cd %dir%