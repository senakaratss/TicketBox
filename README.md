
# 🎫 TicketBox

**TicketBox**, kullanıcıların etkinlikleri görüntüleyip dijital bilet satın alabildiği, koltuklu ve koltuksuz etkinlik türlerini destekleyen, gerçek zamanlı koltuk seçimi sunan modern bir **etkinlik biletleme platformu**dur.

Proje; **Onion Architecture** mimarisi ve **CQRS + MediatR** yaklaşımı kullanılarak geliştirilmiştir.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-blue?logo=dotnet)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-ORM-purple)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?logo=microsoftsqlserver)
![SignalR](https://img.shields.io/badge/SignalR-RealTime-orange)

---

## ✨ Özellikler

### Kullanıcı Tarafı
- 🔐 Kayıt olma / giriş yapma (ASP.NET Core Identity, Cookie Authentication)
- ✉️ Kayıt sonrası e-posta onayı (email confirmation)
- 🧑‍💻 Kullanıcıya özel dashboard (rezervasyonlar, biletler, hesap bilgileri)
- 📅 Etkinlikleri listeleme ve detay görüntüleme
- 🪑 **Koltuklu etkinliklerde** gerçek zamanlı koltuk seçim ekranı
- 🎟️ **Koltuksuz etkinliklerde** doğrudan bilet miktarı seçimi
- ⏳ Seçilen koltukların **10 dakika süreyle hold edilmesi**
- 📩 Rezervasyon sonrası bilet bilgilerinin e-posta ile gönderilmesi
- 🔳 Her bilette **benzersiz seri numarası** ve **QR kod**
- 🖼️ Dijital bilet görselinin oluşturulması ve görüntülenmesi
- 📊 Etkinlik kapasitesine göre kalan bilet/koltuk kontrolü

### Admin Paneli
- 🗂️ Kategori yönetimi (CRUD)
- 🎫 Etkinlik yönetimi (CRUD, koltuklu/koltuksuz ayarı)
- 👥 Kullanıcı yönetimi
- 📋 Rezervasyon/bilet yönetimi ve takibi
- 🛡️ Rol tabanlı erişim kontrolü (`Admin` rolü)

---

## 🛠 Kullanılan Teknolojiler

- **Web Framework**: ASP.NET Core MVC
- **Mimari**: Onion Architecture
- **CQRS + MediatR:** Command/Query ayrımı ve request'lerin ilgili Handler'lar üzerinden işlenmesi
- **ORM**: Entity Framework Core
- **Veritabanı**: SQL Server
- **Kimlik Doğrulama**: ASP.NET Core Identity + Cookie Authentication
- **Gerçek Zamanlı İletişim**: SignalR
- **QR Kod Üretimi**: QRCoder
- **Görsel İşleme (Bilet Tasarımı)**: SkiaSharp
- **E-posta Gönderimi**: MailKit
- **Yetkilendirme**: Role-based Authorization (`[Authorize(Roles = "Admin")]`)

---

## 🏛 Mimari

Proje, bağımlılıkların **dış katmanlardan iç katmanlara doğru aktığı** Onion Architecture prensibine göre katmanlandırılmıştır:

- **Domain**: Dış dünyaya bağımlılığı olmayan temel entity'leri ve domain modellerini içerir.
- **Application**: CQRS pattern ile Command/Query ayrımı, MediatR handler'ları, DTO'lar, validasyonlar ve uygulamaya ait iş akışlarını içerir.
- **Infrastructure**: EF Core DbContext, Identity yapılandırması, dış servis entegrasyonları (e-posta, QR, görsel işleme).
- **Web (Presentation)**: MVC Controller'lar, View'lar, SignalR Hub'ları ve kullanıcı arayüzü.

Bağımlılıkların yönü **dıştan içe** doğrudur; Domain katmanı dış katmanlara bağımlı değildir.

---

## 🪑 Koltuk Seçim Akışı (SignalR)

Koltuklu etkinliklerde bilet satın alma süreci şu şekilde işler:

1. Kullanıcı etkinlik detay sayfasından **koltuk seçim ekranına** yönlendirilir.
2. Sayfa açıldığında **SignalR Hub**'a bağlanılır ve etkinliğe ait koltuk durumları (boş / dolu / hold) gerçek zamanlı olarak alınır.
3. Kullanıcı bir koltuğu seçtiğinde:
   - Koltuk, o kullanıcı için **10 dakikalığına hold** edilir.
   - Hold bilgisi SignalR üzerinden **diğer bağlı kullanıcılara anlık olarak yayınlanır** (`Clients.Others.SendAsync`).
4. Süre dolmadan satın alma tamamlanmazsa, hold otomatik olarak kaldırılır ve koltuk tekrar müsait hale gelir.
5. Kullanıcının seçtiği koltuklar **Create Booking** akışına aktarılır ve seçilen koltuk sayısına göre `TicketQuantity` belirlenir.

> Koltuksuz etkinliklerde bu adım atlanır; kullanıcı doğrudan **Create Booking** sayfasına yönlendirilir ve istediği bilet miktarını seçer.

---

## 🎟 Bilet Oluşturma Süreci

1. **Kapasite Kontrolü**: Booking oluşturulmadan önce etkinliğin toplam kapasitesi ile satılmış bilet sayısı karşılaştırılarak kalan kapasite hesaplanır. Yetersiz kapasite durumunda işlem reddedilir.
2. **Booking Oluşturma**: CQRS kapsamında oluşturulan `CreateBookingCommand`, **MediatR** aracılığıyla CreateBookingCommandHandler'a iletilir.
3. **Bilet Üretimi**:
   - Her bilet için **benzersiz seri numarası** üretilir.
   - **QRCoder** ile bilete özel QR kod oluşturulur.
   - **SkiaSharp** kullanılarak seri numarası, QR kod ve etkinlik bilgilerini içeren dijital **bilet görseli** render edilir.
4. **E-posta Gönderimi**: **MailKit** ile kullanıcıya bilet bilgilerini içeren onay e-postası gönderilir.
5. **Bilet Görüntüleme**: Kullanıcı, hesabından geçmiş ve aktif biletlerini görüntüleyebilir.

---

## 🔐 Yetkilendirme (Authorization)

| Alan                             | Erişim Kuralı                              |
|------------------------------------|---------------------------------------------|
| Etkinlik listeleme/detay           | Herkese açık                                |
| Bilet satın alma, biletlerim       | `[Authorize]` (giriş yapmış kullanıcı)      |
| Admin paneli (tüm controller'lar)  | `[Authorize(Roles = "Admin")]`             |

Kimlik doğrulama, **ASP.NET Core Identity** ile **Cookie Authentication** üzerinden sağlanır. Giriş yapan kullanıcının rolüne göre admin alanlarına erişimi kısıtlanır.

---

## 🖼 Ekran Görüntüleri
 
| <img src="https://github.com/user-attachments/assets/6ae9def3-1509-460f-89bf-5f823a988187" width="260"/> | <img src="https://github.com/user-attachments/assets/5c015f50-06df-4d9a-ab9c-2917982fdcac" width="260"/> | <img src="https://github.com/user-attachments/assets/8a206f73-8185-49e4-a2b8-c50793a8a51c" width="260"/> |
|:---:|:---:|:---:|
| **Anasayfa** | **Kullanıcı Dashboard** | **Etkinlik Detayı** |
 
| <img src="https://github.com/user-attachments/assets/907eb5c3-01f5-4f67-aa5a-8fa7813c2b2c" width="260"/> | <img src="https://github.com/user-attachments/assets/f5dd5a2a-8029-4e6a-9b4e-eef4529a0e4f" width="260"/> | <img src="https://github.com/user-attachments/assets/d41b1847-b1d0-43af-915c-eb7cc155eb9b" width="260"/> |
|:---:|:---:|:---:|
| **Koltuk Seçimi (SignalR)** | **Booking Oluşturma (Koltuklu)** | **Rezervasyonlarım** |
 
| <img src="https://github.com/user-attachments/assets/d55d56aa-3b36-4ea3-a2f9-a94f13f07e16" width="260"/> | <img src="https://github.com/user-attachments/assets/e3726361-c6e8-4ed6-afd0-e7af59e4c34d" width="260"/> | <img src="https://github.com/user-attachments/assets/4a446519-d04f-46b4-ba06-24d9d68bede0" width="260"/> |
|:---:|:---:|:---:|
| **Booking'e ait Biletler** | **Biletlerim** | **Bilet Detayı (QR Kod)** |
 
| <img src="https://github.com/user-attachments/assets/c97cd98a-ac78-4321-a401-8dfcc14a9773" width="260"/> | <img src="https://github.com/user-attachments/assets/01cc38cb-e39b-4c42-b37b-2167523c8077" width="260"/> | <img src="https://github.com/user-attachments/assets/305a1443-3f39-4804-b93e-4c276b9019fb" width="260"/> |
|:---:|:---:|:---:|
| **Booking Oluşturma (Koltuksuz)** | **Etkinlik Listesi** | **Profilim** |
 
| <img src="https://github.com/user-attachments/assets/d416d3b4-cdc9-41f5-bc28-2c27a141a56e" width="260"/> | <img src="https://github.com/user-attachments/assets/d8e76bd7-70a7-48ce-af76-93e24bcaf7fc" width="260"/> | <img src="https://github.com/user-attachments/assets/28f2521e-0187-4376-ae51-a343da6e5f14" width="260"/> |
|:---:|:---:|:---:|
| **Kayıt Ol** | **Giriş Yap** | **E-posta Onayı** |
 
| <img src="https://github.com/user-attachments/assets/35547d9b-292f-4cb5-8ef7-eaaa0f1b811e" width="260"/> | <img src="https://github.com/user-attachments/assets/d9f0827c-fd3b-45a7-bd74-6739bc8a546e" width="260"/> | <img src="https://github.com/user-attachments/assets/f52f4b6e-b463-4f29-b2c8-604510a1ec7f" width="260"/> |
|:---:|:---:|:---:|
| **Admin - Etkinlik Yönetimi** | **Admin - Etkinlik Düzenleme** | **Admin - Rezervasyon Yönetimi** |
 
| <img src="https://github.com/user-attachments/assets/13b17faf-cafa-4c23-86d1-af6b08988be6" width="260"/> | <img src="https://github.com/user-attachments/assets/37aee0d4-144d-4105-82a2-f89ab11425c9" width="260"/> | <img src="https://github.com/user-attachments/assets/8af72802-6340-43e1-a69a-74be9ab5e961" width="260"/> |
|:---:|:---:|:---:|
| **Admin - Rezervasyon Detayı** | **Admin - Bilet Yönetimi** | **Admin - Kullanıcı Yönetimi** |
 
| <img src="https://github.com/user-attachments/assets/2fac58db-a338-40ea-aafb-34809765e807" width="260"/> | <img src="https://github.com/user-attachments/assets/35432adc-ce1e-4dae-bfbe-2071e6d36026" width="260"/> | |
|:---:|:---:|:---:|
| **Admin - Kategori Yönetimi** | **Bilet E-postası** | |
 
---

<p align="center">❤️ ile geliştirildi — TicketBox</p>



<p align="center">❤️ ile geliştirildi — TicketBox</p>
