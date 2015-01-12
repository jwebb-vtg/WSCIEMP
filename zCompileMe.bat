REM Open from a command window root: C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727
set MyDest=D:\VssWrk\Western\WSCIEMP_Deploy

C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_compiler -v /WSCIEMP -p D:\VssWrk\Western\WSCIEMP %MyDest% -f -c -u -fixednames -errorstack


Echo ON

REM Delete ZHost
RD /S /Q %MyDest%\ZHost

REM Delete web.config
DEL %MyDest%\web.config

REM Delete project files
DEL %MyDest%\*.sln
DEL %MyDest%\*.suo
DEL %MyDest%\*.cspro*

REM Delete ZedGraphImages
RD /S /Q %MyDest%\ZedGraphImages

REM Delete PDF
RD /S /Q %MyDest%\PDF

REM Delete Config
RD /S /Q %MyDest%\Config

REM delete error files
DEL %MyDest%\Error*.txt

REM delete ZIP files
DEL %MyDest%\*.zip

REM delete Resharper files
RMDIR /S /Q %MyDest%\_ReSharper.WSCIEMP3

REM delete ZIP files
DEL %MyDest%\*ReSharper.*

REM delete more stuff
DEL %MyDest%\*.docstates

REM Finally remove this bat file from the dest
DEL %MyDest%\zCompileMe.bat




