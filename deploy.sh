#!/bin/bash

dotnet clean && dotnet restore && dotnet build

ssh oserv@10.250.0.18 'fuser -k 50007/tcp'

# Remove os arquivos do diretório remoto
ssh oserv@10.250.0.18 'rm -rf /home/oserv/deploy/Budget/API/*'

ssh oserv@10.250.0.18 'mkdir -p /home/oserv/deploy/Budget/API/Certificates'
ssh oserv@10.250.0.18 'mkdir -p /home/oserv/deploy/Budget/API/runtimes/linux/lib/net8.0'
ssh oserv@10.250.0.18 'mkdir -p /home/oserv/deploy/Budget/API/runtimes/osx/lib/net8.0'
ssh oserv@10.250.0.18 'mkdir -p /home/oserv/deploy/Budget/API/runtimes/win/lib/net8.0'

# Transfere arquivos via SCP
scp -r ./API/bin/Debug/net8.0/* oserv@10.250.0.18:/home/oserv/deploy/Budget/API

ssh -f oserv@10.250.0.18 'cd /home/oserv/deploy/Budget/API && nohup dotnet API.dll &'

echo "Fechou o Pastel!"
