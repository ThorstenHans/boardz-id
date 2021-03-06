FROM microsoft/dotnet:2.0.0-sdk as build-environment

LABEL maintainer="Thorsten Hans <thorsten.hans@gmail.com>"

COPY ./code/Boardz.Id.csproj /app/
WORKDIR /app
RUN dotnet restore
COPY ./code/ /app/
ENV IdSrvUserName=th IdSrvPassword=th IdSrvSubject=1
RUN dotnet publish -c Release -o out Boardz.Id.csproj

FROM microsoft/aspnetcore:2.0.0
WORKDIR /app
COPY --from=build-environment /app/out/ ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Boardz.Id.dll"]