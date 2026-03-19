#!/usr/bin/env bash
set -euo pipefail

RESULTS_DIR="ReqnrollLogin.Tests/bin/Debug/net9.0/allure-results"

echo "==> Running tests..."
dotnet test MedeferQATechTest.sln

echo "==> Launching Allure report..."
allure serve "$RESULTS_DIR"
