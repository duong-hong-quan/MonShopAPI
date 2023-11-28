#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MonShop.Controller/MonShop.Controller.csproj", "MonShop.Controller/"]
COPY ["MonShop.Library/MonShop.Library.csproj", "MonShop.Library/"]
COPY ["MonShop.Payment/MonShop.Payment.csproj", "MonShop.Payment/"]
COPY ["MonShop.Realtime/MonShop.Realtime.csproj", "MonShop.Realtime/"]
RUN dotnet restore "MonShop.Controller/MonShop.Controller.csproj"
COPY . .
WORKDIR "/src/MonShop.Controller"
RUN dotnet build "MonShop.Controller.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MonShop.Controller.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonShop.Controller.dll"]