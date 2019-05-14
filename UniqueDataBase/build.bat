@echo off
set version=%1

if defined version (
    cd KeyValueDatabase
    dotnet publish
	cd ../client1
    dotnet publish
    cd ..

	if not exist build_versions (
		mkdir build_versions
	)

	cd build_versions
	if not exist %version% (
		mkdir %version%
		cd %version%
	
        mkdir KeyValueDatabase
        cd KeyValueDatabase
        xcopy ..\..\..\KeyValueDatabase\bin\Debug\netcoreapp2.2\publish /S
		cd ..
        xcopy ..\..\run_servers_with_dB.bat

        echo KeyValueDatabase:5000> config.txt
		echo KeyValueDatabase:5001>> config.txt

        echo cd client1 > run.bat
        echo start "client1" dotnet client1.dll >> run.bat
		echo cd .. >> run.bat
		echo call run_servers_with_dB.bat >> run.bat

		echo taskkill /IM dotnet.exe /F > stop.bat
	) else (
		echo This version already exists.
	)
) else ( 
	echo Version are required.
)