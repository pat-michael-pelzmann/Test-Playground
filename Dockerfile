FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /src

COPY . .

RUN dotnet restore Test4QA.slnx

RUN dotnet build Test4QA.slnx --no-restore

CMD ["dotnet", "test", "Test4QA.slnx", "--no-build", "--logger:trx;LogFileName=TestResults/tests.trx"]