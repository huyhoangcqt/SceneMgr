TARGET:
1. Tạo Window Editor để tạo config và quản lý  DebuggerConfig. (Tag, màu sắc, show or not?)
Đạt được các tiêu chí sau:
(1) Có thể tạo,thay đổi chỉnh sửa DebuggerConfig: (Tag, màu sắc, show or not?)
(2) Có nút Apply. Khi ấn nút này, sẽ tạo: ra 1 file Config / lưu data vào một đường dẫn cố định. + 1 file LogTags tự động generate các Enum.

2. Debugger tự động tải config này từ đường dẫn cố định khi được gọi Init.
Thay vì phải tạo một monobehaviour và gán Config này vào.

3. Tự động generate được các mục LogTags thay vì phải setup tay như mục 1.

4. Tạo được view để xem các comment đã log ra. Có thể tùy chỉnh / filter các comments này theo LogTags.

HIỆN TẠI:
1. Đã có thể log các nội dùng kèm theo tag, màu sắc, có setup show hoặc không?

Những điểm bất cập:
1. Phải tự tạo enum trong LogTags.
2. Phải copy tên này vào DebuggerConfig.
3. Phải tạo 1 gameObject và gắn component DebugerMgr.
Và phải assign DebugerConfig vào DebugerMgr.
