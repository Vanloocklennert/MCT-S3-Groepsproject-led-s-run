using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client;

namespace Leds_Run.repositories
{
    public class RepoMqtt
    {
        private MqttFactory factory = new MqttFactory();
        private IMqttClientOptions options;
        private IMqttClient mqttClient;

        public RepoMqtt()
        {
            options = new MqttClientOptionsBuilder().WithClientId(Guid.NewGuid().ToString()).WithTcpServer("52.174.57.98").WithCredentials("ledsrun", "N9A0zrF6XKdHRqRl").Build();
            mqttClient = factory.CreateMqttClient();
        }

        public async void PublishMessage(string payload, string workoutId)
        {
            MqttApplicationMessage message = new MqttApplicationMessageBuilder().WithTopic($"workout/{workoutId}").WithPayload(payload).WithExactlyOnceQoS().Build();
            await mqttClient.ConnectAsync(options);
            await mqttClient.PublishAsync(message);
            await mqttClient.DisconnectAsync();
        }
    }
}
