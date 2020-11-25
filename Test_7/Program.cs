using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_7
{
    class WeatherParametersDay
    {
        enum TypeOfWeather
        {
            NotDefined = 0,
            Rain = 1,
            ShortTerm_rain = 2,
            Thunder = 3,
            Snow = 4,
            Fog = 5,
            Cloudy = 6,
            Sunny = 7
        }
        private double avarageDayT;
        private double avarageNightT;
        private double avarageAtm;
        private double precipitation;
        private int typeofWeather;

        private double CheckedInput(double value,string meaning)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"Введіть {meaning}: ");
                    value = Convert.ToDouble(Console.ReadLine());
                    return value;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Некоректне значення");
                }
            }
        }

        public void SetFromFile(StreamReader reader,int lin)
        {
            try
            {
                string line = File.ReadLines("weather.txt").Skip(lin).FirstOrDefault();
                string[] param = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (param.Length != 5)
                {
                    Console.WriteLine($"\nНедостатньо даних у рядку {lin}, будь ласка, введіть все\n");
                }
                else
                {
                    try
                    {
                        this.avarageDayT = Double.Parse(param[0]);
                        this.avarageNightT = Double.Parse(param[1]);
                        this.avarageAtm = Double.Parse(param[2]);
                        this.precipitation = Double.Parse(param[3]);
                        this.typeofWeather = Int32.Parse(param[4]);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nОдин з параметрів був введений неправильно\n");
                    }
                }
                
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не знайдено або його не існує");
                Console.ReadKey();
                Environment.Exit(1);
            }

        }

        public void Setter()
        {
            SetAvarageDayT();
            SetAvarageNightT();
            SetAvarageAtm();
            SetPrecipitation();
            SetTypeOfWeather();
        }

        public double SetAvarageDayT()
        {
            this.avarageDayT = CheckedInput(avarageDayT,"середню температуру вдень");
            return this.avarageDayT;
        }
        public double SetAvarageNightT()
        {
            this.avarageNightT = CheckedInput(avarageNightT, "середню температуру вночі");
            return this.avarageNightT;
        }
        public double SetAvarageAtm()
        {
            this.avarageAtm = CheckedInput(avarageAtm, "середній атмосферний тиск");
            return this.avarageAtm;
        }
        public double SetPrecipitation()
        {
            this.precipitation = CheckedInput(precipitation, "кількість опадів");
            return this.precipitation;
        }

        public int SetTypeOfWeather()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Input number of type of weather: \nNot defined - 0\nRain - 1\nDrizzle - 2\nThunder - 3\nSnow - 4\nFog - 5\nCloudy - 6\nSunny - 7\nAnswer: ");
                    typeofWeather = Convert.ToInt32(Console.ReadLine());
                    if (typeofWeather >= 0 && typeofWeather <= 7)
                    {
                        this.typeofWeather = typeofWeather;
                        return this.typeofWeather;
                    }
                    else
                    {
                        Console.WriteLine("Такого типу немає, оберіть один з типів погоди: \n");
                        continue;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неправильно введений тип.Введіть знову: \n");
                    continue;
                }
            }

        }

        public object GetWeather() 
        { 
            return (TypeOfWeather)typeofWeather; 
        }
        public int GetTypeOfWeather() 
        {
            return typeofWeather;
        }
        public double GetAvarageDayT()
        {
            return avarageDayT;
        }
        public double GetAvarageNightT()
        {
            return avarageNightT;
        }
        public double GetAvarageAtm()
        {
            return avarageAtm;
        }
        public double GetPrecipitation()
        {
            return precipitation;
        }
    }

    class WeatherDays
    {
        WeatherParametersDay[] june = new WeatherParametersDay[30];
        public void SetInfo()
        {
            for (int i = 0; i<30; i++)
            {
                WeatherParametersDay day = new WeatherParametersDay();
                june[i] = day;
                day.Setter();
            }
        }

        public void SetFileInfo()
        {
            using (var sr = new StreamReader("weather.txt"))
            {
                var lines = File.ReadAllLines("weather.txt").Where(arg => !string.IsNullOrWhiteSpace(arg));
                sr.Close();
                File.WriteAllLines("weather.txt", lines);
                for (int i = 0; i < 30; i++)
                {
                    WeatherParametersDay day = new WeatherParametersDay();
                    using (var sread = new StreamReader("weather.txt"))
                    {
                        june[i] = day;
                        int l = i;
                        day.SetFromFile(sread,l);
                    }
                }
            }
        }
        public void CountGloomy()
        {
            int gloomyDays = 0;
            for (int i = 0; i < 30; i++)
            {
                if (june[i].GetTypeOfWeather() == 6)
                {
                    gloomyDays++;
                }
            }
            Console.WriteLine($"Похмурих днів: {gloomyDays}");
        }
        public void CountPrecipitationDays()
        {
            int precipitationDays = 0;
            for (int i = 0; i < 30; i++)
            {
                if (june[i].GetTypeOfWeather() == 1 | june[i].GetTypeOfWeather() == 2 | june[i].GetTypeOfWeather() == 4)
                {
                    precipitationDays++;
                }
            }
            Console.WriteLine($"Днів з опадами: {precipitationDays}");
        }
        public void MinMax()
        {
            double min = june[0].GetAvarageNightT();
            double max = june[0].GetAvarageNightT();
            for (int i = 0; i < 30; i++)
            {
                if (june[i].GetAvarageNightT() > max)
                {
                    max = june[i].GetAvarageNightT();
                }
                if (june[i].GetAvarageNightT() < min)
                {
                    min = june[i].GetAvarageNightT();
                }
            }
            Console.WriteLine($"Максимальна середня температура вночі: {max}");
            Console.WriteLine($"Мінімальна середня температура вночі: {min}");
        }
        public void UserHimself()
        {
            SetInfo();
            CountGloomy();
            CountPrecipitationDays();
            MinMax();
        }
        public void FromFile()
        {
            SetFileInfo();
            CountGloomy();
            CountPrecipitationDays();
            MinMax();
        }
    }

    class Program
    {
        private static int Choice(int choice)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Оберіть режим роботи. 0 - введення вручну, 1 - введення з файлу: ");
                    choice = Convert.ToInt32(Console.ReadLine());
                    return choice;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Некоректний вибір");
                }
                
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            WeatherDays june = new WeatherDays();
            int choice = 0;
            while (true)
            {
                choice = Choice(choice);
                if (choice == 0)
                {
                    june.UserHimself();
                    break;
                }
                else if (choice == 1)
                {
                    june.FromFile();
                    break;
                }
                else
                {
                    Console.WriteLine("Будь ласка, виберіть одну із опцій: ");
                }
            }

            Console.ReadKey();
        }
    }
}
