# Etapa 1: Build da aplicação com SDK completo
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Instalar a ferramenta Entity Framework Core CLI
# Necessária para migrations e atualizações do banco
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copiar apenas o arquivo de projeto primeiro
# Isso permite cachear as dependências separadamente
COPY *.csproj ./
RUN dotnet restore

# Copiar todo o código fonte e compilar
# O publish gera os binários otimizados para produção
COPY . .
RUN dotnet publish -c Release -o out

# Etapa 2: Imagem de produção com runtime mínimo
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copiar apenas os arquivos necessários do estágio de build
# Reduz significativamente o tamanho final da imagem
COPY --from=build /app/out .

# Configurar a porta HTTP padrão
EXPOSE 80

# Comando para iniciar a aplicação
# Usa o assembly principal gerado no publish
ENTRYPOINT ["dotnet", "MotelBookingApi.dll"]
