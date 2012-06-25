@ECHO OFF

SETLOCAL
%~d0
CD "%~dp0"

REM --------- Variables ---------
SET APPCMD="%systemroot%\system32\inetsrv\APPCMD"
REM -----------------------------

%APPCMD% unlock config /section:anonymousAuthentication

MKDIR "%programdata%\Microsoft\Team Foundation"

CACLS "%programdata%\Microsoft\Team Foundation" /E /G "IIS_IUSRS":F /T /C
ICACLS "%programdata%\Microsoft\Team Foundation" /grant :r "IIS_IUSRS":F /T /Q /C