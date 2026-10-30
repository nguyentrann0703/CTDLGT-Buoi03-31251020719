
using System;
using System.Diagnostics;

public class CircularQueue<T>
{
    private T[] items;
    private int front;
    private int rear;
    private int count;

    // Số phần tử hiện có trong hàng đợi
    public int Count => count;

    // Dung lượng hiện tại của mảng
    public int Capacity => items.Length;

    public CircularQueue()
    {
        // Khởi tạo mảng với dung lượng ban đầu là 4
        items = new T[4];
        front = 0;
        rear = 0;
        count = 0;
    }

    public void Enqueue(T value)
    {
        // Mở rộng mảng nếu hàng đợi đã đầy
        if (count == items.Length)
            Resize();

        // Thêm phần tử vào cuối hàng đợi
        items[rear] = value;

        // Di chuyển rear theo cơ chế vòng tròn
        rear = (rear + 1) % items.Length;
        count++;
    }

    public T Dequeue()
    {
        // Không thể lấy phần tử khi hàng đợi rỗng
        if (count == 0)
            throw new InvalidOperationException("Hàng đợi rỗng.");

        // Lấy phần tử đầu tiên trong hàng đợi
        T value = items[front];

        // Xóa dữ liệu cũ tại vị trí vừa lấy
        items[front] = default!;

        // Di chuyển front đến vị trí tiếp theo
        front = (front + 1) % items.Length;
        count--;

        return value;
    }

    public T Peek()
    {
        // Không có phần tử đầu tiên để xem khi hàng đợi rỗng
        if (count == 0)
            throw new InvalidOperationException("Hàng đợi rỗng.");

        // Xem phần tử đầu tiên mà không lấy ra khỏi hàng đợi
        return items[front];
    }

    private void Resize()
    {
        // Tăng gấp đôi dung lượng khi mảng đầy
        T[] newItems = new T[items.Length * 2];

        // Sao chép các phần tử theo đúng thứ tự FIFO
        for (int i = 0; i < count; i++)
            newItems[i] = items[(front + i) % items.Length];

        // Cập nhật mảng mới và đưa front về đầu mảng
        items = newItems;
        front = 0;

        // Rear trỏ đến vị trí trống tiếp theo
        rear = count;
    }
}

class Program
{
    // Kiểm tra ngoại lệ khi thực hiện thao tác trên hàng đợi rỗng
    static void CheckException(string name, Action operation)
    {
        try
        {
            operation();
            Console.WriteLine($"{name}: Không phát sinh lỗi");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"{name}: InvalidOperationException");
        }
    }

    static void Main()
    {
        // Test 1: Kiểm tra hàng đợi mới khởi tạo
        Console.WriteLine("TEST CASE 1");

        var emptyQueue = new CircularQueue<string>();

        Console.WriteLine(
            $"Số phần tử={emptyQueue.Count}, Dung lượng={emptyQueue.Capacity}");

        CheckException("Dequeue khi rỗng", () => emptyQueue.Dequeue());
        CheckException("Peek khi rỗng", () => emptyQueue.Peek());

        // Test 2: Thêm đủ bốn phần tử nhưng chưa cần mở rộng
        Console.WriteLine("\nTEST CASE 2");

        var queue = new CircularQueue<string>();

        queue.Enqueue("A");
        queue.Enqueue("B");
        queue.Enqueue("C");
        queue.Enqueue("D");

        Console.WriteLine(
            $"Số phần tử={queue.Count}, Dung lượng={queue.Capacity}, Phần tử đầu={queue.Peek()}");

        // Test 3: Lấy hai phần tử rồi thêm hai phần tử để kiểm tra quay vòng
        Console.WriteLine("\nTEST CASE 3");

        Console.WriteLine($"Lấy ra: {queue.Dequeue()}, {queue.Dequeue()}");

        queue.Enqueue("E");
        queue.Enqueue("F");

        string[] remaining = new string[4];

        for (int i = 0; i < remaining.Length; i++)
            remaining[i] = queue.Dequeue();

        Console.WriteLine(
            $"Còn lại: {string.Join(", ", remaining)}; Dung lượng={queue.Capacity}");

        // Test 4: Thêm năm phần tử để kiểm tra mở rộng từ 4 lên 8
        Console.WriteLine("\nTEST CASE 4");

        var queue4 = new CircularQueue<int>();

        for (int i = 1; i <= 5; i++)
            queue4.Enqueue(i);

        int[] result4 = new int[5];

        for (int i = 0; i < result4.Length; i++)
            result4[i] = queue4.Dequeue();

        Console.WriteLine($"Dung lượng={queue4.Capacity}");
        Console.WriteLine($"Lấy ra: {string.Join(", ", result4)}");

        // Test 5: Kiểm tra mở rộng khi các phần tử đã quay vòng
        Console.WriteLine("\nTEST CASE 5");

        var queue5 = new CircularQueue<string>();

        foreach (string value in new[] { "A", "B", "C", "D" })
            queue5.Enqueue(value);

        queue5.Dequeue();
        queue5.Dequeue();

        foreach (string value in new[] { "E", "F", "G" })
            queue5.Enqueue(value);

        string[] result5 = new string[5];

        for (int i = 0; i < result5.Length; i++)
            result5[i] = queue5.Dequeue();

        Console.WriteLine($"Dung lượng={queue5.Capacity}");
        Console.WriteLine($"Lấy ra: {string.Join(", ", result5)}");

        // Test 6: Lặp Enqueue và Dequeue một triệu lần để kiểm tra quay vòng
        Console.WriteLine("\nTEST CASE 6");

        var queue6 = new CircularQueue<int>();

        for (int i = 0; i < 1_000_000; i++)
        {
            queue6.Enqueue(i);
            queue6.Dequeue();
        }

        Console.WriteLine(
            $"Số phần tử={queue6.Count}, Dung lượng={queue6.Capacity}");

        // Test 7: Thêm và lấy 100.000 phần tử để kiểm tra thứ tự FIFO
        Console.WriteLine("\nTEST CASE 7");

        var queue7 = new CircularQueue<int>();

        var stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < 100_000; i++)
            queue7.Enqueue(i);

        bool correctOrder = true;

        for (int i = 0; i < 100_000; i++)
        {
            if (queue7.Dequeue() != i)
                correctOrder = false;
        }

        stopwatch.Stop();

        Console.WriteLine($"Đúng thứ tự={correctOrder}");
        Console.WriteLine($"Số phần tử={queue7.Count}");
        Console.WriteLine($"Dung lượng cuối={queue7.Capacity}");
        Console.WriteLine(
            $"Thời gian chạy: {stopwatch.Elapsed.TotalMilliseconds:F3} ms");
    }
}
