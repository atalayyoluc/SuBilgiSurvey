# SuBilgiSurvey

- **Backend:** [SuBilgiSurvey.Backend](SuBilgiSurvey.Backend/) — `dotnet run --project SuBilgiSurvey.Backend/SuBilgiSurveyBackend.Api` (http://localhost:5083)
- **Frontend:** [SuBilgiSurvey.UI](SuBilgiSurvey.UI/) — `cd SuBilgiSurvey.UI && npm run dev` (http://localhost:3000)

`.env.local` içinde `NEXT_PUBLIC_API_URL=http://localhost:5083` olmalı.

Kökte kalan boş / eski `SuBilgiSurveyBackend.Api` veya `subilgisurvey-ui` klasörlerini silebilirsiniz.

## Docker

**Tam yığın** (Postgres 5432, API 5083, UI 3000) — API açılışta migration uygular:

```bash
docker compose up --build
```

**Sadece PostgreSQL** (yerel API/UI için):

```bash
docker compose -f docker-compose.db.yml up -d
```

Yerel bağlantı: `Host=localhost;Database=SuBilgiSurvey;Username=postgres;Password=postgres`


## 🚀 Canlı Demo
**Link:** [demo-subilgisurvey.atalayyoluc.com](https://demo-subilgisurvey.atalayyoluc.com/)

### 🔑 Test Kimlik Bilgileri
| Rol | E-posta | Şifre |
| :--- | :--- | :--- |
| **Admin** | `atalayyoluc@gmail.com` | `Atalay+1` |
| **User** | `testuser@gmail.com` | `Atalay+1` |

---
