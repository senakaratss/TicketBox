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

| <img src="https://github.com/user-attachments/assets/6c3e60ae-3169-4965-a447-7e18609adf7a" width="260"/> | <img src="https://github.com/user-attachments/assets/95755432-3866-4dc8-9b9d-d612df4d7279" width="260"/> | <img src="https://github.com/user-attachments/assets/52472bf3-86f5-4235-b8b2-7e2d0d650932" width="260"/> |
|:---:|:---:|:---:|
| **Anasayfa** | **Kullanıcı Dashboard** | **Etkinlik Detayı** |

| <img src="https://github.com/user-attachments/assets/d3a73ff9-b3a2-410f-997a-7ec34f3592fd" width="260"/> | <img src="https://github.com/user-attachments/assets/d971839e-41f2-4737-8046-7c9436da5673" width="260"/> | <img src="https://github.com/user-attachments/assets/2cc2f249-b39c-4f7d-8b23-7de6aec0bde1" width="260"/> |
|:---:|:---:|:---:|
| **Koltuk Seçimi (SignalR)** | **Booking Oluşturma (Koltuklu)** | **Rezervasyonlarım** |

| <img src="https://github.com/user-attachments/assets/4418aee9-3272-4504-b375-57026d658f8e" width="260"/> | <img src="https://github.com/user-attachments/assets/c3907efc-3298-481b-855a-f71484797870" width="260"/> | <img src="https://github.com/user-attachments/assets/964c58fc-c759-432a-9e16-b2313271eb9f" width="260"/> |
|:---:|:---:|:---:|
| **Booking'e ait Biletler** | **Biletlerim** | **Bilet Detayı (QR Kod)** |

| <img src="https://github.com/user-attachments/assets/d26f70a1-c11e-405d-92c9-a8865c11d79d" width="260"/> | <img src="https://github.com/user-attachments/assets/e444c421-b63a-4677-a3c7-4adee881410c" width="260"/> | <img src="https://github.com/user-attachments/assets/2ac6171e-4ba0-4f43-8e61-f0dd1fdf521b" width="260"/> |
|:---:|:---:|:---:|
| **Booking Oluşturma (Koltuksuz)** | **Etkinlik Listesi** | **Profilim** |

| <img src="https://github.com/user-attachments/assets/cb592e07-8382-49d8-aaa0-496dc107d811" width="260"/> | <img src="https://github.com/user-attachments/assets/66a539e6-f63c-4f5e-9030-53f0a8c80b81" width="260"/> | <img src="https://github.com/user-attachments/assets/bbd44f17-3869-4135-8a8e-5a9cadbdc838" width="260"/> |
|:---:|:---:|:---:|
| **Kayıt Ol** | **Giriş Yap** | **E-posta Onayı** |

| <img src="https://github.com/user-attachments/assets/efe1b94c-8225-4062-a0c3-acbcd11e9572" width="260"/> | <img src="https://github.com/user-attachments/assets/6cb88abe-ad9a-4b02-827f-7945200d3ac1" width="260"/> | <img src="https://github.com/user-attachments/assets/02445f75-3800-40ea-8e43-e330b334c6c2" width="260"/> |
|:---:|:---:|:---:|
| **Admin - Etkinlik Yönetimi** | **Admin - Etkinlik Düzenleme** | **Admin - Rezervasyon Yönetimi** |

| <img src="https://github.com/user-attachments/assets/c71cff2b-e0b7-4a30-bb9c-5c879945f42d" width="260"/> | <img src="https://github.com/user-attachments/assets/e1ffa993-d2b8-4d41-be3c-7d61818b4563" width="260"/> | <img src="https://github.com/user-attachments/assets/d5674009-2188-4a5e-83ac-8343f48b0758" width="260"/> |
|:---:|:---:|:---:|
| **Admin - Rezervasyon Detayı** | **Admin - Bilet Yönetimi** | **Admin - Kullanıcı Yönetimi** |

| <img src="https://github.com/user-attachments/assets/60fac1a9-2adc-4dd1-b44e-32ca486517cc" width="260"/> | <img src="https://github.com/user-attachments/assets/5f1ee3a0-0ab8-4d8b-9da7-18a0dd647924" width="260"/> | |
|:---:|:---:|:---:|
| **Admin - Kategori Yönetimi** | **Bilet E-postası** | |

---

<p align="center">❤️ ile geliştirildi — TicketBox</p>
