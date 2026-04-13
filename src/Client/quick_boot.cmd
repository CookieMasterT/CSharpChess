@echo off
TITLE Chess GUI Launcher

cd /d "%~dp0ChessGUI"

start /min npx serve

start http://localhost:3000

echo Chess GUI is now running.