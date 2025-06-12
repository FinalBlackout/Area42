```mermaid
classDiagram
    %% Interfaces
    interface IAccommodatieService {
      + Task<List<Accommodatie>> GetAllAccommodatiesAsync()
      + Task AddAccommodatieAsync(Accommodatie acc)
      + Task DeleteAccommodatieAsync(int id)
    }
    interface IAccommodatieRepository {
      + Task<List<Accommodatie>> GetAllAccommodatiesAsync()
      + Task AddAccommodatieAsync(Accommodatie acc)
      + Task DeleteAccommodatieAsync(int id)
    }
    interface IReserveringService {
      + Task<List<Reservering>> GetReserveringenAsync(String userId = null)
      + Task<Reservering> GetReserveringByIdAsync(int id)
      + Task CreateReserveringAsync(Reservering r)
      + Task UpdateReserveringAsync(Reservering r)
      + Task DeleteReserveringAsync(int id)
      + Task ApproveReserveringAsync(int id)
      + Task RejectReserveringAsync(int id)
    }
    interface IReserveringRepository {
      + Task<List<Reservering>> GetReserveringenAsync(String userId = null)
      + Task<Reservering> GetReserveringByIdAsync(int id)
      + Task AddAsync(Reservering r)
      + Task UpdateAsync(Reservering r)
      + Task DeleteAsync(int id)
      + Task ApproveAsync(int id)
      + Task RejectAsync(int id)
    }
    interface IUserService {
      + Task<User?> GetUserByUsernameAsync(String username)
    }
    interface IUserRepository {
      + Task<User?> GetUserByUsernameAsync(String username)
    }

    %% Services
    class AccommodatieService {
      - IAccommodatieRepository _repo
      + AccommodatieService(IAccommodatieRepository repo)
      + GetAllAccommodatiesAsync()
      + AddAccommodatieAsync(acc)
      + DeleteAccommodatieAsync(id)
    }
    class ReserveringService {
      - IReserveringRepository _repo
      + ReserveringService(IReserveringRepository repo)
      + GetReserveringenAsync(userId)
      + GetReserveringByIdAsync(id)
      + CreateReserveringAsync(r)
      + UpdateReserveringAsync(r)
      + DeleteReserveringAsync(id)
      + ApproveReserveringAsync(id)
      + RejectReserveringAsync(id)
    }
    class UserService {
      - IUserRepository _repo
      + UserService(IUserRepository repo)
      + GetUserByUsernameAsync(username)
    }

    %% Repositories
    class AccommodatieRepository {
      - string _connectionString
      + AccommodatieRepository(IConfiguration config)
      + GetAllAccommodatiesAsync()
      + AddAccommodatieAsync(acc)
      + DeleteAccommodatieAsync(id)
    }
    class ReserveringRepository {
      - string _cs
      + ReserveringRepository(IConfiguration config)
      + GetReserveringenAsync(userId)
      + GetReserveringByIdAsync(id)
      + AddAsync(r)
      + UpdateAsync(r)
      + DeleteAsync(id)
      + ApproveAsync(id)
      + RejectAsync(id)
    }
    class UserRepository {
      - string _connectionString
      + UserRepository(IConfiguration config)
      + GetUserByUsernameAsync(username)
    }

    %% Entities
    class Accommodatie {
      + int Id
      + string Naam
      + string Type
      + int Capaciteit
      + string Beschrijving
      + decimal PrijsPerNacht
      + string Faciliteiten
      + string Status
      + string ImagePath
    }
    class Reservering {
      + int Id
      + int AccommodatieId
      + int UserId
      + DateTime Startdatum
      + DateTime Einddatum
      + string Status
    }
    class User {
      + int Id
      + string Username
      + string Role
    }

    %% Relaties
    IAccommodatieService <|.. AccommodatieService
    IAccommodatieRepository <|.. AccommodatieRepository
    AccommodatieService --> IAccommodatieRepository

    IReserveringService <|.. ReserveringService
    IReserveringRepository <|.. ReserveringRepository
    ReserveringService --> IReserveringRepository

    IUserService <|.. UserService
    IUserRepository <|.. UserRepository
    UserService --> IUserRepository
    ```
