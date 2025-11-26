@echo off

dotnet publish -c Release -o bin/Release/PublishOutput

docker build -f Dockerfile -t ghcr.io/quanbui02/work-api:latest .

echo %GHCR_PAT% | docker login ghcr.io -u quanbui02 --password-stdin

docker push ghcr.io/quanbui02/work-api:latest

curl -X POST http://15.134.201.47:9000/hooks/deploy-api

pause
