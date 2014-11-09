// Democode from: https://2lemetry.atlassian.net/wiki/pages/viewpage.action?pageId=15335494

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
namespace MQTT_Connection
{
    class Program
    {
        const string USERNAME = "<your ThingFabric username here>";
        const string TOKEN_HASH = "<your (pre-hashed) ThingFabric token here>";
        const string TOPIC = "[your domain]/test-stuff/test-thing";
        const string PAYLOAD = "{\"Hello\":\"World!\"}";
        static void Main(string[] args)
        {
            Console.WriteLine("Connecting...");
            MqttClient client = new MqttClient(IPAddress.Parse("75.101.161.236"));
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;     // Define what function to call when a message arrives
            client.MqttMsgDisconnected += client_MqttMsgDisconnected;           // Define what function to call when client disconnects
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;               // Define what function to call when a subscription is acknowledged
            client.MqttMsgPublished += client_MqttMsgPublished;                 // Define what function to call when a message is published
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId, USERNAME, TOKEN_HASH);
            client.Subscribe(new string[] { TOPIC }, new byte[] { 0 });
            client.Publish(TOPIC, Encoding.UTF8.GetBytes(PAYLOAD));
            Console.WriteLine("Message Published!");
        }
		
		// uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishedEventArgs
        private static void client_MqttMsgPublished(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishedEventArgs e)
        {
            Console.Write("Message " + e.MessageId + " has been sent.");
        }
        // Handle subscription acknowledgements
        private static void client_MqttMsgSubscribed(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Subscribed!");
        }
        // Handle disconnections
        private static void client_MqttMsgDisconnected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        // Handle incoming messages
        private static void client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Message received:");
            Console.WriteLine(Encoding.UTF8.GetString(e.Message));
        }
    }
}