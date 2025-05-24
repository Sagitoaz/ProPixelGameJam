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


**Mô tả:**


# 3. Các Chức Năng Chính
- [Chức năng 1]
- [Chức năng 2]
- [Chức năng 3]
- [Chức năng 4]

# 4. Công nghệ

## 4.1. Công Nghệ Sử Dụng
- [Công nghệ 1]
- [Công nghệ 2]
- [Công nghệ 3]
- [Công nghệ 4]

VD:
- Java 8
- LibGDX https://libgdx.com/
- Gradle
- ...

## 4.2 Cấu trúc dự án

```
- assets 
  - images
  - sounds
- core
  - common
  - config
  - view
  - model
  - controller
- desktop
- ios
...
```

Diễn giải:
- **assets:** Chứa các tài nguyên như hình ảnh, âm thanh
- **core:** Chứa các class chính của game như model, view, controller
- **desktop, ios, android:** Chứa các class để chạy trên các nền tảng khác nhau"
- ...





# 5. Ảnh và Video Demo

**Ảnh Demo:**
![Ảnh Demo](#)

**Video Demo:**
[Video Link](#)






# 6. Các Vấn Đề Gặp Phải

## Vấn Đề 1: [Mô tả vấn đề]
**Ví dụ:** Game gặp phải vấn đề hiệu năng kém, fps thấp dù không có nhiều đối tượng trên màn hình

## Hành Động Để Giải Quyết

**Giải pháp:** Do việc tạo object quá nhiều, nên dẫn tới tràn ram và giảm hiệu năng
- Sử dụng Design Pattern Object Pool để tái sử dụng object. Khi object không còn sử dụng, sẽ được đưa vào pool để sử dụng lại. 

## Kết Quả

- Sau khi sử dụng Object Pool, hiệu năng game đã được cải thiện, fps tăng lên đáng kể. Từ *30fps lên 60fps* (Rõ ràng hơn với số liệu cụ thể)

## Vấn Đề 2: [Mô tả vấn đề]
**Ví dụ:** Có quá nhiều class quái khác nhau, dù chúng có nhiều điểm chung


## Hành Động Để Giải Quyết

**Giải pháp:** Sử dụng Design Pattern Builder để tạo các object quái với các thuộc tính khác nhau mà không cần tạo nhiều class. Ngoài ra sử dụng Strategy Pattern để tạo các hành vi khác nhau cho các object quái mà không cần tạo nhiều class.

## Kết Quả

- Sau khi sử dụng Builder và Strategy Pattern, việc tạo các object quái đã trở nên dễ dàng hơn, không cần tạo nhiều class. Có thể chỉ cần config các thuộc tính và hành vi cho object quái mà không cần tạo nhiều class.

# 7. Kết Luận

**Kết quả đạt được:** [Mô tả kết quả đạt được sau khi giải quyết các vấn đề]

**Hướng phát triển tiếp theo:** [Mô tả hướng phát triển tiếp theo của dự án]
