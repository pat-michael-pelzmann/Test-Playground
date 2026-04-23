FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /src

COPY . .

RUN dotnet restore Test4QA.sln

RUN dotnet build Test4QA.sln --no-restore

CMD ["dotnet", "test", "Test4QA.sln", "--no-build", "--logger:trx;LogFileName=TestResults/tests.trx"]