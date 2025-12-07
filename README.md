# 🧮 PosOptimizer – POS Komisyon Oranı Hesaplama ve Optimizasyon Servisi

PosOptimizer; POS sağlayıcılarının komisyon oranlarını toplayan, Redis üzerinde cache’leyen, SQL Server’da saklayan, API üzerinden en uygun POS oranını hesaplayan ve background job ile oranları düzenli güncelleyen modern bir .NET projesidir.

Tamamen Dockerize çalışır ve unit testlerle desteklenmiştir.

---

## 🚀 Özellikler

- POS oranlarını dış API’den alır
- Redis üzerinde 1 günlük cache mekanizması
- SQL Server'a veri kaydı
- API üzerinden gerçek zamanlı POS hesaplaması
- Background Worker ile ratio senkronizasyonu
- Docker Compose ile tüm servisler tek komutla ayağa kalkar
- NUnit + Moq test altyapısı
- Migration otomatik + retry mekanizmalı çalışır

---

## 📁 Proje Yapısı

src/ <br>
├── PosOptimizer.Api              → API katmanı <br>
├── PosOptimizer.Application      → Business logic & domain servisleri <br>
├── PosOptimizer.Infrastructure   → EF Core, SQL, repository <br>
├── PosOptimizer.Job              → Background worker (ratio fetch) <br>
├── PosOptimizer.MockApiClient    → Mock ratio provider client <br>
├── PosOptimizer.Common           → Ortak modeller & yardımcı sınıflar <br>
tests/ <br>
└── PosOptimizer.Tests            → NUnit + Moq unit testleri <br>
docker-compose.yml                 → Tüm servislerin orkestrasyonu <br>
Dockerfile.api                     → API Docker imajı <br>
Dockerfile.job                     → Job Docker imajı <br>

---

## ⚙️ Kurulum – Tek Komut

Tüm sistemi başlatmak için:

docker-compose up --build

### Çalışan Servisler:

| Servis | Port | Açıklama |
|--------|------|----------|
| API | 7170 | POS hesaplama endpointi |
| SQL Server | 1433 | EF Core DB |
| Redis | 6379 | Cache |
| Job | background | Ratio senkronizasyonu |

---

## 🗄️ Migration Mekanizması

API ayağa kalkarken migration otomatik çalışır:

```csharp
using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  db.Database.Migrate();
}
```

SQL Server geç açılırsa retry mekanizması devreye girer.

---

## 🔁 Background Job – POS Ratio Senkronizasyonu

Background worker her çalıştığında:

1. Redis'teki POS ratio cache silinir
2. Mock API’den yeni ratio verileri alınır
3. Redis’e tekrar set edilir (TTL = 1 gün)
4. SQL Server’a kayıt yapılır

---

## 🧠 POS Hesaplama Mantığı

### ✔ Komisyon:
```
commission = amount × rate
```

TRY dışındaki para birimlerinde:

```
commission = commission × 1.01
```

### ✔ Cost:
```
cost = max(commission, minFee)
```

### ✔ POS Seçim Sıralaması:
1. En düşük cost
2. Priority yüksek
3. CommissionRate düşük
4. POS adı alfabetik

---

## 🔌 API Kullanımı

### Endpoint:
```
POST /calculate-post
```

### Request Body:
```json
{
"amount": 100,
"installment": 3,
"currency": "TRY"
}
```

### Response:
```json
{
"success": true,
"data": {
  "posName": "BankA",
  "cost": 5.30,
  "commission": 4.90
 }
}
```

---

## 🧪 Unit Testler

Testler NUnit + Moq ile yazılmıştır.

Test konuları:

- Redis cache hit & miss
- API hesaplamaları
- Commission & Cost algoritması
- TRY dışı multiplier
- ErrorCode senaryoları
- Background job insert + cache davranışı

Testleri çalıştırmak için:

```bash
dotnet test
```

---

## 🐳 Docker Compose Bileşenleri

Tüm servisleri başlatmak için:

```bash
docker-compose up --build
```

---

## 📄 Lisans

MIT License
EOF
