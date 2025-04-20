// dotnet add package Azure.Identity
// dotnet add package Azure.AI.Projects --prerelease
// dotnet add package Azure.AI.OpenAI --prerelease

using System;
using Azure;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

// Add references
using Azure.Identity;
using Azure.AI.Projects;
//using Azure.AI.Inference;
using OpenAI.Chat;
using Azure.AI.OpenAI;


namespace chat_app
{
    class Program
    {
        static void Main(string[] args)
        {
            // Clear the console
            Console.Clear();
            
            try
            {
                // Get configuration settings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string project_connection = configuration["PROJECT_CONNECTION"];
                string model_deployment = configuration["MODEL_DEPLOYMENT"];



                // Initialize the project client
                var projectClient = new AIProjectClient(project_connection,
                                    new DefaultAzureCredential());



                // Get a chat client
                ChatClient chatClient = projectClient.GetAzureOpenAIChatClient(model_deployment);




                // Initialize prompts
                string system_message = "You are an AI assistent in a grocery store that sells fruit.";
                string prompt = "";

                // Loop until the user types 'quit'
                while (prompt.ToLower() != "quit")
                {
                    // Get user input
                    Console.WriteLine("\nAsk a question about the image\n(or type 'quit' to exit)\n");
                    prompt = Console.ReadLine().ToLower();
                    if (prompt == "quit")
                    {
                        break;
                    }
                    else if (prompt.Length < 1)
                    {
                        Console.WriteLine("Please enter a question.\n");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Getting a response ...\n");


                        // Get a response to image input
                        string imagePath = "mystery-fruit.jpeg";
                        string mimeType = "image/jpeg";
                      
                        // Read and encode the image file
                        byte[] imageBytes = File.ReadAllBytes(imagePath);
                        var binaryImage = new BinaryData(imageBytes);

                        List<ChatMessage> messages =
                        [
                            new SystemChatMessage(system_message),
                            new UserChatMessage(
                                ChatMessageContentPart.CreateTextPart(prompt),
                                ChatMessageContentPart.CreateImagePart(binaryImage, mimeType)),
                        ];

                        ChatCompletion completion = chatClient.CompleteChat(messages);

                        Console.WriteLine(completion.Content[0].Text);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.Message);
            }
        }
    }
}

