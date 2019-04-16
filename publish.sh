#!/bin/bash
outpath=`pwd`
outpath=$outpath/light
echo -e "\n发布目录：$outpath \n"
echo -e "请用cd命令切换到 $outpath 目录下，然后执行 ./BreatheLight 二进制目标文件来启动运行。\n"
dotnet publish -c release -r linux-x64 --self-contained -o $outpath ./src/BreatheLight/BreatheLight.csproj