namespace Project_2;
/// <summary>
/// В классе Statistics привидены методы для решения пункта 5 из меню целиком.
/// </summary>
public class Statistics 
{
    /// <summary>
    /// Метод MaxYear возвращает максимальный год, в который написан отзыв.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="max_year"></param>
    public void MaxYear(Review[] reviews, out int max_year)
    {
        max_year = 0;
        for (int i = 0; i < reviews.Length; i++)
        {
            int year = Convert.ToInt32(reviews[i].Date[1]);
            if (year > max_year)
            {
                max_year = year;
            }
        }
    }
    /// <summary>
    /// Метод MinYear возвращает минимальный год, в который написан отзыв.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="min_year"></param>
    public void MinYear(Review[] reviews, out int min_year)
    {
        min_year = 100000;
        for (int i = 0; i < reviews.Length; i++)
        {
            int year = int.Parse(reviews[i].Date[1]);
            if (year < min_year)
            {
                min_year = year;
            }
        }
    }
    /// <summary>
    /// Метод ArrayOfYears возвращает массив, состоящий из всех годов, в которые были написаны отзывы.
    /// </summary>
    /// <param name="reviews"></param>
    /// <returns></returns>
    public int[] ArrayOfYears(Review[] reviews)
    {
        MaxYear(reviews, out int max_year);
        MinYear(reviews, out int min_year);
        int[] arrOfYears = new int[max_year- min_year + 1]; 
        int year = min_year;
        for (int i = 0; i < arrOfYears.Length; i++)
        {
            arrOfYears[i] = year;
            year++;
        }
        return arrOfYears;
    }
    /// <summary>
    /// Метод возвращает количество отзывов, полученных в каждом году.
    /// </summary>
    /// <param name="reviews"></param>
    /// <returns></returns>
    public int[] CountReviewsByYears(Review[] reviews)
    {
        MaxYear(reviews, out int max_year);
        int[] count = new int[max_year+1];

        for (int i = 0; i < reviews.Length; i++)
        {
            int year = Convert.ToInt32(reviews[i].Date[1]);
            count[year]++;
        }
        return count;
    }
    /// <summary>
    /// Метод выводит данные, требуемые для задачи 4.1.
    /// </summary>
    /// <param name="reviews"></param>
    public void WritingCountReviewsByYears(Review[] reviews)
    {
        int[] count = CountReviewsByYears(reviews);
        int[] arrOfYears = ArrayOfYears(reviews);
        for (int i = 0; i < arrOfYears.Length; i++)
        {
            Console.WriteLine($"В {arrOfYears[i]} году было получено {count[arrOfYears[i]]}");
        }
    }
    /// <summary>
    /// Метод возвращает целочисленный массив, содержащий количество отзывов с разным рейтингом.
    /// </summary>
    /// <param name="reviews"></param>
    /// <returns></returns>
    public int[] PercentageOfReviews(Review[] reviews)
    {
        int[] countOfRatings = new int[7];
        for (int i = 0; i < reviews.Length; i++)
        {
            if (reviews[i].Rating != "N/A")
            {
                int rating = Convert.ToInt32(reviews[i].Rating);
                countOfRatings[rating]++;
            }
            else
            {
                countOfRatings[6]++;
            }

        }
        return countOfRatings;
    }
    /// <summary>
    /// В методе выводится процентное соотношение отзывов с разным рейтингом к общему числу отзывов.
    /// </summary>
    /// <param name="reviews"></param>
    public void WritingPercentageOfReviews(Review[] reviews)
    {
        int[] countOfRatings = PercentageOfReviews(reviews);
        // Переменная отвечает за соотношение отзывов.
        double ratio = 0;
        for (int i = 1; i < countOfRatings.Length-1; i++)
        {
            ratio = ((double)countOfRatings[i] / reviews.Length) * 100;
            Console.WriteLine($"Процентное отношение отзывов с рейтингом {i} " +
                              $"по отношению к общему числу: {ratio}%");
        }
        ratio = (((double)countOfRatings[countOfRatings.Length - 1]) / reviews.Length) * 100;
        Console.WriteLine($"Процентное отношение отзывов с рейтингом N/A " +
                          $"по отношению к общему числу: {ratio}%");
    }
    /// <summary>
    /// Метод возращает кол-во отзывов с рейтингом 1 с картинкой и без нее.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="rewievsWithImage"></param>
    /// <param name="reviewsWithoutImage"></param>
    public void RewievsWithImage(Review[] reviews, out int rewievsWithImage, out int reviewsWithoutImage)
    {
        rewievsWithImage = 0;
        reviewsWithoutImage = 0;
        for (int i = 0; i < reviews.Length; i++)
        {
            if (reviews[i].Rating == "1")
            {
                if (reviews[i].Image_Links[0] != "'No Images'")
                {
                    rewievsWithImage++;
                }
                else
                {
                    reviewsWithoutImage++;
                }
            }
        }
    }
    /// <summary>
    /// В методе выводятся на экран данняе, нужные для пункта 4.3
    /// </summary>
    /// <param name="reviews"></param>
    public void RatioOfReviews(Review[] reviews)
    {
        RewievsWithImage(reviews, out int reviewsWithImage, out int reviewsWithoutImage);
        Console.WriteLine($"Соотношение отзывов с рейтингом 1, у которых есть изображение," +
                          $"к отзывам без него: {reviewsWithImage}/{reviewsWithoutImage}");
    }
    /// <summary>
    /// Метод возращает кол-во тзывов с рейтингом 1-2 и 3-4.
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="count_12"></param>
    /// <param name="count_34"></param>
    public void CountsReviewsWithRating(Review[] reviews, out int[] count_12, out int[] count_34)
    {
        MaxYear(reviews, out int max_year);
        // Переменная отвечает за кол-во отзывов с рейтингоом 1-2.
        count_12 = new int[max_year+1];
        // Переменная отвечает за кол-во отзывов с рейтингоом 3-4.
        count_34 = new int[max_year+1];
        for (int i = 0; i < reviews.Length; i++)
        {
            int year = Convert.ToInt32(reviews[i].Date[1]);
            if (reviews[i].Rating == "1" || reviews[i].Rating == "2")
            {
                count_12[year]++;
            }
            else if (reviews[i].Rating == "3" || reviews[i].Rating == "4")
            {
                count_34[year]++;
            }
        }
    }
    /// <summary>
    /// В методе выовдятся данные, необходимые в пункте 4.4
    /// </summary>
    /// <param name="reviews"></param>
    /// <param name="count_12"></param>
    /// <param name="count_34"></param>
    public void WritingCountsReviewsWithRating( Review[] reviews, int[] count_12, int[] count_34)
    {
        int[] arrOfYears = ArrayOfYears(reviews);
        for (int i = 0; i < arrOfYears.Length; i++)
        {
            Console.WriteLine($"Количество отзывов с рейтингом 1-2: {count_12[arrOfYears[i]]} в {arrOfYears[i]} году");
        }
        for (int i = 0; i < arrOfYears.Length; i++)
        {
            Console.WriteLine($"Количество отзывов с рейтингом 3-4: {count_34[arrOfYears[i]]} в {arrOfYears[i]} году");
        }
    }
}