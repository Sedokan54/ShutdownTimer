@echo off
echo Building Shutdown Timer Release...
echo ================================

REM Clean previous builds
if exist "bin\Release" rmdir /s /q "bin\Release"
if exist "obj\Release" rmdir /s /q "obj\Release"

REM Build self-contained executable
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ================================
    echo Build completed successfully!
    echo Executable location: bin\Release\net6.0-windows\win-x64\publish\ShutdownTimer.exe
    echo ================================
    
    REM Copy executable to root for easy access
    copy "bin\Release\net6.0-windows\win-x64\publish\ShutdownTimer.exe" "ShutdownTimer.exe"
    
    echo.
    echo ShutdownTimer.exe is ready to use!
    echo You can now run it directly without any dependencies.
) else (
    echo.
    echo Build failed! Please check the errors above.
)

pause