#!/usr/bin/env bash

# Build source code compiler

dotnet run

ilasm ./out/lsdlang.il

wine ./out/lsdlang.exe

