import serial
import time
import paho.mqtt.client as mqtt
import json
import RPi.GPIO as GPIO
from datetime import datetime
msgString = ""

# The callback for when the client receives a CONNACK response from the server.
def on_connect(client, userdata, flags, rc):
    print("Connected To server "+str(rc))

    # Subscribing in on_connect() means that if we lose the connection and
    # reconnect then subscriptions will be renewed.
    client.subscribe("workout/#")

# The callback for when a PUBLISH message is received from the server.
def on_message(client, userdata, msg):
    global msgString

    try:
        time_obj_seconds = datetime.strptime(str(json.loads(msg.payload)[1]["Intervals"][0]["time"]), '%H:%M:%S').strftime('%S')
        time_obj_minutes = datetime.strptime(str(json.loads(msg.payload)[1]["Intervals"][0]["time"]), '%H:%M:%S').strftime('%M')
        time_obj_hours = datetime.strptime(str(json.loads(msg.payload)[1]["Intervals"][0]["time"]), '%H:%M:%S').strftime('%H')

        time_miliseconds = (int(time_obj_seconds) + (int(time_obj_minutes) * 60) + (int(time_obj_hours) * 3600)) * 1000

        msgString = "xxxxx&{0}&{1}&{2}%".format(str(time_miliseconds), str(json.loads(msg.payload)[1]["Intervals"][0]["speed"]), str(json.loads(msg.payload)[0])[1:7])
        print(msgString)
    except:
        pass


def arduino_datarequest(pin):
    global msgString
    ser = serial.Serial('/dev/ttyAMA0', 11800, timeout=1)
    if not msgString == "":
        ser.flush()
        ser.write(str.encode(msgString))
        msgString = ""
 







GPIO.setmode(GPIO.BCM) # choose the pin numbering
GPIO.setup(26, GPIO.IN, GPIO.PUD_DOWN)
GPIO.add_event_detect(26, GPIO.FALLING, callback=arduino_datarequest, bouncetime=100)

client = mqtt.Client()
client.username_pw_set("ledsrun", password="N9A0zrF6XKdHRqRl")


client.on_connect = on_connect
client.on_message = on_message

client.connect("52.174.57.98", 1883, 60)

# Blocking call that processes network traffic, dispatches callbacks and
# handles reconnecting.
# Other loop*() functions are available that give a threaded interface and a
# manual interface.
client.loop_forever()
