REM ntp.sydney.nmi.gov.au time.windows.com,0x9

w32tm /config /manualpeerlist:"ntp.sydney.nmi.gov.au,0x1 time.windows.com,0x9" /syncfromflags:manual /reliable:yes /update
net stop w32time
net start w32time