# SuBilgiSurvey Backend API (.NET 8)
# Build + runtime aynı sdk:8.0 tabanında: aspnet:8.0 ayrı çekilirken oluşan
# "short read / unexpected EOF" (bozuk katman) sorunlarını önler; imaj biraz daha büyüktür.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SuBilgiSurveyBackend.sln ./
COPY SuBilgiSurveyBackend.Core/SuBilgiSurveyBackend.Core.csproj SuBilgiSurveyBackend.Core/
COPY SuBilgiSurveyBackend.Application/SuBilgiSurveyBackend.Application.csproj SuBilgiSurveyBackend.Application/
COPY SuBilgiSurveyBackend.Infrastructure/SuBilgiSurveyBackend.Infrastructure.csproj SuBilgiSurveyBackend.Infrastructure/
COPY SuBilgiSurveyBackend.Api/SuBilgiSurveyBackend.Api.csproj SuBilgiSurveyBackend.Api/
RUN dotnet restore SuBilgiSurveyBackend.Api/SuBilgiSurveyBackend.Api.csproj

COPY SuBilgiSurveyBackend.Core/ SuBilgiSurveyBackend.Core/
COPY SuBilgiSurveyBackend.Application/ SuBilgiSurveyBackend.Application/
COPY SuBilgiSurveyBackend.Infrastructure/ SuBilgiSurveyBackend.Infrastructure/
COPY SuBilgiSurveyBackend.Api/ SuBilgiSurveyBackend.Api/

RUN dotnet publish SuBilgiSurveyBackend.Api/SuBilgiSurveyBackend.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5083
EXPOSE 5083
ENTRYPOINT ["dotnet", "SuBilgiSurveyBackend.Api.dll"]
