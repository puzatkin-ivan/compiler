#!/usr/bin/env bash

# Build source code compiler

dotnet run

cd ../ConsoleApp

dotnet publish >> /dev/null

dotnet-ildasm ./bin/Debug/netcoreapp3.1/ConsoleApp.dll > ../compiler/out/lsdlang.il

cd ../compiler

cd ./bin/Replacer

dotnet run

cd ../../

ilasm ./out/lsdlang.il

wine ./out/lsdlang.exe

