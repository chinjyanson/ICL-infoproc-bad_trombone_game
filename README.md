### Project Information

This project is a group coursework done in Year 2 Imperial EIE for Information Processing. We created a Unity game, that uses the accelerometer of the FPGA to play the game. Unfortunately the game will not work without 2 players and without FPGA's. To make up for that, you'll find below some images of our Project in action!


### Introduction and Purpose

Bad Trombone is a two player music rhythm game where players use FPGA to control visual cursors to match the rhythm bars of the song, and play an equivalent trombone sound as the bars are hit. The two players are able to play by looking at their own screen at different locations, choose between 3 music to play from together to compete with each other, and finally their player names and scores will be shown on a leaderboard for that song. The game is developed mainly using C# in Unity for the game client, Python for server scripts and C in Quartus for utilising the accelerometer sensor as well as detecting a button press on the FPGA board. AWS DynamoDB is utilised to store the leaderboard information for each song.

### System Architecture:

![sysarch](https://github.com/chinjyanson/ICL-infoproc-bad_trombone_game/blob/main/images/sysarch.jpg)

Above is an overall flowchart of the performance view and a diagram for the system design architecture of Bad Trombone, which can be broken down into the following 3 contributing sections:
FPGA controller
Back-end system of information transportation 
Game design using Unity 

### Overview of Components and Design Decisions

## FPGA Controller
# Overview of the FPGA Controller
There are three physical features on the FPGA that are utilised by the game: accelerometer sensor, key0 button and LED lights. FPGAs are tilted up and down by the players to control visual cursors in the game to match the rhythm bars of the song, by which the accelerometer sensor on each FPGA returns a value to determine its titling position. One button from the FPGA is used and its button press determines when the player is proceeding to the next scene of the game (ie from login scene to main game scene), and  when playing the musical game at the main scene, the player needs to press the button as the rhythm bars are hit by the cursor to score and play a trombone sound. The LED lights are used to correspond to the accelerometer sensor’s output value (ie the tilting of the FPGA) for visual debug purposes.

# Sending the FPGA data 
Both the accelerometer filtered data and the button trigger boolean value are transmitted to the PC client using a python script running on the PC upon request. The python code interfaces with the nios processor via the JTAG-UART port. When necessary, it sends the string "Plotting" to the NIOS terminal to activate plotting mode. Meanwhile, the processor is equipped with an interrupt linked to the UART port, which activates whenever a string is received in the buffer sent from Python code. Upon activation and transition to plotting mode, the NIOS2 processor transmits a string of accelerometer data, appended with a space and the boolean value indicating whether the button is pressed, to the Python code, again received through the JTAG/UART interface. Subsequently, this client python code does the process of the received information from FPGA, unpacks it from the string, and repackages it for transmission to the server using the Python struct library. 

## Backend system of information transportation
# Overview of the server on AWS Cloud 
The back-end system, consisting of one server hosted on the Amazon Elastic Compute Cloud instances (EC2), is written in python to transfer information across multiple clients. It uses socket and threading libraries to manage TCP requests from multiple clients and exchange data efficiently. The server simultaneously communicates with four clients: two FPGA PC clients and two Unity clients. Additionally, it interacts with the Amazon DynamoDB database to access and update information as needed. To ensure smooth operation and minimise packet loss and traffic congestion, the system opens four main threads, each operating on a unique port, creating its individual TCP welcoming sockets, binding and listening to its corresponding client. The server also utilises global variables that serve as a centralised mechanism for exchanging and synchronising important data such as player position and button presses between different threads. Having multiple threads operating on unique ports also allow for easier debugging as different types of calls to the server are assigned a specific port and nodes causing issues during testing can be isolated.

# Interaction between the server with its FPGA clients
This game uses two FPGA nodes on two different computers with local python code (fpga.py) that establish connections to the corresponding ports on the server and transmit the required information as needed. The python struct library is used to pack the data into the smallest possible size in byte format to be transmitted over TCP for lower bandwidth usage and faster transmission speed. By utilising this library, both the accelerometer value and the button press indicator value is sent in one packet instead of two to eliminate redundancy of transmitting multiple separate packets, hence increasing its efficiency. 

#  Interaction between the server and Unity game client 
Similarly, one port is used specifically for communication between the Unity clients and the central server. A single main thread is employed and binded to the port and acts as a reception of TCP requests from different players on Unity. Due to the nature of this game, which allows two players to participate simultaneously from different locations and devices, this main thread is crucial for managing concurrent communication streams. Upon successful connection to the server, each player’s Unity client is assigned a unique connection socket and a new thread will be initialised, allowing for simultaneous handling of game clients. Through this connection, individual clients then receive updates regarding their gameplay status, including button press indications, cursor movement updates and score changes during the game. After the game ends, the Unity client sockets will be closed and the server will be ready to accept new connections again for a new game.

# Using the AWS DynamoDB
The system stores music note information and user scores in AWS DynamoDB. Musical note information for the three songs are initially generated as MIDI files through a digital music editor and converted into JSON string using a python script. The JSON strings are loaded into the database at server initialisation. They are retrieved by the Unity client after song selection via TCP . After the game ends, each player’s username and score will be saved to the score database for that particular song, and the top five player’s name and scores will be retrieved and sent to the game to display the leaderboard.

## Game Design

![starting](https://github.com/chinjyanson/ICL-infoproc-bad_trombone_game/blob/main/images/starting.png | width=100 )
![sysarch](https://github.com/chinjyanson/ICL-infoproc-bad_trombone_game/blob/main/images/sysarch.jpg)
![sysarch](https://github.com/chinjyanson/ICL-infoproc-bad_trombone_game/blob/main/images/sysarch.jpg)
![sysarch](https://github.com/chinjyanson/ICL-infoproc-bad_trombone_game/blob/main/images/sysarch.jpg)

