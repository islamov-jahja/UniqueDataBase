cd client1 
start "client1" dotnet client1.dll 
cd ..\PreservingComponent 
start "PreservingComponent" dotnet PreservingComponent.dll 
cd .. 
call run_servers_with_dB.bat 
