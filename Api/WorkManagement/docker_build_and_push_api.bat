@echo off
echo ====== PUBLISH .NET API ======
dotnet publish -c Release -o bin/Release/PublishOutput

echo ====== DOCKER BUILD ======
docker build -f Dockerfile -t ghcr.io/quanbui02/workmanagement-api:1.0 .

echo ====== DOCKER LOGIN GHCR ======
echo %GHCR_PAT% | docker login ghcr.io -u quanbui02 --password-stdin

echo ====== DOCKER PUSH ======
docker push ghcr.io/quanbui02/workmanagement-api:1.0

echo ====== CALL DEPLOY WEBHOOK ======
curl -X POST http://15.134.201.47:9000/hooks/deploy-api

pause
