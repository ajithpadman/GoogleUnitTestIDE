@echo off
make clean -j4 
make all -j4> gnumake.out 2>error.txt
exit