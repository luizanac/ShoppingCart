FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . /src/ShoppingCart
WORKDIR "/src/ShoppingCart"
RUN dotnet restore "ShoppingCart.csproj"
RUN dotnet build "ShoppingCart.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingCart.csproj" --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
EXPOSE 80 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingCart.dll"]