using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

class Customer
{
    // Lưu thông tin của từng khách hàng
    public int Id { get; }
    public long Arrival { get; }
    public long ServiceTime { get; }

    public Customer(int id, long arrival, long serviceTime)
    {
        Id = id;
        Arrival = arrival;
        ServiceTime = serviceTime;
    }
}

class Program
{
    
    static void Main()
    {
        // Đảm bảo số thực được hiển thị bằng dấu chấm
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        // Nhập số lượng khách hàng và số quầy phục vụ
        string[] firstLine = (Console.ReadLine() ?? "").Split(
            ' ', StringSplitOptions.RemoveEmptyEntries);

        int customerCount = int.Parse(firstLine[0]);
        int counterCount = int.Parse(firstLine[1]);

        // Queue lưu khách theo thứ tự đến quầy (FIFO)
        var customerQueue = new Queue<Customer>();

        // Đọc thông tin từng khách hàng và đưa vào hàng đợi
        // for (int i = 1; i <= customerCount; i++)
        // {
        //     string[] parts = (Console.ReadLine() ?? "").Split(
        //         ' ', StringSplitOptions.RemoveEmptyEntries);

        //     long arrival = long.Parse(parts[0]);
        //     long serviceTime = long.Parse(parts[1]);

        //     customerQueue.Enqueue(new Customer(i, arrival, serviceTime));
        // }
        // Thay vòng lặp trên bằng vòng lặp dưới cho test 6 (Tạo tự động 100000 khách hàng)
        for (int i = 0; i < customerCount; i++)
        {
            customerQueue.Enqueue(new Customer(i + 1, i, 10));
        }

        // Lưu thời điểm mỗi quầy hoàn thành công việc
        long[] counterFreeAt = new long[counterCount];

        long totalWaitingTime = 0;
        long maxWaitingTime = 0;
        long lastCompletionTime = 0;

        // Lưu thời gian chờ của từng khách để in kết quả
        List<long> waitingTimes = new List<long>();

        // Xử lý từng khách theo thứ tự trong hàng đợi
        var stopwatch = Stopwatch.StartNew();
        while (customerQueue.Count > 0)
        {
            Customer customer = customerQueue.Dequeue();

            // Tìm quầy rảnh sớm nhất, nếu hòa thì chọn quầy có số nhỏ hơn
            int counterIndex = 0;

            for (int j = 1; j < counterCount; j++)
            {
                if (counterFreeAt[j] < counterFreeAt[counterIndex])
                    counterIndex = j;
            }

            // Xác định thời điểm bắt đầu phục vụ và thời gian khách phải chờ
            long startTime = Math.Max(
                customer.Arrival, counterFreeAt[counterIndex]);

            long waitingTime = startTime - customer.Arrival;
            long finishTime = startTime + customer.ServiceTime;

            // Cập nhật thời gian hoạt động của quầy và các thống kê
            counterFreeAt[counterIndex] = finishTime;
            totalWaitingTime += waitingTime;
            waitingTimes.Add(waitingTime);
            maxWaitingTime = Math.Max(maxWaitingTime, waitingTime);
            lastCompletionTime = Math.Max(lastCompletionTime, finishTime);
            
        }
        stopwatch.Stop();
        Console.WriteLine($"Thời gian chạy: {stopwatch.Elapsed.TotalMilliseconds:F3} ms");

        // Tính thời gian chờ trung bình của tất cả khách hàng
        double averageWaitingTime = customerCount == 0? 0: (double)totalWaitingTime / customerCount;

        // In kết quả theo định dạng của đề bài
        if (customerCount <= 100)
            Console.WriteLine($"Chờ: {string.Join(", ", waitingTimes)}");   
        Console.WriteLine($"TB {averageWaitingTime:F2}; lâu nhất {maxWaitingTime}; xong lúc {lastCompletionTime}");
    }

}
