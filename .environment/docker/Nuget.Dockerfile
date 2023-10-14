FROM mono
RUN apt update && apt install -y wget
COPY nuget /usr/bin/nuget
RUN wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe && \
    mv nuget.exe /usr/local/bin/nuget.exe && \
    chmod +x /usr/bin/nuget