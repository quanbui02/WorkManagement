@echo off
setlocal

echo =====================================
echo   LOGIN TO GITHUB CONTAINER REGISTRY
echo =====================================

echo %GHCR_PAT% | docker login ghcr.io -u quanbui02 --password-stdin

if %errorlevel% neq 0 (
    echo ERROR: LOGIN FAILED !
    pause
    exit /b
)

echo =====================================
echo   BUILDING DOCKER IMAGE work-api
echo =====================================

docker build -t ghcr.io/quanbui02/work-api:latest .

if %errorlevel% neq 0 (
    echo ERROR: DOCKER BUILD FAILED !
    pause
    exit /b
)

echo =====================================
echo   PUSHING IMAGE TO GHCR
echo =====================================

docker push ghcr.io/quanbui02/work-api:latest

if %errorlevel% neq 0 (
    echo ERROR: DOCKER PUSH FAILED !
    pause
    exit /b
)

echo =====================================
echo   TRIGGERING DEPLOY WEBHOOK
echo =====================================

curl -X POST http://15.134.201.47:9000/hooks/deploy-api

pause
