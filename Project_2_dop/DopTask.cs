using System.Globalization;
using System.Text;

namespace Project_2_dop;

public class DopTask
{
    /// <summary>
    /// В методе SelectionOfReviews выводим на экран выборку записей по отзывам в одном и том же месте.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="location_"></param>
    /// <param name="arr"></param>
    public void SelectionOfReviews(Review[] reviews, string location_, string[] arr)
    {
        // Переменная хранит в себе подходящие строки массива.
        StringBuilder selection = new StringBuilder();
        for (int i = 0; i < reviews.Length; i++)
        {
            if (reviews[i].Location == $"{location_}")
            {
                Console.WriteLine(reviews[i].Image_Links[0]);
                if (reviews[i].Image_Links[0] == "'No Images'")
                {
                    selection.AppendLine($"{reviews[i].Name}, {reviews[i].Location}, {reviews[i].Date[0] + reviews[i].Date[1]}, " +
                                         $"{reviews[i].Rating}, {reviews[i]._Review}, {reviews[i].Image_Links[0]} ");
                }
            }
        }
        // Проверяем, верное ли место ввел пользователь.
        if (selection.ToString() == "")
        {
            Console.WriteLine("Такого места не существует или нет такой выборки." +
                          "Попробуйте еще раз.");
        }
        else
        {
            Console.WriteLine(selection.ToString());
        }
    }
    /// <summary>
    /// В методе SavingSelection сохраняем нужные отзывы в файл.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="location_"></param>
    /// <param name="arr"></param>
    /// <param name="path_out"></param>
    /// <param name="columnNames"></param>
    public void SavingSelection(Review[] reviews, string location_, string[] arr, string path_out, string columnNames)
    {
        // Переменная хранит в себе нужные строки массива.
        StringBuilder output = new StringBuilder();
        output.AppendLine(columnNames);
        for (int i = 0; i < reviews.Length; i++)
        {
            if (reviews[i].Location == location_)
            {
                if (reviews[i].Image_Links[0] == "'No Images'")
                {
                    output.AppendLine(arr[i+1]);
                }
            }
        }
        // Проверяем, верное ли место ввел пользователь.
        if (output.ToString() == columnNames)
        {
            Console.WriteLine("Такого места не существует или нет такой выборки." +
                              "Попробуйте еще раз.");
        }
        else
        {
            WritingToFile(output.ToString(), path_out);
        }
    }
    public void WritingToFile(string StrOfReview, string path_out)
    {
        try
        {
            File.WriteAllText(path_out, StrOfReview);
            Console.WriteLine("Данные сохранены в файл.");
        }
        
        catch (IOException)
        {
            Console.WriteLine("Проблемы с сохранением файла.");
            throw; // Должно ли выкидывать?
        }
    }
    /// <summary>
    /// Метод возвращает массив массивов, состоящий из дат в нужном для нас формате.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="date"></param>
    public void CreateData(Review[] reviews, out int[][] date)
    {
        // Массив массивов, состоящий из дат.
        date = new int[reviews.Length][];
        for (int i = 0; i < reviews.Length; i++)
        {
            date[i] = new int[3];
            string data = reviews[i].Date[0][9..] + reviews[i].Date[1];
            string format1 = "MMMM dd yyyy";
            string format2 = "MMMM d yyyy";
            string format3 = "MMM. dd yyyy";
            string format4 = "MMM. d yyyy";
            DateTime parsedDate;
            string formattedDate = "";
            if (data[..4] == "Sept")
            {
                data = data[..3] + data[4..];
            }
            if (DateTime.TryParseExact(data, format1, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out parsedDate))
            {
                formattedDate = parsedDate.ToString("d.M.yyyy");
            }
            else if (DateTime.TryParseExact(data, format2, CultureInfo.InvariantCulture, DateTimeStyles.None,
                         out parsedDate))
            {
                formattedDate = parsedDate.ToString("d.M.yyyy");
            }
            else if (DateTime.TryParseExact(data, format3, CultureInfo.InvariantCulture, DateTimeStyles.None,
                         out parsedDate))
            {
                formattedDate = parsedDate.ToString("d.M.yyyy");
            }
            else if (DateTime.TryParseExact(data, format4, CultureInfo.InvariantCulture, DateTimeStyles.None,
                         out parsedDate))
            {
                formattedDate = parsedDate.ToString("d.M.yyyy");
            }
            date[i] = formattedDate.Split('.').Select(int.Parse).ToArray();
        }
    }
    /// <summary>
    /// В методе сортируем все необходимые массивы по датам.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="reviews"></param>
    /// <param name="arr"></param>
    public void Sort(int[][] date, Review[] reviews, string[] arr)
    {
        for (int i = 0; i < reviews.Length; i++)
        {
            int[] current = date[i];
            for (int j = i+1; j < reviews.Length; j++)
            {
                if (current[0] < date[j][0] && current[1] < date[j][1] && current[2] < date[j][2])
                {
                    // Меняем в текущем массиве дат.
                    (current, date[j]) = (date[j], current); 
                    // Меняем в массиве объекта класса.
                    (reviews[i], reviews[j]) = (reviews[j], reviews[i]); 
                    // Меняем в массиве, содержащем изначальные строки файла.
                    (arr[i+1], arr[j+1]) = (arr[j+1], arr[j+1]); 
                }
            }
        }
    }
    // Сортируем по рейтингу
    /// <summary>
    /// Метод возвращает зубчатый массив, состоящий из строк массива reviews.
    /// Метод возвращает зубчатый массив, состоящий из строк массива, содержащего изначальные строки файла.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="arr"></param>
    /// <param name="dataByRating"></param>
    /// <param name="dataToFile"></param>
    public void SortedDataSet(Review[] reviews, string[] arr, out string[][] dataByRating, out string[][] dataToFile)
    {
        // Зубчатый массив, состоящий из строк массива reviews.
        dataByRating = new string[6][];
        // Зубчатый массив, массив, состоящий из строк массива, содержащего изначальные строки файла.
        dataToFile = new string[6][];
        int ind;    // Переменная отвечает за индекс зубчатых массивов для их заполнения по порядку.
        for (int i = 0; i < 5; i++) 
        {
            dataByRating[i] = new string[reviews.Length];
            dataToFile[i] = new string[reviews.Length];
            ind = 0;
            for (int j = 0; j < reviews.Length; j++)
            {
                if (reviews[j].Rating != "N/A")
                {
                    int rating = int.Parse(reviews[j].Rating);
                    if (rating == (i + 1))
                    {
                        dataByRating[i][ind] = $"{reviews[j].Name}, {reviews[j].Location}, {reviews[j].Date[0] + reviews[j].Date[1]}, " +
                                               $"{reviews[j].Rating}, {reviews[j]._Review}";
                        for (int k = 0; k < reviews[j].Image_Links.Length; k++)
                        {
                            dataByRating[i][ind] += reviews[j].Image_Links[k];
                        }

                        dataToFile[i][ind] = arr[j + 1];
                        ind++;
                    }
                }
            }
            // Обрезаем массивы, чтоб они не содержали лишних элементов.
            dataByRating[i] = dataByRating[i][..ind];
            dataToFile[i] = dataToFile[i][..(ind+1)];
        }
        ind = 0;
        dataToFile[5] = new string[reviews.Length];
        dataByRating[5] = new string[reviews.Length];
        for (int j = 0; j < reviews.Length; j++)
        {
            if (reviews[j].Rating == "N/A")
            {
                dataToFile[5][ind] = arr[j + 1];
                dataByRating[5][ind] = $"{reviews[j].Name}, {reviews[j].Location}, {reviews[j].Date[0] + reviews[j].Date[1]}, " +
                                       $"{reviews[j].Rating}, {reviews[j]._Review}";
                for (int k = 0; k < reviews[j].Image_Links.Length; k++)
                {
                    dataByRating[5][ind] += reviews[j].Image_Links[k];
                }
                ind++;
            }
        }
        dataByRating[5] = dataByRating[5][..ind];
        dataToFile[5] = dataToFile[5][..(ind+1)];
        
    }
    /// <summary>
    /// В методе выводим на экран переупорядоченный набор отзывов.
    /// </summary>
    /// <param name="dataByRating"></param>
    public void OutputToScreen(string[][] dataByRating)
    {
        for (int i = 0; i < 6; i++)
        {
            if (i <= 4)
            {
                Console.WriteLine($"Выборка отзывов с рейтингом {i+1}");
            }
            else
            {
                Console.WriteLine($"Выборка отзывов с рейтингом N/A");
            }
            for (int j = 0; j < dataByRating[i].Length; j++)
            {
                Console.WriteLine(dataByRating[i][j]);
            }
        }
    }
    /// <summary>
    /// В методе сохраняем переупорядоченный набор данных в файл.
    /// </summary>
    /// <param name="dataToFile"></param>
    /// <param name="columnNames"></param>
    public void OutputToFile(string[][] dataToFile, string columnNames)
    {
        // Проверяем корректно ли сохранились данные.
        try
        {
            string path_out = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}grouped-rates.csv";
            StringBuilder output = new StringBuilder();
            output.AppendLine(columnNames);
            for (int i = 0; i < dataToFile.Length; i++)
            {
                for (int j = 0; j < dataToFile[i].Length; j++)
                {
                     output.AppendLine(dataToFile[i][j]);
                }
            }
            File.WriteAllText(path_out, output.ToString());
            Console.WriteLine("Данные сохранены в файл.");
        }
        
        catch (IOException)
        {
            Console.WriteLine("Проблемы с сохранением файла.");
        }
    }

    
}