using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    // Trả về -1 nếu chuỗi hợp lệ, ngược lại trả về vị trí lỗi đầu tiên
    static int ValidateBrackets(string input)
    {
        // Lưu ngoặc mở cùng vị trí xuất hiện để xác định chính xác vị trí lỗi
        var bracketStack = new Stack<(char bracket, int index)>();

        // Duyệt chuỗi từ trái sang phải.
        for (int index = 0; index < input.Length; index++)
        {
            char character = input[index];

            // Gặp ngoặc mở thì đưa vào Stack để chờ ghép cặp
            if (character == '(' || character == '[' || character == '{')
            {
                bracketStack.Push((character, index));
            }
            // Gặp ngoặc đóng thì kiểm tra với ngoặc mở gần nhất
            else if (character == ')' || character == ']' || character == '}')
            {
                // Stack rỗng nghĩa là không có ngoặc mở tương ứng
                if (bracketStack.Count == 0)
                    return index;

                // Lấy ngoặc mở ở đỉnh Stack để kiểm tra
                var topBracket = bracketStack.Pop();

                // Hai ngoặc không cùng loại thì vị trí hiện tại là vị trí lỗi
                if (!IsMatchingPair(topBracket.bracket, character))
                    return index;
            }
        }

        // Nếu còn ngoặc mở chưa được đóng, tìm vị trí xuất hiện sớm nhất
        if (bracketStack.Count > 0)
        {
            int earliestErrorIndex = int.MaxValue;

            foreach (var bracket in bracketStack)
            {
                if (bracket.index < earliestErrorIndex)
                    earliestErrorIndex = bracket.index;
            }

            return earliestErrorIndex;
        }

        // Không còn ngoặc mở nào chưa được đóng nên chuỗi hợp lệ
        return -1;
    }

    // Kiểm tra hai ký tự có phải là một cặp ngoặc đúng loại không
    static bool IsMatchingPair(char openingBracket, char closingBracket)
    {
        return (openingBracket == '(' && closingBracket == ')')
            || (openingBracket == '[' && closingBracket == ']')
            || (openingBracket == '{' && closingBracket == '}');
    }

    static void Main()
    {
        // Nhập chuỗi cần kiểm tra. Nhấn Enter ngay nếu muốn thử chuỗi rỗng
        Console.Write("Nhập chuỗi cần kiểm tra: ");
        // Test case 9: Đổi dòng dưới thành string input = new string('(', 500000) + new string(')', 500000);
        // Test case 9: Đổi dòng dưới thành string input = new string('(', 1000000);
        string input = Console.ReadLine() ?? "";

        // Đo thời gian thực hiện thuật toán kiểm tra
        var stopwatch = Stopwatch.StartNew();
        int errorIndex = ValidateBrackets(input);
        stopwatch.Stop();

        // In kết quả và vị trí lỗi nếu chuỗi không hợp lệ
        Console.WriteLine(errorIndex == -1 ? "HỢP LỆ" : $"SAI tại vị trí {errorIndex}");
        Console.WriteLine($"Thời gian: {stopwatch.Elapsed.TotalMilliseconds:F3} ms");
    }
}


