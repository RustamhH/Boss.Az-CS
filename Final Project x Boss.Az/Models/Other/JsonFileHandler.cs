using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Final_Project_x_Boss.Az.Models.Other
{
    internal sealed class JsonFileHandler
    {

        public static T Read<T>(string filePath)
        {
            try
            {
                string text = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }
        public static void Write<T>(string filePath, T values)
        {
            try
            {
                JsonSerializerOptions op = new JsonSerializerOptions();
                op.WriteIndented = true;
                File.WriteAllText(filePath, JsonSerializer.Serialize(values, op));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }


    }
}
