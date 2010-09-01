@ECHO OFF
%SYSTEMROOT%\Microsoft.Net\Framework\v4.0.30319\msbuild.exe "MvcMembership.sln" /t:rebuild /p:Configuration=Release
PAUSE