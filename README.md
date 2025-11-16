📘 Word Quiz App – ASP.NET Core

Belirli formatta hazırlanmış .txt dosyalarından İngilizce kelimeleri, Türkçe anlamlarını ve seviyelerini okuyup bir veritabanına kaydeden; sonrasında ise bu kelimelerle quiz yapan bir ASP.NET Core uygulaması.

Özellikler
🔹 Kelime Yükleme

Kullanıcı, belirli formatta hazırlanmış .txt dosyasını yükler.

Her satırda:

kelime | anlam1, anlam2, anlam3 | seviye (A1–C2)


Uygulama dosyadaki tüm kelimeleri parse eder ve otomatik olarak veritabanına kaydeder.

🔹 Quiz Sistemi

Uygulama, veritabanına kaydedilen kelimeleri quiz formatında kullanıcıya sorar.

Kullanıcı kelimenin Türkçe anlamını yazarak cevap verir.

Doğru/yanlış kontrolü yapılır.

🔹 Filtreleme Seçenekleri

Quiz aşağıdaki kriterlere göre hazırlanabilir:

Seviye (örn: A1 / A2 / B1 …)

Kelime türü (örn: bağlaç, sıfat, fiil vs.)

Haftalara göre ezber listeleri

Tüm kelimelerden rastgele quiz

🔹 İstatistik / Gelişim Takibi

Doğru bilinen ve yanlış yapılan kelimeler kaydedilir.

Tekrar edilmesi gereken kelimeler işaretlenebilir. 


🛠️ Kullanılan Teknolojiler

ASP.NET Core 8.0

Entity Framework Core

MSSQL 

Razor Pages / MVC

📥 TXT Dosyası Formatı Örneği
accept - kabul etmek /A1
although - rağmen, -e karşın / B1 *(Bağlaç olduğunu belirtmek için "*" konuluyor.)
run - koşmak, işletmek /A2 
