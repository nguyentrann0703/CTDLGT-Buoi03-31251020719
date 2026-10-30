# BÀI TẬP THỰC HÀNH 03 – CẤU TRÚC DỮ LIỆU VÀ GIẢI THUẬT

**Sinh viên:** Trần Tất Nguyên  
**MSSV:** 31251020719  
**Học phần:** Cấu trúc dữ liệu và Giải thuật  
**Giảng viên:** TS. Nguyễn Thành Huy  
**Ngôn ngữ:** C#
**Lớp học phần:** 26C1INF50900706

## 1. Cấu trúc repository

Repository được tổ chức theo yêu cầu nộp bài của Buổi 03:

```text
CTDLGT-Buoi03-31251020719/
├── Bai1/
├── Bai2/
├── Bai3/
├── README.md
└── KetQuaThucNghiem.md
```

- `Bai1/`: Kiểm tra tính hợp lệ và thứ tự của các cặp ngoặc.
- `Bai2/`: Mô phỏng khách hàng và các quầy phục vụ bằng Queue.
- `Bai3/`: Tự cài đặt hàng đợi vòng `CircularQueue<T>`.
- `KetQuaThucNghiem.md`: Tổng hợp ca kiểm thử, kết quả và thời gian chạy ở các ca lớn.

Bài 4 là phần thử thách nên không được đưa vào phạm vi bài nộp này.

## 2. Yêu cầu môi trường

- Cài đặt .NET SDK (khuyến nghị .NET 6 trở lên).
- Có thể sử dụng Terminal trên macOS/Linux hoặc Command Prompt/PowerShell trên Windows.
- Mỗi thư mục bài cần có project Console C# tương ứng (`.csproj`).

Kiểm tra .NET SDK:

```bash
dotnet --version
```

## 3. Hướng dẫn biên dịch và chạy chương trình

Mở Terminal tại thư mục repository, sau đó chuyển vào thư mục bài cần chạy.

### Bài 1

```bash
cd Bai1
dotnet run
```

Nhập một chuỗi ký tự khi chương trình yêu cầu. Chương trình bỏ qua các ký tự không phải ngoặc và kiểm tra ba loại ngoặc `()`, `[]`, `{}`. Nếu chuỗi không hợp lệ, chương trình thông báo vị trí lỗi; nếu hợp lệ, chương trình in `HỢP LỆ`.

### Bài 2

```bash
cd Bai2
dotnet run
```

Nhập dòng đầu gồm số khách hàng `n` và số quầy `k`, cách nhau bởi dấu cách. Sau đó nhập lần lượt `n` dòng, mỗi dòng gồm thời điểm đến `a` và thời gian phục vụ `s`.

Ví dụ:

```text
3 1
0 5
1 3
2 4
```

Chương trình mô phỏng việc phục vụ khách theo thứ tự FIFO, chọn quầy rảnh sớm nhất và in thời gian chờ của từng khách cùng các thống kê tổng hợp.

Với ca kiểm thử lớn theo đề (`n = 100000`, `k = 10`), chương trình tạo dữ liệu tự động theo quy luật khách thứ `i` đến tại thời điểm `i - 1` và có thời gian phục vụ bằng `10`; không cần nhập thêm 100.000 dòng dữ liệu.

### Bài 3

```bash
cd Bai3
dotnet run
```

Bài 3 không yêu cầu nhập dữ liệu từ bàn phím. Chương trình tự chạy các ca kiểm thử được viết trong `Main()`, bao gồm kiểm tra hàng đợi rỗng, thêm/xem/lấy phần tử, quay vòng chỉ số, mở rộng mảng và kiểm tra dữ liệu lớn.

Sau khi chạy xong, có thể quay về thư mục gốc bằng:

```bash
cd ..
```

## 4. Mô tả giải pháp và độ phức tạp

### Bài 1 – Kiểm tra cặp ngoặc bằng Stack

