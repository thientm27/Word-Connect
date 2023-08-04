using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// I wrote this script only for create a Json file from custom .txt file
    /// so that this scripts will be not use in game
    /// </summary>
    public class TxtToJson : MonoBehaviour
    {
        private readonly string filePath = "Assets/Dictionary.txt";
        private readonly string outputPath = "Assets/GameData.json";

        void Start()
        {
            List<Item> items = ReadItemsFromFile(filePath);
            Debug.Log("Read success");

            SaveItemsToJsonFile(items, outputPath);
        }

        public List<Item> ReadItemsFromFile(string filePath)
        {
            List<Item> items = new List<Item>();
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                string currentName = null;
                var descriptionBuilder = new StringBuilder();

                foreach (string line in lines)
                {
                    if (line.StartsWith("@"))
                    {
                        if (!string.IsNullOrEmpty(currentName) && descriptionBuilder.Length > 0)
                        {
                            string description = RemoveUnwantedPrefixes(descriptionBuilder.ToString());
                            Item item = new Item
                            {
                                Name = currentName,
                                Description = description.Trim()
                            };
                            items.Add(item);
                            descriptionBuilder.Clear();
                        }

                        currentName = line.Substring(1).Trim();
                    }
                    else
                    {
                        descriptionBuilder.AppendLine(line.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(currentName) && descriptionBuilder.Length > 0)
                {
                    string description = RemoveUnwantedPrefixes(descriptionBuilder.ToString());
                    Item item = new Item
                    {
                        Name = currentName,
                        Description = description.Trim()
                    };
                    items.Add(item);
                }
            }

            return items;
        }

        private static string RemoveUnwantedPrefixes(string description)
        {
            return description;
        }

        public static void SaveItemsToJsonFile(List<Item> items, string filePath)
        {
            string jsonString = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }

        public class Item
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}