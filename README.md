# 1. Thông Tin Nhóm

**Tên Dự Án:**  Auralis: Return Of The Souls

**Link Dự Án:** https://github.com/Sagitoaz/ProPixelGameJam

**Thành Viên Nhóm:**
- Nguyễn Thành Trung
- Nguyễn Mạnh Dũng
- Lê Đình Chuyên
- Mentor:  Phan Thanh Tân, Nguyễn Nhật Thành

## Mô hình làm việc

Team hoạt động theo mô hình Scrum, sử dụng Linear để quản lý công việc. Các công việc được keep track đầy đủ trên Linear.
- Link linear: https://linear.app/btck-java/project/progamejam-5272f74cac32/overview

Mỗi tuần, team sẽ ngồi lại để review công việc đã làm, cùng nhau giải quyết vấn đề và đề xuất giải pháp cho tuần tiếp theo. Sau đó sẽ có buổi demo cho mentor để nhận phản hồi và hướng dẫn.

## Version Control Strategy
Team hoạt động theo Gitflow để quản lý code. Mỗi thành viên sẽ tạo branch từ `develop` để làm việc, các branch đặt theo format `feature/ten-chuc-nang`, sau khi hoàn thành sẽ tạo Pull Request để review code và merge vào develop
- Các nhánh chính:
  - `master`: Chứa code ổn định, đã qua kiểm tra và test kỹ lưỡng
  - `develop`: Chứa code mới nhất, đã qua review và test
  - `feature/`: Các nhánh chứa code đang phát triển, short-live, sau khi hoàn thành sẽ merge vào `develop`. 