**Giải pháp:** Sử dụng `Stack<(char bracket, int index)>` để lưu ký tự ngoặc mở cùng vị trí xuất hiện. Khi gặp ngoặc đóng, kiểm tra Stack có rỗng hay không, sau đó lấy ngoặc mở gần nhất ra để so khớp. Nếu hết chuỗi mà Stack vẫn còn phần tử, chương trình xác định vị trí ngoặc mở chưa được đóng xuất hiện sớm nhất.

**Độ phức tạp:**
- Thời gian: `Θ(n)`, với `n` là độ dài chuỗi. Mỗi ký tự được duyệt tối đa một lần; thao tác Push/Pop có chi phí `O(1)` khấu hao.
- Bộ nhớ phụ: `O(n)` trong trường hợp xấu nhất khi chuỗi chứa toàn ngoặc mở.

### Bài 2 – Mô phỏng quầy dịch vụ bằng Queue

**Giải pháp:** Lưu khách hàng trong `Queue<Customer>` để bảo đảm thứ tự phục vụ FIFO. Với mỗi khách, chương trình duyệt mảng `counterFreeAt` để tìm quầy rảnh sớm nhất; nếu nhiều quầy cùng rảnh thì giữ quầy có số thứ tự nhỏ nhất. Thời điểm bắt đầu phục vụ là giá trị lớn hơn giữa thời điểm khách đến và thời điểm quầy rảnh.

**Độ phức tạp:**
- Thời gian: `Θ(n·k)`, vì mỗi khách được xử lý một lần và mỗi lượt kiểm tra tối đa `k` quầy.
- Bộ nhớ: `Θ(n + k)`, gồm hàng đợi khách hàng, danh sách thời gian chờ và mảng thời điểm rảnh của các quầy.

### Bài 3 – Tự cài đặt CircularQueue<T>

**Giải pháp:** Cài đặt hàng đợi vòng bằng mảng với ba biến `front`, `rear` và `count`. Chỉ số được quay vòng bằng phép chia lấy dư. Khi mảng đầy, `Resize()` tạo mảng mới có dung lượng gấp đôi và sao chép phần tử theo thứ tự logic bắt đầu từ `front`, sau đó đặt lại `front = 0` và `rear = count`.

**Độ phức tạp:**
- `Enqueue()`: `O(1)` thông thường, `O(n)` khi mở rộng; độ phức tạp khấu hao `O(1)`.
- `Dequeue()`: `O(1)`.
- `Peek()`: `O(1)`.
- `Resize()`: `O(n)`.
- Bộ nhớ: `O(n)` theo dung lượng mảng hiện tại.

## 5. Ca kiểm thử và kết quả thực nghiệm

Các ca kiểm thử chi tiết, kết quả mong đợi, kết quả thực tế và ảnh chụp Terminal được trình bày trong báo cáo Word nộp trên LMS. File `KetQuaThucNghiem.md` dùng để tổng hợp các lần chạy và thời gian đo được.

Một số ca dữ liệu lớn cần ghi nhận thời gian chạy:

- **Bài 1:** Chuỗi gồm 500.000 ngoặc mở và 500.000 ngoặc đóng; chuỗi gồm 1.000.000 ngoặc mở.
- **Bài 2:** 100.000 khách hàng và 10 quầy phục vụ.
- **Bài 3:** Một triệu lượt `Enqueue()`/`Dequeue()` liên tiếp và ca thêm/lấy 100.000 phần tử.

Thời gian chạy phụ thuộc thiết bị, môi trường .NET và trạng thái thực thi. Vì vậy, chỉ ghi số đo từ lần chạy thực tế trên máy, không xem thời gian trong README là kết quả cố định.


## 6. Khai báo sử dụng AI

Tôi đã sử dụng ChatGPT để hỗ trợ định dạng và trình bày báo cáo, xây dựng bảng kiểm thử, đồng thời tham khảo giải thích thuật toán và cách kiểm thử. Tôi tự kiểm tra, đối chiếu mã nguồn và chịu trách nhiệm về nội dung bài nộp.# CTDLGT-Buoi03-31251020719
