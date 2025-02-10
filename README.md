# MotelBooking API
## 🎥 Demonstração do Projeto

Assista ao vídeo explicativo: [Clique aqui](https://www.youtube.com/watch?v=IeGipsYI3gw)

## Objetivo do Projeto
Desenvolvimento de uma API RESTful completa para gerenciamento de motéis, quartos, reservas e usuários. A API implementa autenticação JWT, otimização de queries SQL e uso eficiente de cache, com foco em segurança, performance e uma arquitetura escalável.

### Funcionalidades Principais:
- Cadastro e login de usuários (com JWT)
- Gerenciamento de motéis, quartos e reservas
- Faturamento mensal
- Consulta de perfil de usuários
- Filtragem e listagem de reservas
- Endpoints otimizados para performance e segurança

## Tecnologias Utilizadas

**Backend:**
- .NET 8
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Memory Cache

**Infraestrutura:**
- Docker & Docker Compose
- Swagger/OpenAPI
- GitHub Actions (CI/CD)

## Arquitetura

```plaintext
MotelBookingAPI/
 ┣ Controllers/
 ┃ ┣ AuthController.cs
 ┃ ┣ UsersController.cs
 ┃ ┣ ReservasController.cs
 ┃ ┣ MoteisController.cs
 ┃ ┣ QuartosController.cs
 ┃ ┗ FaturamentoController.cs
 ┣ Models/
 ┣ Data/
 ┣ Services/
 ┣ Migrations/
 ┣ Startup.cs (ou Program.cs no .NET 7/8)
 ┣ docker-compose.yml
 ┣ Dockerfile
 ┗ README.md
```

## Como Executar

### Pré-requisitos
- Docker & Docker Compose
- .NET SDK 8 (opcional)
- PostgreSQL (opcional)

### Usando Docker (Recomendado)

1. Clone o repositório:

```bash
git clone https://github.com/seu-usuario/motelbooking-api.git
cd motelbooking-api
```

2. Configure o ambiente criando um arquivo ```.env``` na raiz do projeto:

```bash
POSTGRES_DB=MotelBookingDb
POSTGRES_USER=myUser
POSTGRES_PASSWORD=myPassword
JWT_KEY=segredo_super_secreto
JWT_ISSUER=motelbooking
JWT_AUDIENCE=motelbooking_users
```

3. Execute com Docker Compose:

```
docker-compose up --build -d
```

### Usando Localmente (Sem Docker)

1. Configure o Banco de Dados:
- Se estiver usando PostgreSQL local, crie um banco de dados conforme definido nas variáveis de ambiente.
2. Execute as Migrations:

```
dotnet ef database update
```

3. Inicie a Aplicação:

```
dotnet run
```

A API estará disponível em http://localhost:5000 ou http://localhost:8081, dependendo da configuração.

## Endpoints Principais

| Recurso      | Método  | Endpoint                            | Descrição                                      |
|--------------|---------|-------------------------------------|------------------------------------------------|
| **Faturamento** | **GET** | `/api/Faturamento/mensal`          | Obtém o faturamento mensal                     |
| **Motéis**      | **POST** | `/api/Moteis/adicionar`            | Adiciona um novo motel                        |
|              | **PUT**  | `/api/Moteis/editar/{id}`          | Edita os dados de um motel                    |
|              | **DELETE** | `/api/Moteis/excluir/{id}`        | Exclui um motel                               |
|              | **GET**  | `/api/Moteis/listar`               | Lista todos os motéis                         |
| **Quartos**     | **POST** | `/api/Quartos/adicionar`           | Adiciona um novo quarto                       |
|              | **GET**  | `/api/Quartos/obter/{id}`          | Obtém os detalhes de um quarto                |
|              | **PUT**  | `/api/Quartos/editar/{id}`         | Edita os dados de um quarto                   |
|              | **DELETE** | `/api/Quartos/excluir/{id}`       | Exclui um quarto                              |
|              | **GET**  | `/api/Quartos/listar`              | Lista todos os quartos                        |
| **Reservas**    | **POST** | `/api/Reservas/adicionar`          | Adiciona uma nova reserva                     |
|              | **GET**  | `/api/Reservas/listar`             | Lista todas as reservas                       |
|              | **GET**  | `/api/Reservas/mensal`             | Obtém todas as reservas do mês                |
|              | **GET**  | `/api/Reservas/{id}`               | Obtém os detalhes de uma reserva específica   |
| **Usuários**    | **POST** | `/api/Usuarios/login`              | Login do usuário (gera token JWT)             |
|              | **POST** | `/api/Usuarios/adicionar`          | Adiciona um novo usuário                      |
|              | **PUT**  | `/api/Usuarios/editar/{id}`        | Edita os dados de um usuário                  |
|              | **DELETE** | `/api/Usuarios/excluir/{id}`      | Exclui um usuário                             |
|              | **GET**  | `/api/Usuarios/listar`             | Lista todos os usuários                       |
|              | **GET**  | `/api/Usuarios/perfil`             | Obtém o perfil do usuário autenticado         |

### Segurança
- JWT para autenticação
- Proteção contra SQL Injection
- Validação de dados
- Cache otimizado


### Performance
- Cache em memória para consultas frequentes
- Eager Loading otimizado
- Queries parametrizadas
- Índices no banco de dados

### Melhorias Futuras
- Cache distribuído com Redis
- Testes de integração
- Logging avançado
- Frontend em React
- CI/CD completo

#### Autor
Marcos Roberto Longhi Junior

**📧 marcosrlj@hotmail.com**
