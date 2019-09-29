# build client	
FROM node:10 as BUILD_CLIENT
COPY ./frontend ./app
WORKDIR /app
RUN npm install	
RUN npm run build

# build server	
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS BUILD_SERVER
COPY ./ ./app
WORKDIR /app
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# copy client and server
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
COPY --from=BUILD_SERVER /app/out /app
COPY --from=BUILD_CLIENT /app/dist /app/wwwroot

# run
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "Waremap.dll"]