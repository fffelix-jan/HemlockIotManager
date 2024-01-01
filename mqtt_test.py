import paho.mqtt.client as mqtt
import random
import json
import datetime
import time

# Configuration
broker_address = "127.0.0.1"  # Replace with your MQTT broker address
broker_port = 1883  # Replace with your MQTT broker port
topic = "gps/coordinates"
device_id = 2  # Replace with your device ID
owner_id = "921f4990-afde-4c08-8fa7-d7e431b65903"  # Replace with the owner ID of the device

# Function to generate random coordinates near Hangzhou
def generate_random_coordinates():
    lat = 30.2741 + random.uniform(-0.01, 0.01)
    lon = 120.1551 + random.uniform(-0.01, 0.01)
    return lat, lon

# Callback for when the client receives a CONNACK response from the server
def on_connect(client, userdata, flags, rc):
    print(f"Connected with result code {rc}")

# Create an MQTT client and attach the on_connect callback
client = mqtt.Client()
client.on_connect = on_connect

# Connect to the MQTT broker
client.connect(broker_address, broker_port, 60)

# Start a loop to keep the script running and to maintain the connection
client.loop_start()

try:
    while True:
        latitude, longitude = generate_random_coordinates()
        payload = {
            "DeviceID": device_id,
            "OwnerID": owner_id,
            "LogDateTime": datetime.datetime.now().isoformat(),
            "Latitude": latitude,
            "Longitude": longitude,
            "Altitude": random.uniform(0, 100),  # Random altitude
            "BatteryLevel": random.uniform(0, 100),  # Random battery level
            "LogMessage": "Random log message"
        }

        # Publish the payload
        client.publish(topic, json.dumps(payload))
        print(f"Sent payload: {payload}")

        # Wait for some time before sending the next payload
        time.sleep(5)  # Sends a payload every 5 seconds

except KeyboardInterrupt:
    client.loop_stop()
    client.disconnect()
