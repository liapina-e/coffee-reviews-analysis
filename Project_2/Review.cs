using System.Net;
using System.Text.RegularExpressions;
namespace Project_2;
/// <summary>
/// В классе Review создаем поля и их свойства, с которым потом будем взаимодействовать на протяжении всей программы.
/// </summary>
public class Review
{
    
    private string name; // Поле, хранящее имя пользователя.
    // Свойства имени пользователя.
    public string Name { get => name; set => name = value; }
    private string location; // Поле, хранящее место написания отзыва.
    // Свойства места отзыва.
    public string Location { get => location; set => location = value; }
    private string[] date;  // Поле, хранящее дату написания отзыва.
    // Свойства даты отзыва.
    public string[] Date { get => date; set => date = value; }
    private string rating;  // Поле, хранящее рейтинг места.
    // Свойства рейтинга отзыва.

    public string Rating{ get => rating; set => rating = value; }

    private string review;  // Поле, хранящее отзыв.
    // Свойства отзыва.
    public string _Review { get => review; set => review = value; }
    private string[] image_links;   // Поле, хранящее картинки к отзыву.
    // Свойства картинок к отзыву.
    public string[] Image_Links { get => image_links; set => image_links = value; }
    /// <summary>
    /// В конструкторе присваиваем значения полям.
    /// </summary>
    /// <param name="result"></param>
    public Review(string[] result)
    {
        name = result[0];
        location = result[1];
        date = result[2].Split(',');
        rating = result[3];
        review = result[4];
        image_links = StringToArr(result[5]);
    }
    /// <summary>
    /// В методе StringToArr преобразуем массив в строку.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string[] StringToArr(string str)
    {
        str = str[(str.IndexOf('[')+1)..(str.IndexOf(']'))];
        string[] arr = str.Split(',');
        // Console.WriteLine(str);
        return arr;
    }

}