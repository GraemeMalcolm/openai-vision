using System;
using Azure;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net;

// Add references


namespace dalle_client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Clear the console
            Console.Clear();
            
            try
            {
                // Get config settings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string project_connection = configuration["PROJECT_CONNECTION"];
                string model_deployment = configuration["MODEL_DEPLOYMENT"];

                // Initialize the project client


                // Get an OpenAI client


                // Loop until the user types 'quit'
                int imageCount = 0;
                string input_text = "";
                while (input_text.ToLower() != "quit")
                {
                    // Get user input
                    Console.WriteLine("Enter the prompt (or type 'quit' to exit):");
                    input_text = Console.ReadLine();
                    if (input_text.ToLower() != "quit")
                    {
                        // Generate an image


                        // Save the image to a file
                        imageCount++;
                        string fileName = $"image_{imageCount}.png";
                        SaveImage(imageUrl, fileName);
                        
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void SaveImage(Uri uri, string fileName)
        {
            // Create the file path
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            // Download the image and save it to the specified path
            using (var client = new WebClient())
            {
                client.DownloadFile(uri, filePath);
            }
        }
    }
}

