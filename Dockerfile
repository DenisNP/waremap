# build client	
FROM node:10 as BUILD_CLIENT
COPY ./frontend .
RUN npm install	
RUN npm run build

# build server	
FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100 AS BUILD_SERVER
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# copy client and server
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
COPY --from=BUILD_SERVER ./out /app
COPY --from=BUILD_CLIENT ./dist /app/wwwroot

# run
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "Waremap.dll"]