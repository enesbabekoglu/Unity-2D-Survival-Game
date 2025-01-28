# Unity 2D Survival Game

Oyunumuz bir hayatta kalma oyunudur. Başlangıç olarak karakterimizin açlığının azalması, balık tutma (panelde mini oyun şeklinde), ağaç kesme, odunlar ile ateş yakma, ateşte balık pişirme, çalıların arasına girince canının azalması ve yavaşlaması, yemek yeme arayüzü (paneli) yapılmıştır.

## Demo Bağlantısı
https://test.enesbabekoglu.com.tr

## Proje Üyeleri / Görev Dağılımları
### 1. Ahmet Talha Geçgelen (22360859024) - [@gecgelenus](https://github.com/gecgelenus)

  ### VİZE
  - Çalıda iken yavaşlama damageOnCollision.cs:44 --  :55
  - Çalıda iken hasar alma damageOnCollision.cs:36
  - Ateş alanına yakınlık tespiti FireInteraction.cs:33
  - Oyuncu çarpışma tespiti playerController.cs:32
  - Oyuncu hareket kontrolü (+animasyon) playerController.cs:42
  - Nesnenin oyuncuyu takibi followHolder.cs:56 -- :71
  - Ağaç ile etkileşime geçme kontrolü takeDamage.cs:47 -- :64
  - Ağaç ile etkileşim takeDamage.cs:87
  - Ağacın yeniden doğması takeDamage.cs:38
  - Kameranın oyuncuyu takibi playerControls.cs:76
  - Ateşin yanma süresi kontrolü FireControl.cs:42
  - Ağaçın sağlık kontrolü/limiti takeDamage.cs:89
  
  ### FİNAL
  - Duraklama Menüsü (10x)
  - Asenkronizasyon (10x)
  - Animasyonlar (20x)
  - Ses ve Müzik (20x)
 

### 2. Enes Babekoğlu (20360859113) - [@enesbabekoglu](https://github.com/enesbabekoglu)

 ### VİZE
  - Balık pişirme BarbecueSystem.cs:37
  - Yiyecek yeme sistemi EatingSystem.cs:167
  - Balık tutma minioyunu FishingSystem.cs:55
  - Balık tutma sistemi/Su ile etkileşim FishingTriggerHandler.cs:33
  - Can ve açlık sistemi HungerHealthSystem.cs:60 -- :72 -- :85
  - Envanter sistemi InventorySystem.cs:48 -- :69 -- :92 -- :103
  - Oyun yönetim scripti GameManager.cs:50
  - Nesnelerin etkileşime geçilebilirlik kontrolü followHolder.cs:112 -- :44
  - Ateş alanına tıklayarak etkileşime geçme FireInteraction.cs:74
  - Oyuncu hızının açlığa bağlı olarak değişimi playerState.cs:51
  - Mini oyun kullanıcı girdisi FishingSystem.cs:95 -- :101
  - Balık tutma süresi kontrolü FishingSystem.cs:132
  - Can ve açlık kontrolü/limiti HungerHealthSystem.cs:66 -- :81

  ### FİNAL
  - Ana Menü (30x)
  - Duraklama Menüsü (10x)
  - Animasyonlar (10x)
  - Ses ve Müzik (10x)


## Oyun İçi Özellikler ve Görseller

<b>Ana Menü: </b> Çoklu dil desteği, ses ve müzik ayarlarını değiştirebilme gibi seçenekler bulunmaktadır.

<img width="951" alt="image" src="https://github.com/user-attachments/assets/2d78cc29-d5ce-4cff-afa3-1cd84a40dc1b" />


<b>Hayvanlar: </b> Belirlenen alanda rastgele olarak yürüyen tavuklar ve horoz. Tavuklar belirli aralıklarla rastgele olarak yumurtlama özelliğine sahiptir. Tavukların yumurtalarına temas ettiğinde envantere gelmekte ve karakterimiz yumurtaları tüketebilmektedir.

<img width="951" alt="image" src="https://github.com/user-attachments/assets/63ec50eb-63fb-40ff-a7f0-be2edb24bd15" />


<b>Hasar Alma: </b> Karakterimiz dikenli otların arasında gezinirken hasar alır.

<img width="982" alt="image" src="https://github.com/user-attachments/assets/2c374022-aac0-4cef-abbd-048bb0a279d6" />


<b>Balık Tutma: </b> Su kenarında elinde olta varken karakterimiz klavyenin sağ ve sol tuşları ile balık tutabilir.

<img width="982" alt="image" src="https://github.com/user-attachments/assets/9957056b-214e-4817-9c73-1c60a134655b" />


<b>Ateş Yakma ve Balık Tutma: </b> Su kenarında elinde olta varken karakterimiz klavyenin sağ ve sol tuşları ile balık tutabilir.

<img width="982" alt="image" src="https://github.com/user-attachments/assets/83484b64-4dbb-4cfe-9d45-076a49183bcc" />


<b>Yemek Yeme: </b> Tutulan ve pişirilen balıklar ile toplanan yumurtalar tüketilebilir.

<img width="982" alt="image" src="https://github.com/user-attachments/assets/f011ef96-bf4c-4967-9314-82db83f3350c" />


<b>Ağaç Kesme: </b> Ağaçlar kesilebilir ve odun kazanılabilir. Odunlar ile ateş yakılır. Ağaç kesmek için balta elimizde olmalıdır.

<img width="982" alt="image" src="https://github.com/user-attachments/assets/4e21a5bd-12b0-411e-a44d-c7507401bfca" />


<b>Asenkron 2. Sahne</b>

<img width="982" alt="image" src="https://github.com/user-attachments/assets/35a1d2a7-bb58-4f44-ab61-d6a52783b897" />


<b>Duraklama Menüsü</b>

<img width="982" alt="image" src="https://github.com/user-attachments/assets/f4a60043-c582-4410-8429-feefc3a7c6e3" />


## Kaynaklar

#### UI ve Oyun Objeleri
https://assetstore.unity.com/packages/essentials/tutorial-projects/happy-harvest-2d-sample-project-259218
https://www.freepik.com/free-vector/organic-flat-element-animation-frames_13818850.htm#fromView=user&page=4&position=19
https://www.freepik.com/premium-vector/cock-walk-cycle-animation-sequence-vector_19081201.htm#fromView=user&page=4&position=3
https://www.freepik.com/free-photo/close-up-boiled-egg_945744.htm
https://www.freepik.com/premium-vector/cock-walk-cycle-animation-sequence-vector_19081209.htm
https://www.freepik.com/free-vector/square-wooden-frames-game-user-avatar_32491225.htm
https://www.freepik.com/free-vector/vector-cartoon-game-icon-set-ui-interface-design_39985656.htm

#### Karakter
https://www.artstation.com/marketplace/p/rnGJ/free-2d-demo-character-sprite-sheet

#### Sesler
https://mixkit.co/free-sound-effects/walk/
https://pixabay.com/sound-effects/

