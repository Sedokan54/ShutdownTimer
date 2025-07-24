# 🕐 Shutdown Timer

Modern, kullanıcı dostu Windows uygulaması ile bilgisayarınızı zamanlayarak kapatın, yeniden başlatın veya uyku moduna alın.

## ✨ Özellikler

- **🔄 Çoklu Sistem İşlemleri**: Kapatma, yeniden başlatma ve uyku modu desteği
- **⏰ Esnek Zamanlayıcı**: Saat, dakika ve saniye cinsinden zamanlama
- **📊 Görsel Geri Sayım**: Gerçek zamanlı geri sayım ve ilerleme çubuğu
- **📋 Sistem Tepsisi Entegrasyonu**: Sistem tepsisinde çalışma ve arka plan desteği
- **⚠️ Uyarı Sistemi**: Son sanyelerde görsel ve sesli uyarılar
- **🎨 Modern Arayüz**: ModernWPF ile temiz, modern tasarım
- **🌙 Tema Desteği**: Sistem temasına otomatik uyum (Açık/Koyu)
- **🚀 Tek Tık Çalıştırma**: Kurulum gerektirmez, çift tıkla çalışır
- **📦 Taşınabilir**: Tüm bağımlılıklar dahil, tek dosya

## 🚀 Hızlı Başlangıç

### Tek Tıkla Kullanım
1. `build-release.bat` dosyasını çalıştırın (sadece ilk sefer)
2. Oluşan `ShutdownTimer.exe` dosyasını çift tıklayın
3. Kullanıma hazır! ✅

### Manuel İndirme
1. [Releases](../../releases) sayfasından en son sürümü indirin
2. `ShutdownTimer.exe` dosyasını istediğiniz klasöre kopyalayın
3. Çift tıklayarak çalıştırın

## 📋 Sistem Gereksinimleri

- Windows 10/11 (64-bit)
- Herhangi bir ek yazılım gerektirmez!

## 📖 Kullanım

1. **🎯 İşlem Seçin**: Kapatma, Yeniden Başlatma veya Uyku modu arasından seçim yapın
2. **⏱️ Süre Belirleyin**: Saat, dakika ve saniye kutularını kullanın
3. **▶️ Başlatın**: "Başlat" butonuna tıklayarak geri sayımı başlatın
4. **👀 Takip Edin**: Gerçek zamanlı geri sayım ve ilerleme çubuğunu izleyin
5. **❌ İptal Edin**: "İptal" butonu veya sistem tepsisi menüsüyle durdurun

### ⚠️ Uyarı Sistemi

- **30 saniye**: Turuncu geri sayım rengi
- **10 saniye**: Kırmızı yanıp sönen geri sayım
- **🔊 Ses Uyarıları**: Son 10 saniyede 2 saniyede bir ses uyarısı
- **💬 Bildirimler**: Önemli anlarda sistem tepsisi bildirimleri

## 🛠️ Kaynak Koddan Derleme

### Gereksinimler
- Visual Studio 2022 veya üzeri
- .NET 6.0 SDK veya üzeri

### Derleme Adımları
1. Bu repository'yi klonlayın
2. `ShutdownTimer.sln` dosyasını Visual Studio'da açın
3. NuGet paketlerini geri yükleyin
4. Çözümü derleyin (Ctrl+Shift+B)

### Hızlı Derleme
```bash
# Otomatik derleme için
./build-release.bat
```

### Bağımlılıklar
- **ModernWpf**: Modern UI tasarımı
- **Hardcodet.NotifyIcon.Wpf**: Sistem tepsisi işlevselliği

## 🔧 Teknik Detaylar

- **Framework**: .NET 6.0 WPF
- **UI Framework**: ModernWPF (Windows 11 tarzı tasarım)
- **Mimari**: MVVM dostu tek pencere uygulaması
- **Threading**: Async/await ile engellemeyen işlemler
- **Deployment**: Self-contained, tek dosya çalıştırılabilir

## 📄 Lisans

Bu proje MIT Lisansı altında lisanslanmıştır - detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 🤝 Katkı

Katkılarınızı bekliyoruz! Lütfen Pull Request göndermekte çekinmeyin.

## 💬 Destek

Herhangi bir sorun yaşarsanız veya özellik talebiniz varsa, [Issues](../../issues) sayfasında konu açabilirsiniz.