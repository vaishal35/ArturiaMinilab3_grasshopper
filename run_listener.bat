@echo off
python -c "import sys" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Python is not installed. Please install Python 3 from https://python.org.
    exit /B 1
)
"%~dp0minilab_listener.py" %*
