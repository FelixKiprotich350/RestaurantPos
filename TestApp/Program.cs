using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = -2;
            synthesizer.SelectVoice(synthesizer.GetInstalledVoices()[3].VoiceInfo.Name);
            // Synchronous
            synthesizer.Speak("Ticket Number ....... one      two three four proceed to counter number 12.");

            // Asynchronous
            synthesizer.SpeakAsync("Hello World");
            // Create an in-process speech recognizer for the en-US locale.  
            //using ( SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine( new System.Globalization.CultureInfo("en-US")))
            //{

            //    // Create and load a dictation grammar.  
            //    recognizer.LoadGrammar(new DictationGrammar());
            //    Console.WriteLine("Starting....");
            //    // Add a handler for the speech recognized event.  
            //    recognizer.SpeechRecognized +=
            //      new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            //    // Configure input to the speech recognizer.  
            //    recognizer.SetInputToDefaultAudioDevice();

            //    // Start asynchronous, continuous speech recognition.  
            //    recognizer.RecognizeAsync(RecognizeMode.Multiple);

            //    // Keep the console window open.  
            //    while (true)
            //    {
            //        Console.ReadLine();
            //    }
            //}
        }

        // Handle the SpeechRecognized event.  
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognized text: " + e.Result.Text);
        }
    }
}
