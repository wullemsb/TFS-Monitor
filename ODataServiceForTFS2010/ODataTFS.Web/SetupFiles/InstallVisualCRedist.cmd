@ECHO OFF

SETLOCAL
%~d0
CD "%~dp0"

IF "%PROCESSOR_ARCHITECTURE%" == "x86" GOTO x86
IF "%PROCESSOR_ARCHITECTURE%" == "AMD64" GOTO x64

:x86
REM Intended for Compute Emulator usage on x86 machines.
.\vcredist_x86.exe /q
GOTO End

:x64
.\vcredist_x64.exe /q
GOTO End

:End
REM Wait for the installation to finish.
PING -n 30 127.0.0.1>nul 