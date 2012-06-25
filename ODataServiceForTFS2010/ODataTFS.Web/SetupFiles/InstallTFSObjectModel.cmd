@ECHO OFF

SETLOCAL
%~d0
CD "%~dp0"

IF "%PROCESSOR_ARCHITECTURE%" == "x86" GOTO x86
IF "%PROCESSOR_ARCHITECTURE%" == "AMD64" GOTO x64

:x86
REM Intended for Compute Emulator usage on x86 machines.
.\TFSObjectModel-x86_enu.msi -quiet USING_EXUIH=1

GOTO End

:x64
.\TFSObjectModel-x64_enu.msi -quiet USING_EXUIH=1
GOTO End

:End