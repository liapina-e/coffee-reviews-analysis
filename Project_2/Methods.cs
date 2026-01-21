using System.Text;

namespace Project_2;
/// <summary>
/// В классе Methods представлены методы для выполения 2-4 пункта меню.
/// </summary>
public static class Methods
{
    /// <summary>
    /// В методе ReadFile создаем массив строк, состоящий из данных из файла.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="arr"></param>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void ReadFile(string path, out string[] arr)
    {
        try
        {
            // Если файла не существует или он состоит только из некорректынх данных, то обрабатываем исключения.
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            if (path.Length == 0 || path is null)
            {
                throw new ArgumentException();
            }
            arr = File.ReadAllLines(path);
            
        }
        // Понять, как сделать так, чтоб не выбрасывало из программы
        catch (FileNotFoundException)
        {
            Console.Write("Входной Файл на диске отсутствует.");
            Console.WriteLine("Введите другой путь к файлу.");
            throw;
        }
        catch (ArgumentException)
        {
            Console.Write("Корректных данных в файле нет.");
            Console.WriteLine("Введите другой путь к файлу.");
            throw;
        }
        catch (IOException)
        {
            Console.Write("Проблемы с открытием файла");
            Console.WriteLine("Введите другой путь к файлу.");
            throw;
        }
        
    }
    /// <summary>
    /// В методе GetReview возвращаем строчный массив, состоящий из строк из файла в нужном для нас формате.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string[] GetReview(string str)
    {
        // Переменная смотрит, находимся ли мы в одинарных кавычках или нет.
        bool weAreInSingleQuotes = false;
        // Заменяем кавычки и запятые на символы, которых нет файле, чтоб потом строки коррекно разделились.
        str = str.Replace("\"\"", "|");
        string s1 = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '"' && weAreInSingleQuotes)
            {
                weAreInSingleQuotes = false;
                s1 += str[i];
            }
            else if (str[i] == '"')
            {
                weAreInSingleQuotes = true;
                s1 += str[i];
            }
            else
            {
                if (str[i] == ',' && weAreInSingleQuotes)
                {
                    s1 += '^';
                }
                else
                {
                    s1 += str[i];
                }
            }
        }
        string[] almostDone = s1.Split(',');
        string[] result = new string[6];
        result[0] = almostDone[0];
        result[1] = almostDone[1][1..^1].Replace('^', ',');
        result[2] = almostDone[2][1..^1].Replace('^', ',');
        result[3] = almostDone[3].Replace('^', ',');
        result[4] = almostDone[4][1..^1].Replace('^', ',').Replace("|", "\"\"");
        result[5] = almostDone[5].Replace('^', ',').Replace("|", "\"\"");
        return result;
    }
    /// <summary>
    /// В методе CreateReviews формруем и возвращаем массив - объект класса Review.
    /// В нем хранятся все необходимые для нас данные.
    /// Также метод возвращает названия стобцов таблицы для дальнейшей работы с ними.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="reviews"></param>
    /// <param name="columnNames"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public static void CreateReviews(string path, out Review[] reviews, out string columnNames)
    {
        ReadFile(path, out string[] arr);
        reviews = new Review[arr.Length-2];
        string[] res;
        // Проверяем корректны ли названия столбцов в файле.
        try
        {
            if (arr[0] != "name,location,Date,Rating,Review,Image_Links")
            {
                throw new FileNotFoundException();
            }
        }
        
        catch(FileNotFoundException)

        {
            Console.WriteLine("Шапка не соответствует файлу.");
            Console.WriteLine("Попробуйте еще раз.");
        }
        // Переменная содержит названия столбцов в файле.
        columnNames = arr[0];
        for (int i = 1; i <arr.Length-1; i++)
        {
            res = GetReview(arr[i]);
            reviews[i-1] = new Review(res);
        }
        res = GetReview(arr[^1]);
        reviews[^1] = new Review(res);
    }
    /// <summary>
    /// В методе ReviewHighestRating находим максимальный рейтинг в 2020 и 2021 годах для пункта 3 из меню.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="max_20"></param>
    /// <param name="max_21"></param>
    public static void ReviewHighestRating(Review[] reviews, out int max_20, out int max_21)
    {
        max_20 = 0; // Максимальный рейтинг в 2020 году.
        max_21 = 0; // Максимальный рейтинг в 2021 году.
        for (int i = 0; i < reviews.Length; i++)
        {
            if (reviews[i].Rating != "N/A")
            {
                int rating = int.Parse(reviews[i].Rating);
                if (rating > max_20 & int.Parse(reviews[i].Date[1]) == 2020)
                {
                    max_20 = rating;
                }
                else if (rating> max_21 & int.Parse(reviews[i].Date[1]) == 2021)
                {
                    max_21 = rating;
                }
            }
            
        }
    }
    /// <summary>
    /// В методе OutputOfHighestRating выводим отзывы с максимальным рейтингом за 2020 и 2021 года.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="max_20"></param>
    /// <param name="max_21"></param>
    public static void OutputOfHighestRating(Review[] reviews, int max_20, int max_21)
    {
        for (int i = 0; i < reviews.Length; i++)
        {
            if (reviews[i].Rating != "N/A")
            {
                int rating = int.Parse(reviews[i].Rating);
                if (rating == max_20 & int.Parse(reviews[i].Date[1]) == 2020)
                {
                    Console.WriteLine(reviews[i].Name);
                    Console.WriteLine(reviews[i].Location);
                    Console.WriteLine(reviews[i].Date[0] + " " + reviews[i].Date[1]);
                    Console.WriteLine(reviews[i].Rating);
                    Console.WriteLine(reviews[i]._Review);
                    for (int j = 0; j < reviews[i].Image_Links.Length; j++)
                    {
                        Console.WriteLine(reviews[i].Image_Links[j]);
                    }
                    Console.WriteLine();
                }
                else if (rating == max_21 & int.Parse(reviews[i].Date[1]) == 2021)
                {
                    Console.WriteLine(reviews[i].Name);
                    Console.WriteLine(reviews[i].Location);
                    Console.WriteLine(reviews[i].Date[0] + " " + reviews[i].Date[1]);
                    Console.WriteLine(reviews[i].Rating);
                    Console.WriteLine(reviews[i]._Review);
                    for (int j = 0; j < reviews[i].Image_Links.Length; j++)
                    {
                        Console.WriteLine(reviews[i].Image_Links[j]);
                    }
                    Console.WriteLine();
                }
            }

        }
    }
    /// <summary>
    /// В методе ReviewsWithRatingN ныходим и сохраняем в файл отзывы с рейтингом, введенным пользователем.
    /// </summary>
    /// <param name="N"></param>
    /// <param name="reviews"></param>
    /// <param name="arr"></param>
    /// <param name="columnNames"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>

    public static void ReviewsWithRatingN(int N, Review[] reviews, string[] arr, string columnNames)
    {
        // Проверяем корректность данных от пользователя.
        try
        {
            if (N < 1 || N > 5)
            {
                throw new ArgumentOutOfRangeException();
            }
            // Вводим StringBuilder, в котором будут хранится нужные строки файла.
            StringBuilder output = new StringBuilder();
            output.AppendLine(columnNames);
            for (int i = 0; i < reviews.Length; i++)
            {
                if (reviews[i].Rating != "N/A")
                {
                    if (int.Parse(reviews[i].Rating) == N)
                    {
                        output.AppendLine(arr[i+1]);
                    }
                        
                }
            }
            WritingToFile(output.ToString());
            Console.WriteLine("Данные успешно загружены в файл.");
        }
        
        catch(ArgumentOutOfRangeException)
        {
            Console.WriteLine("Введенные данные некоррректны."); //Возвращает меню пользователю
        }
    }
    /// <summary>
    /// В методе WritingToFile записываем нужные данный в файл, требуемый в задаче.
    /// </summary>
    /// <param name="StrOfReview"></param>

    public static void WritingToFile(string StrOfReview)
    {
        // Проверяем правильно ли сохраняются данные.
        try
        {
            string path_out = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}top-reviews-20-21.csv";
            File.WriteAllText(path_out, StrOfReview);
        }
        
        catch (IOException)
        {
            Console.WriteLine("Проблемы с сохранением файла.");
            throw; // Должно ли выкидывать?
        }
    }
}