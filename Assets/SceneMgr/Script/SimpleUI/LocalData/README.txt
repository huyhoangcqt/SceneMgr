Local data:
    1. Setting Data.
        - Config cứng: (bundle link, resource link,... other setting needed for your app running)
        => Lưu trữ dưới dạng file config trong máy / mã hóa để tăng thêm tính bảo mật.
    2. User temp data.
        - Chẳng hạn như thông tin lưu account/password trên device.
        - Setting trên device.
        => Những thông tin có thể clear khi user remove app hoặc chuyển qua device khác.
        => Có thể lưu trữ trên PlayerPref
    3. Account Database. => //SQLite Database
        Data liên quan đến account của user:
        - Túi đồ, vật phẩm.
        - Thông tin sự kiện, tích lũy, điểm.
        - Thông tin nhân vật,...

Mục 3 có thể chuyển sang dùng cloud như firebase / chuyển qua xử lý ở server.
Nếu như game simple => có thể tạo một hệ thống serialization để lưu file dưới local hoặc thậm chí có thể chuyển tất cả 
data serialization về thành chuỗi, lưu trữ lại trong PlayerPref.

Project hiện tại chỉ muốn target vào lưu trữ AssetDatabase và AssetBundle, hotpdate nên là sẽ tạo một simple local database system thôi.
