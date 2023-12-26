using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebSocketClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ClientWebSocket webSocket = new ClientWebSocket();
        Uri serverUri = new Uri("ws://localhost:8080");
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void buttonstart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await webSocket.ConnectAsync(serverUri, CancellationToken.None);

                // Start listening for messages from the server
                _ = ReceiveMessage(webSocket);
                MessageBox.Show("status=open");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //finally
            //{
            //    if (webSocket.State == WebSocketState.Open)
            //    {
            //        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", CancellationToken.None);
            //    }
            //}
        }
        public async Task ReceiveMessage(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Received message from server: {message}");
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server closed", CancellationToken.None);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public async Task SendMessage(ClientWebSocket webSocket, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async void buttonsend_Click(object sender, RoutedEventArgs e)
        {
            var message = textmessage.Text;

            if (message.ToLower() == "exit")
            {
                return;
            }

            await SendMessage(webSocket, message);
        }
    }
}

