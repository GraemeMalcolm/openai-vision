# python -m venv labenv
# ./labenv/bin/Activate.ps1
# pip install python-dotenv azure-identity azure-ai-projects
# pip install openai

import os
from urllib.request import urlopen, Request
import base64
from pathlib import Path

# Add references
from dotenv import load_dotenv
from azure.identity import DefaultAzureCredential
from azure.ai.projects import AIProjectClient
#from azure.ai.inference.models import (
#    SystemMessage,
#    UserMessage,
#    TextContentItem,
#    ImageContentItem,
#    ImageUrl,
#)
import openai


def main(): 

    # Clear the console
    os.system('cls' if os.name=='nt' else 'clear')
        
    try: 
    
        # Get configuration settings 
        load_dotenv()
        project_connection = os.getenv("PROJECT_CONNECTION")
        model_deployment = os.getenv("MODEL_DEPLOYMENT")



        # Initialize the project client
        project_client = AIProjectClient.from_connection_string(
            conn_str=project_connection,
            credential=DefaultAzureCredential())

        

        # Get a chat client
        chat_client = project_client.inference.get_azure_openai_client(api_version="2024-10-21")
        



        # Initialize prompts
        system_message = "You are an AI assistent in a grocery store that sells fruit."
        prompt = ""

        # Loop until the user types 'quit'
        while True:
            prompt = input("\nAsk a question about the image\n(or type 'quit' to exit)\n")
            if prompt.lower() == "quit":
                break
            elif len(prompt) == 0:
                    print("Please enter a question.\n")
            else:
                print("Getting a response ...\n")


                # Get a response to image input
                script_dir = Path(__file__).parent  # Get the directory of the script
                image_path = script_dir / 'mystery-fruit.jpeg'
                mime_type = "image/jpeg"

                # Read and encode the image file
                with open(image_path, "rb") as image_file:
                    base64_encoded_data = base64.b64encode(image_file.read()).decode('utf-8')

                # Include the image file data in the prompt
                data_url = f"data:{mime_type};base64,{base64_encoded_data}"

                response = chat_client.chat.completions.create(
                    model=model_deployment,
                    messages=[
                        { "role": "system", "content": system_message },
                        { "role": "user", "content": [  
                            { 
                                "type": "text", 
                                "text": prompt 
                            },
                            { 
                                "type": "image_url",
                                "image_url": {
                                    "url": data_url
                                }
                            }
                        ] } 
                    ]
                )
                completion = response.choices[0].message.content
                print(completion)
                    


    except Exception as ex:
        print(ex)


if __name__ == '__main__': 
    main()