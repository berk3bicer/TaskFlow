# TaskFlow API

Modern .NET 10 ile geliştirilmiş, Clean Architecture ve CQRS prensipleriyle yazılmış bir proje ve görev yönetim sistemi REST API'si.

## Mimari

Clean Architecture — 4 katmanlı yapı. Bağımlılıklar her zaman içeri doğru akar; Domain hiçbir şeye bağımlı değildir.

- **TaskFlow.Domain** — Entity'ler ve iş kuralları. Saf C#, dış bağımlılık yok. Her entity kendi kurallarını metotlarıyla korur (`AssignTo`, `Edit`, `UpdateDetails` gibi).
- **TaskFlow.Application** — CQRS handler'ları (MediatR), use case'ler, FluentValidation validator'ları. Her özellik vertical slice olarak organize edilir: Command/Query + Handler + (gerekirse) Validator.
- **TaskFlow.Infrastructure** — EF Core (PostgreSQL), JWT üretimi, şifre hashleme.
- **TaskFlow.API** — Controller'lar, JWT authentication, merkezi exception handling.

### Tasarım kararları

- **CQRS (MediatR):** Her işlem bir Command (yazma) veya Query (okuma) olarak modellenir, kendi handler'ında işlenir.
- **Yetki handler'da:** Kimlik (`RequesterId`) controller'da JWT'den (`ClaimTypes.NameIdentifier`) okunur ve command'e enjekte edilir. Client kimlik göndermez — bu, kullanıcının başkası adına işlem yapmasını (IDOR) engeller.
- **Resource-based authorization:** Yetki, kullanıcının kaynakla ilişkisine göre belirlenir (proje sahibi, yorum yazarı, task'a atanan kişi). Rol tabanlı değil.
- **Merkezi exception handling:** Domain exception'ları HTTP durum kodlarına maplenir — `NotFoundException` → 404, `ForbiddenException` → 403, `ConflictException` → 409, `DomainException` → 400.

## Teknolojiler

- .NET 10, ASP.NET Core
- Entity Framework Core (PostgreSQL)
- MediatR (CQRS)
- FluentValidation
- JWT (Bearer authentication)
- Scalar (API dokümantasyonu / test arayüzü)
- xUnit + EF Core InMemory (testler)

## Kurulum

### Gereksinimler

- .NET 10 SDK
- PostgreSQL

### Adımlar

1. Repoyu klonlayın:
   ```bash
   git clone https://github.com/berk3bicer/TaskFlow.git
   cd TaskFlow
   ```

2. PostgreSQL'de `taskflow` adında bir veritabanı oluşturun.

3. Hassas yapılandırmayı user-secrets ile tanımlayın (bu değerler repoya girmez):
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=taskflow;Username=postgres;Password=ŞİFRENİZ" --project src/TaskFlow.API

   dotnet user-secrets set "Jwt:Key" "EN_AZ_32_KARAKTERLİK_GİZLİ_BİR_ANAHTAR" --project src/TaskFlow.API
   ```

4. Veritabanı şemasını oluşturun (migration'ları uygulayın):
   ```bash
   dotnet ef database update --project src/TaskFlow.Infrastructure --startup-project src/TaskFlow.API
   ```

5. API'yi çalıştırın:
   ```bash
   dotnet run --project src/TaskFlow.API
   ```

6. Scalar arayüzüne tarayıcıdan erişin (geliştirme ortamında):
   ```
   https://localhost:<port>/scalar/v1
   ```

## Endpoint'ler

Auth dışındaki tüm endpoint'ler JWT gerektirir (`Authorization: Bearer <token>`).

### Auth

| Metot | Rota | Açıklama |
|-------|------|----------|
| POST | `/api/auth/register` | Kayıt ol, JWT al |
| POST | `/api/auth/login` | Giriş yap, JWT al |

### Projects

| Metot | Rota | Açıklama | Yetki |
|-------|------|----------|-------|
| POST | `/api/projects` | Proje oluştur | Giriş yapan kullanıcı sahip olur |
| GET | `/api/projects` | Kullanıcının projelerini listele | Sahip |
| DELETE | `/api/projects/{projectId}` | Projeyi sil (task ve yorumlar cascade ile silinir) | Sahip |

### Tasks

| Metot | Rota | Açıklama | Yetki |
|-------|------|----------|-------|
| POST | `/api/tasks` | Task oluştur | Proje sahibi |
| GET | `/api/tasks/{projectId}` | Projenin task'larını listele | Sahip tümünü, atanan kendininkini |
| PUT | `/api/tasks/{taskId}` | Task durumunu güncelle | Sahip veya atanan |
| PUT | `/api/tasks/{taskId}/assign` | Task'ı bir kullanıcıya ata | Proje sahibi |
| PUT | `/api/tasks/{taskId}/details` | Task başlık/açıklama/tarih güncelle | Proje sahibi |
| DELETE | `/api/tasks/{taskId}` | Task'ı sil | Proje sahibi |

### Comments

| Metot | Rota | Açıklama | Yetki |
|-------|------|----------|-------|
| POST | `/api/tasks/{taskId}/comments` | Yorum ekle | Proje sahibi veya atanan |
| GET | `/api/tasks/{taskId}/comments` | Yorumları listele | Proje sahibi veya atanan |
| PUT | `/api/tasks/{taskId}/comments/{commentId}` | Yorumu düzenle | Yazar |
| DELETE | `/api/tasks/{taskId}/comments/{commentId}` | Yorumu sil | Yazar veya proje sahibi (moderasyon) |

## Veritabanı ilişkileri

Cascade davranışları EF Core'da yapılandırılmıştır:

- **Project → Tasks:** Cascade (proje silinince task'ları da silinir)
- **Task → Comments:** Cascade (task silinince yorumları da silinir)
- **Comment → Author (User):** Restrict (yorumu olan kullanıcı doğrudan silinemez)
- **Task → AssignedTo (User):** SetNull (atanan kullanıcı silinirse task'ın ataması boşalır)

## Testler

xUnit ile yazılmış 33 unit test:

- **Domain testleri:** Entity iş kurallarını doğrular (boş/uzun içerik, geçersiz tarih, durum geçişleri).
- **Handler testleri:** Yetki mantığını EF Core InMemory provider ile doğrular — her handler için yetkili erişim, yetkisiz erişimde `ForbiddenException` ve bulunamayan kaynakta `NotFoundException` senaryoları.

Testleri çalıştırmak için:

```bash
dotnet test
```

## Lisans

MIT