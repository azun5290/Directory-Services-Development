REM ntp.sydney.nmi.gov.au time.windows.com,0x9

w32tm /config /syncfromflags:domhier /update
net stop w32time
net start w32time