using System;
using System.IO;
using System.Runtime.InteropServices;

class Programm
{
    public static void Main(string[] args)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(@"C:\Users\shabalin_sa\Desktop\папка");
        long size = LengthFiles(dirInfo); // расчет исходного объема файлов

        if (dirInfo.Exists) // проверка
            Console.WriteLine("\n\tДиректорий: " + dirInfo + "");
        long size1 = size;
        Console.WriteLine("\n\t" + size1 + " байт - исходный размер директория");

        DeleteFiles(dirInfo); // удаления файлов

        size = LengthFiles(dirInfo); // повторный расчета объема файлов
        long size2 = size;
        Console.WriteLine("\n\t" + size2 + " байт - размер директория после очистки");

        long size3 = size1 - size2;
        Console.WriteLine("\n\t" + size3 + " байт - очищено");
    }

    /// <summary>
    /// Рекурсия - расчет объема файлов в директории
    /// </summary>
    /// <param name="dirInfo"></param>
    /// <returns></returns>
    public static long LengthFiles(DirectoryInfo dirInfo)
    {
        long size = 0;

        FileInfo[] fff = dirInfo.GetFiles();
        foreach (FileInfo ff in fff) // перебор файлов в директории
        {
            size += ff.Length; // считаем объем, суммируем
        }

        DirectoryInfo[] ddd = dirInfo.GetDirectories();
        foreach (DirectoryInfo dd in ddd) // перебор директории
        {
            try
            {
                size += LengthFiles(dd); // считаем объем, суммируем
                dirInfo = dd;
            }
            catch (Exception e) // проверка исключений
            {
                Console.WriteLine(e.Message);
            }
        }
        return size;
    }

    /// <summary>
    /// Рекурсия - удаление файлов, сохраненых более 30 минут назад
    /// </summary>
    /// <param name="dirInfo"></param>
    public static void DeleteFiles(DirectoryInfo dirInfo)
    {
        FileInfo[] fff = dirInfo.GetFiles();
        foreach (FileInfo ff in fff) // перебор файлов в директории
        {
            DateTime lastWriteTime = File.GetLastWriteTime("" + ff); // время последнего сохранения 
            TimeSpan timeSpan = TimeSpan.FromMinutes(30); // допустимое время
            DateTime minTime = DateTime.Now.Subtract(timeSpan); // допустимое время

            if (lastWriteTime < minTime) // если время последнего сохранения < допустимого
            {
                File.Delete("" + ff);
            }
        }

        DirectoryInfo[] ddd = dirInfo.GetDirectories();
        foreach (DirectoryInfo dd in ddd) // перебор директории
        {
            try
            {
                dirInfo = dd;
                DeleteFiles(dirInfo);
            }
            catch (Exception e) // проверка исключений
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}