![image](https://github.com/user-attachments/assets/39ee206a-de98-4e32-8796-bf0e40b6cd4f)

Sau mỗi tuần, team sẽ merge `develop` vào `master` để release phiên bản mới.


# 2. Giới Thiệu Dự Án

**Tên Game:** Auralis: Return Of The Souls

**Thể loại:** 2D Platformer, Action, Hack & Slash

**Lối chơi:** Di chuyển xuyên qua các map khác nhau, khám phá cốt truyện, mở khóa kỹ năng, mua đồ,....

**Tóm tắt cốt truyện:** 
Tại một nơi xa xăm trong vũ trụ bao la, hành tinh Lumora rực rỡ ánh sáng, nơi rừng ngọc phát quang hòa quyện cùng những dòng suối hát vang khúc ca vĩnh cửu. Lumora được chở che bởi Auralis, vị thần bảo hộ với trái tim chan chứa tình yêu dành cho mọi sinh vật. Một ngày nọ, hành tinh Lumora bị Daros – hiện thân hỗn mang – tàn phá, xé tan linh hồn vị thần Auralis thành bốn mảnh: Sợ Hãi, Nỗi Buồn, Giận Dữ, Hy Vọng. Thời gian trôi qua, tái sinh dưới hình hài của một chàng trai loài người không ký ức, Auralis tỉnh dậy giữa Lumora hoang tàn, đầy thống khổ. Nghe theo một giọng nói bí ẩn vang lên trong đầu, ngài bắt đầu hành trình thu thập mảnh vỡ linh hồn, đối mặt bóng tối trong tâm hồn khi ba bản thể tiêu cực hợp thành Ferosor. Vượt qua Ferosor và hợp nhất với bản thể Hy Vọng - chủ nhân của giọng nói bí ẩn, Auralis đánh bại Daros, quyết định số phận của hắn: tha thứ để tái tạo hòa bình hoặc trả thù, để lại một thiên đường lạnh lẽo.

**Gameplay:**
Người chơi điều khiển Auralis qua nhiều thế giới, thu thập coin, vật phẩm và nâng cấp kỹ năng như nhảy đôi, dash. Gameplay tập trung vào di chuyển linh hoạt, chiến đấu với kẻ thù, giải đố và đánh bại boss. Hệ thống inventory cho phép sử dụng vật phẩm như Health Potion, Mana Potion, hỗ trợ hành trình khám phá cốt truyện và quyết định số phận Lumora qua các lựa chọn then chốt.

# 3. Các Chức Năng Chính
- Cơ chế điều khiển nhân vật: di chuyển, nhảy, lướt, nhận sát thương, tấn công, chết, tương tác vật phẩm.
- Hệ thống kỹ năng nhân vật: nhảy hai lần, bơi, lướt.
- Hệ thống combat: đánh thường gồm combo 2 lần, đánh đặc biệt.
- Hệ thống quái vật, boss: cơ chế điều khiển cơ bản (di chuyển, tấn công, nhận sát thương, chết), boss có các kĩ năng đặc biệt như tàng hình, dịch chuyển, vòng bảo hộ gây sát thương.
- Hệ thống map: map có chủ đề đa dạng, hệ thống mini map, dịch chuyển giữa các checkpoint.
- Hệ thống vật phẩm: tiền tệ, vật phẩm chức năng.
- Hệ thống cửa hàng: mua vật phẩm, refill vật phẩm, mô tả vật phẩm.

# 4. Công nghệ

## 4.1. Công Nghệ Sử Dụng
- Engine Unity 6

## 4.2 Cấu trúc dự án

```
ProGameJam/
├── Assets/
│   ├── Audio/
│   │   └── (các file âm thanh: nhạc nền, hiệu ứng âm thanh)
│   ├── Font/
│   │   └── (các font chữ)
│   ├── Image/
│   │   └── (các hình ảnh thô hoặc texture)
│   ├── Materials/
│   │   ├── EnemyPrefabs/
│   │   │   └── (các Prefab của kẻ thù)
│   │   ├── InventoryPrefabs/
│   │   │   └──  (các Prefab của hệ thống Inventory)
│   │   ├── NPCPrefabs/
│   │   │   └── (các Prefab của NPC)
│   │   ├── PlayerPrefabs/
│   │   │   └── (các Prefab liên quan tới player)
│   │   ├── ShopSystemPrefabs/
│   │   │   └── (các Prefab liên quan tới hệ thống cửa hàng)
│   │   ├── SaveGamePrefabs/
│   │   │   └── (các Prefab liên quan tới hệ thống lưu game)
│   │   ├── UIPrefabs/
│   │   │   └── (các Prefab giao diện: inventory UI, shop UI)
│   │   └── (các material khác)
│   ├── RenderTexture/
│   │   └── (các render texture cho hiệu ứng đồ họa)
│   ├── Scenes/
│   │   ├── Game/
│   │   ├── MainMenu/
│   ├── Scripts/
│   │   ├── Audio/
│   │   │   └── (script cài đặt âm thanh game)
│   │   ├── DataPersistence/
│   │   │   └── (script cài đặt hệ thống lưu game)
│   │   ├── Enemy/
│   │   │   └── (script liên quan tới hệ thống quái và boss)
│   │   ├── GameManager/
│   │   │   └── (script quản lý các thành phần trong game)
│   │   ├── Items/
│   │   │   └── (script quản lý hệ thống vật phẩm)
│   │   ├── Map/
│   │   │   └── (script quản lý hệ thống bản đồ)
│   │   ├── MenuUI/
│   │   │   └── (script điều chỉnh Menu UI)
│   │   ├── NPC/
│   │   │   └── (script quản lý hệ thống NPC)
│   │   ├── Player/
│   │   │   └── (các script cho nhân vật)
│   │   ├── Shop/
│   │   │   └── (các script liên quan đến shop)
│   │   └── Video/
│   │       └── (các script liên quan đến video cắt cảnh)
│   ├── Settings/
│   │   └── (các tệp thiết lập tùy chỉnh)
│   ├── Sound/
│   │   └── (các tệp âm thanh bổ sung nếu có)
│   ├── Sprites/
│   │   ├── Character/
│   │   │   └── (các sprite của Auralis, kẻ thù)
│   │   ├── Effect/
│   │   │   └── (các sprite hiệu ứng: tấn công, hồi máu)
│   │   ├── Environment/
│   │   │   └── (các sprite môi trường: cây cối, nền đất)
│   │   ├── Inventory/
│   │   │   └── (các sprite giao diện inventory)
│   │   ├── Item/
│   │   │   └── (các sprite vật phẩm: HealthPotion.png, ManaPotion.png)
│   │   └── (các sprite khác)
│   ├── TextMeshPro/
│   │   └── (font, sprite asset của TextMeshPro)
│   ├── TileMap/
│   │   └── (các tilemap cho môi trường game)
│   ├── UX_UI/
│   │   └── (các tài nguyên giao diện: nút, thanh máu)
│   └── Video/
│       └── (các file video cắt cảnh)
├── Packages/
│   └── (các package Unity: TextMeshPro, 2D Sprite, v.v.)
└── (các thư mục/tệp khác: Library/, ProjectSettings/, Logs/)
...
```

# 5. Ảnh và Video Demo

**Ảnh Demo:**
https://drive.google.com/drive/folders/1iTtKBR8d_gjoQaxAhEz-ccvnMhbevjC1?usp=sharing

**Video Demo:**
https://youtu.be/JKnJl8QNuZE

# 6. Các Vấn Đề Gặp Phải

## Vấn Đề 1: Gặp conflict liên quan tới folder UserSetting khi merge lên Github

Do file UserSetting lưu lại các thành phần trên màn hình làm việc của người dùng nên các thay đổi về bố cục của Workspace cũng sẽ gây ra sự thay đổi của file này.

**Giải pháp:** 
- Xóa file GameUser.Unity trong nhánh develop, main và cho vào file .gitignore để không gặp lỗi khi merge.

**Kết Quả:**
- Sau khi thực hiện giải pháp, tình trạng bị conflict do folder UserSetting không còn xuất hiện khi merge các nhánh vào nhánh develop.

## Vấn Đề 2: Conflict khi merge git lfs => Mất map

Vì game được thiết kế theo kiểu Metroidvania nên toàn bộ nội dung được đặt trong một scene duy nhất. Điều này có thể gây ra xung đột khi merge giữa các nhánh, dẫn đến việc mất GameObject do khác biệt giữa hai phiên bản.

**Giải pháp:** 
- Biến nó thành prefab material để ko bị mất và pack nó thành lfs.

**Kết Quả:**
- Sau khi thực hiện giải pháp,  map ko bị mất khi merge từ các nhánh vào.
  
# 7. Kết Luận

**Kết quả đạt được:** 

Sau quá trình hoàn thiện và kiểm tra, tất cả các lỗi và sự cố kỹ thuật trong game đã được xử lý hoàn toàn. Nhóm phát triển đã tiến hành rà soát, điều chỉnh và tối ưu code nhằm đảm bảo hiệu năng và độ ổn định của trò chơi. . Hiện tại, game đã hoạt động ổn định và sẵn sàng để triển khai đến người dùng.


**Hướng phát triển tiếp theo:**

- Dự án được thực hiện trong thời gian gấp rút và yêu cầu vừa nghiên cứu vừa thực hành trên chính sản phẩm nên có thể sẽ còn nhiều lỗi chưa được phát hiện kịp thời. Nhóm sẽ tiến hành kiểm tra và khắc phục triệt để các lỗi còn tồn tại trong game.
- Nhóm dự định sẽ mở rộng hơn về hệ thống vật phẩm, cửa hàng, đồng thời sẽ cải thiện phần đồ họa để người chơi có trải nghiệm tốt nhất.
