//by Vokinsark
using System;

namespace binarTestCsharp
{
    class Program
    {
        static string decToBin(string inp)
        {
            string output = "";
            ulong temp = 1;

            if (inp.Length > 20 || inp.Length >= 20 && string.Compare(inp, "18446744073709551615") > 0)
                return ""; //"Введено слишком большое число. Попробуйте еще раз.";
            for (int i = 0; i < inp.Length; i++)
                if (!char.IsDigit(inp[i]))
                    return ""; //"В строке ввода были обнаружены недопустимые символы. Попробуйте еще раз.";

            ulong input = inp == "" ? 0 : ulong.Parse(inp);
            int[] massBits = new int[input == 0 ? 1 : (int)Math.Log(input, 2) + 1];

            for (int i = 0; i < massBits.Length; i++) {
                massBits[i] = (input & temp) > 0 ? 1 : 0;
                temp = temp << 1;
            }

            for (int i = massBits.Length - 1; i >= 0; i--)
                output += massBits[i];

            return output;
        }

        static ulong strBinToDec(string inp)
        {
            ulong result = 0;

            for(int i = inp.Length - 1; i >= 0; i--)
                result += inp[i] > '0' ? (ulong)Math.Pow(2, inp.Length - i - 1) : 0;

            return result;
        }

        static string permut(string block, bool dir)
        {
            string result = "";
            int[] permutF = {2, 3, 0, 1, 5, 7, 4, 6, 2, 7, 1, 3};
            int[] permutS = {7, 6, 2, 1, 4, 3, 0, 5};
            if(dir)
                for(int i = 0; i < 12; i++)
                    result += block[permutF[i]];
            else
                for(int i = 0; i < 8; i++)
                    result += block[permutS[i]];

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n  [I] Блок {0} перестановки: Вход: {1} Выход: {2}", dir?"начальной":"конечной", block, result);
            return result;
        }

        static string subs(string block, int stage)
        {
            string result = "";
            string[] subs1 = {"100", "110", "001", "011", "101", "111", "010", "101", "101", "111", "010", "100", "110", "001", "011", "110"};
            string[] subs2 = {"011", "101", "111", "010", "100", "110", "001", "111", "100", "110", "001", "011", "101", "111", "010", "001"};
            string[] subs3 = { "01", "10", "11", "01", "10", "11", "01", "10", "11", "01", "10", "11", "01", "10", "11", "01" };
            
            switch(stage)
            {
                case 0:
                    result = subs1[strBinToDec(block)];
                    break;
                case 1:
                    result = subs2[strBinToDec(block)];
                    break;
                case 2:
                    result = subs3[strBinToDec(block)];
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    [I] Блок замены {0}: Вход: {1} Выход: {2}", stage + 1, block, result);
            return result;
        }

        static string sumMod2(string x, string y, int blockSize)
        {
            string result = "";

            for(int i = 0; i < blockSize; i++)
            {
                if ((x[i] == '0' && y[i] == '0') || (x[i] == '1' && y[i] == '1'))
                    result += '0';
                else
                    result += '1';
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n  [I] Правый блок XOR {0}: Правый блок: {1} {0}: {2} \n    Результат: {3}", blockSize == 12 ? "Ключ" : "Левый блок", x, y, result);
            return result;
        }

        static string des(string text, string key, bool dir)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n[I] Инициация {0}", dir?"шифрования":"дешифрования");
            string temp;
            string tempRight;
            if(text.Length < 16)
            {
                temp = "";
                for(int i = 0; i < 16-text.Length; i++) temp += "0";
                text = temp + text;
            }
            if(key.Length < 24)
            {
                temp = "";
                for(int i = 0; i < 24-key.Length; i++) temp += "0";
                key = temp + key;
            }
            string[] keyI = new string[3];
            string result = "";
            string left = text.Substring(0, 8);
            string right = text.Substring(8, 8);

            if(dir)
                for(int i = 0; i < 3; i++)
                    keyI[i] = key.Substring(i*6, 12);
            else
                for (int i = 0; i < 3; i++)
                    keyI[i] = key.Substring((2-i)*6, 12);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[I] Входящий блок (16бит): {0}", text);
            Console.WriteLine("\n[I] Начальный ключ (24бит): {0}", key);
            Console.WriteLine("\n[I] Ключи: Ключ 1: {0} Ключ 2: {1} Ключ 3: {2}", keyI[0], keyI[1], keyI[2]);
            for(int i = 0; i < 3; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n[I] {0} круг {1}: Левый блок: {2} Правый блок: {3}", dir ? "Шифрование" : "Дешифрование", i + 1, left, right);
                temp = "";
                tempRight = right;

                //F start
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n    ---- Начало функции F");
                right = permut(right, true);
                right = sumMod2(right, keyI[i], 12);
                for(int j = 0; j < 3; j++)
                    temp += subs(right.Substring(j*4, 4), j);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n    [I] Результат блоков замен: {0}", temp);
                right = temp;
                right = permut(right, false);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n    ---- Конец функции F");
                //F finish

                right = sumMod2(right, left, 8);
                left = tempRight;
            }
            result = strBinToDec(right + left).ToString();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n[I] Результат {0}: {1}", dir ? "шифрования" : "дешифрования", right+left);
            Console.ForegroundColor = ConsoleColor.Cyan;
            return result;
        }

        static void Main(string[] args)
        {
            Console.Title = "Учебный DES";
            string input;
            string key;
            string result;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Введите число: ");
                input = Console.ReadLine();
                Console.Write("Введите ключ: ");
                key = Console.ReadLine();
                input = decToBin(input);
                key = decToBin(key);

                Console.WriteLine("\nЧисло в 2-ном виде: " + input);
                Console.WriteLine("Ключ в 2-ном виде: " + key);

                result = des(input, key, true);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nПолученное зашифрованное число: " + result);
                Console.WriteLine("\nПолученное расшифрованное число: " + des(decToBin(result), key, false));
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
