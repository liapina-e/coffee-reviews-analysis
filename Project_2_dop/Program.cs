/*
 * Ляпина Екатерина Максимовна
 * БПИ 2410
 * вариант 15
 * 22.11.2024
 */
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp.RuntimeBinder;


namespace Project_2_dop;

class Program
{
    /// <summary>
    /// В методе Menu представлено два варианта меню в зависимости от условий задачи.
    /// </summary>
    /// <param name="path"></param>
    static void Menu(string path)
    {
        Console.WriteLine("Введите номер пункта меню для запуска действия:");
        if (path == "") // Если нет ссылки на файл
        {
            Console.WriteLine("1. Ввести адрес файла");
        }
        else // Если есть поменят на пункт смены данных
        {
            Console.WriteLine("1. Сменить адрес файла");
        }
        Console.WriteLine("2. Загрузить данные из файла");
        Console.WriteLine("3. Вывести на экран информацию об отзывах с самым высоким рейтингом за 2020 и 2021 года");
        Console.WriteLine("4. Сохранить в файл выборку об отзывах с выбранным рейтингом");
        Console.WriteLine("5. Вывести на экран сводную статистику по данным загруженного файла");
        Console.WriteLine("6. Вывести выборку записей по отзывам, полученным в одном месте");
        Console.WriteLine("7. Вывести переупорядоченный по рейтингу и дате набор данных");
        Console.WriteLine("8. Завершить работу программы");
    }
    /// <summary>
    /// В методе Main обрабатываем все данные, введенные пользователем.
    /// Запускаем все методы, представленные в других классах.
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="FormatException"></exception>
    static void Main(string[] args)
    {
        // Вводим переменную, отвечаущую за относительный путь к обрабатываемому файлу.
        string path = "";
        // Ждем, пока пользователь введет путь к файлу, иначе выдаем ошибку.
        while (path == "")
        {
            Menu(path);
            int num = int.Parse(Console.ReadLine());
            if (num != 1)
            {
                Console.WriteLine("Невозможно выполнить операцию, т.к. нет файла.");
            }
            else
            {
                Console.WriteLine("Введие путь к файлу:");
                path = Console.ReadLine();
            }
        }

        ConsoleKeyInfo keyToExit = default;
        // Программа работает, пока пользователь сам не захочет выйти, нажав клавишу Escape.
        do
        {
            // Переменная flagOfDataAvailability отвечает за выполнение первых двух пунктов меню.
            // Без них дальнейшая программа работать не будет.
            bool flagOfDataAvailability = true;
            int num = 0;
            try
            {
                Menu(path);
                num = int.Parse(Console.ReadLine());
                if (num < 1 || num > 8)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Такого пункта меню не сущетсвует.");
                Console.WriteLine("Попробуйте еще раз.");
                continue;
            }

            // Ниже представлены все возможные действия, которые может совершить пользователь.
            if (num == 1)
            {
                Console.WriteLine("Введите новый путь к файлу:");
                path = Console.ReadLine();
            }
            else if (num == 2)
            {

                Methods.ReadFile(path, out string[] arr);
                flagOfDataAvailability = true;
                Console.WriteLine("Данные из файла загружены.");
                Console.WriteLine("Нажми любую клавишу для вывода меню");
            }
            else if (num == 8)
            {
                Console.WriteLine("Для выхода нажмите Escape....");
            }
            // Если выполнены первые два пункта меню, то можно выбирать любой из следующих.

            if (flagOfDataAvailability)
            {
                Methods.CreateReviews(path, out Review[] reviews, out string columnNames);
                Methods.ReadFile(path, out string[] arr); 
                if (num == 3)
                {
                    Methods.ReviewHighestRating(reviews, out int max_20, out int max_21);
                    Methods.OutputOfHighestRating(reviews, max_20, max_21);
                    Console.WriteLine("Нажми любую клавишу для вывода меню");

                }
                else if (num == 4)
                {
                    Console.WriteLine("Введите значение рейтинга, с которым вывести отзывы:");
                    int N = int.Parse(Console.ReadLine());
                    Methods.ReviewsWithRatingN(N, reviews, arr, columnNames);
                    Console.WriteLine("Нажми любую клавишу для вывода меню");
                }
                else if (num == 5)
                {
                    Statistics statistics = new Statistics();
                    statistics.WritingCountReviewsByYears(reviews);
                    statistics.WritingPercentageOfReviews(reviews);
                    statistics.RatioOfReviews(reviews);
                    statistics.CountsReviewsWithRating(reviews, out int[] count_12, out int[] count_34);
                    statistics.WritingCountsReviewsWithRating(reviews, count_12, count_34);
                    Console.WriteLine("Нажми любую клавишу для вывода меню");
                }
                else if (num == 6)
                {
                    DopTask dop = new DopTask();
                    int number = 0;
                    Console.WriteLine("Введите:" +
                                      " 1 - если хотите вывести данные на экран" +
                                      " 2 - если хотите записать данные в файл");
                    // Проверяем, корректность данных от пользователя.
                    try
                    {
                        number = int.Parse(Console.ReadLine());
                        if (number < 1 || number > 2)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Такого пункта меню не сущетсвует.");
                        Console.WriteLine("Попробуйте еще раз.");
                        continue;
                    }

                    if (number == 1)
                    {
                        Console.WriteLine("Введите место, из которого вы хотите получить отзывы:");
                        string location_ = Console.ReadLine();
                        dop.SelectionOfReviews(reviews, location_, arr);
                        Console.WriteLine("Нажми любую клавишу для вывода меню");
                    }
                    else
                    {
                        Console.WriteLine("Введите место, из которого вы хотите получить отзывы:");
                        string location_ = Console.ReadLine();
                        Console.WriteLine("Введите путь к файлу, в который хотите сохранить данные:");
                        string path_out = Console.ReadLine();
                        dop.SavingSelection(reviews, location_, arr, path_out, columnNames);
                        Console.WriteLine("Нажми любую клавишу для вывода меню");
                    }
                    
                }
                else if (num == 7)
                {
                    DopTask dop = new DopTask();
                    int number = 0;
                    Console.WriteLine("Введите:");
                    Console.WriteLine("1 - если хотите вывести переупорядоченный набор данных на экран");
                    Console.WriteLine( "2 - если хотите записать переупорядоченный набор данных в файл");
                    // Проверяем, корректность данных от пользователя.
                    try
                    {
                        number = int.Parse(Console.ReadLine());
                        if (number < 1 || number > 2)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Такого пункта меню не сущетсвует.");
                        Console.WriteLine("Попробуйте еще раз.");
                        continue;
                    }
                    dop.CreateData(reviews, out int[][] date);
                    dop.Sort(date, reviews, arr);
                    dop.SortedDataSet(reviews, arr, out string[][] dataByRating, out string[][] dataToFile);

                    if (number == 1)
                    {
                        dop.OutputToScreen(dataByRating);
                        Console.WriteLine("Нажми любую клавишу для вывода меню");
                    }
                    else
                    {
                        dop.OutputToFile(dataToFile, columnNames);
                        Console.WriteLine("Нажми любую клавишу для вывода меню");
                    }
                }
                
            }
            else
            {
                Console.WriteLine("Невозможно выполнить действие, т.к. данные из файла не загружены.");
            }

            keyToExit = Console.ReadKey();
        } while (keyToExit.Key != ConsoleKey.Escape);
    }
}