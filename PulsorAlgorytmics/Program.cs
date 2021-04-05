using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PulsorAlgorytmics
{
    public class ProcessingOfData
    {
        string name;
        int age;
        int height;
        int weight;

        public ProcessingOfData(string n, int a, int h, int w)
        {
            name = n;
            age = a;
            height = h;
            weight = w;
        }

        void GetInfo()
        {
            Console.WriteLine($"ФИО: {name} Возраст: {age} Рост: {height} Вес: {weight}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введи ФИО объекта ");
            string n = Console.ReadLine();
            Console.WriteLine("Введи возраст объекта");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введи рост объекта");
            int h = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введи вес объекта ");
            int w = Convert.ToInt32(Console.ReadLine());
            var lib = new ProcessingOfData(n, a, h, w);
            var tochki = DataFromFile.Get(@"C:\data.txt");
            var HR = Pulse.GetHR(tochki);
            var MedianPhiltr = Pulse.MedianPhiltr(tochki);
            Console.ReadKey();
        }
    }
    static class DataFromFile
    {
        static public List<double> Get(string path) // объявление метода
        {
            List<double> ojerele = new List<double>(); // создал список пустой (list)
            using (StreamReader s = new StreamReader(path))
            {
                string dannie = s.ReadToEnd();
                string[] BoxWithData = dannie.Split(' '); //обратились к данным и разделили их пробелом и вернули в Box коробку с данными

                for (int i = 0; i < BoxWithData.Length; i++) //length считает колличество в коробке
                {
                    ojerele.Add(Convert.ToDouble(BoxWithData[i])); //ожерелье бери из коробки листочки 
                }
            }
            return ojerele;
        }

    }

    static class Pulse
    {
        static public List<int> GetHR(List<double> ojerele)
        {
            List<int> IndexiEpizodov = new List<int>();
            List<int> IndexiAmplitudas = new List<int>();
            List<int> ChSSMgnovennaya = new List<int>();
            for (int i = 1; i < ojerele.Count - 1; i++)
            {
                if (ojerele[i] < 50)
                {
                    if (ojerele[i - 1] > ojerele[i] && ojerele[i + 1] > ojerele[i])
                    {
                        IndexiEpizodov.Add(i); // коробка с точками минимумов
                    }
                }
            }
            for (int i = 0; i < IndexiEpizodov.Count - 1; i++)
            {
                List<double> braslet = ojerele.GetRange(IndexiEpizodov[i], IndexiEpizodov[i + 1] - IndexiEpizodov[i]);
                double Amplituda = braslet.Max();
                IndexiAmplitudas.Add(IndexiEpizodov[i] + braslet.IndexOf(Amplituda));
            }
            for (int i = 1; i < IndexiAmplitudas.Count; i++)
            {
                ChSSMgnovennaya.Add((100 * 60) / (IndexiAmplitudas[i] - IndexiAmplitudas[i - 1]));
            }
            return ChSSMgnovennaya;

        }
        static public List<double> MedianPhiltr(List<double> ojerele) // метод Медианный фильтр
        {
            List<double> NormTrechki = new List<double>();
            int k;
            k = ojerele.Count % 3;
            for (int i = 0; i < ojerele.Count - k; i += 3)
            {
                List<double> trechki = ojerele.GetRange(i, 3); // массив с трешками не отсортированнми
                trechki.Sort(); // отсортировали по порядку 
                NormTrechki.Add(trechki[1]); // добавляем в нормтрешки 2 значение
            }
            return NormTrechki;
        }
    }
}
