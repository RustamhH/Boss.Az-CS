using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Final_Project_x_Boss.Az.Models.Other
{
    internal sealed class JsonFileHandler
    {

        public static T Read<T>(string filePath) where T:new()
        {
            
            JsonSerializerOptions op = new JsonSerializerOptions();
            using FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            return JsonSerializer.Deserialize<T>(fs, op);
            
            
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
