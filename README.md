## Project Information

This project is a group coursework done in Year 2 Imperial EIE for Information Processing. We created a Unity game, that uses the accelerometer of the FPGA to play the game. Unfortunately the game will not work without 2 players and without FPGA's. To make up for that, you'll find below some images of our Project in action!


## Introduction and Purpose

Bad Trombone is a two player music rhythm game where players use FPGA to control visual cursors to match the rhythm bars of the song, and play an equivalent trombone sound as the bars are hit. The two players are able to play by looking at their own screen at different locations, choose between 3 music to play from together to compete with each other, and finally their player names and scores will be shown on a leaderboard for that song. The game is developed mainly using C# in Unity for the game client, Python for server scripts and C in Quartus for utilising the accelerometer sensor as well as detecting a button press on the FPGA board. AWS DynamoDB is utilised to store the leaderboard information for each song.

## System Architecture:

![sysarch](https://github.com/chinjyanson/ICL-infoproc-bad_trombone_game/images/image.jpg?raw=true)

Above is an overall flowchart of the performance view and a diagram for the system design architecture of Bad Trombone, which can be broken down into the following 3 contributing sections:
FPGA controller
Back-end system of information transportation 
Game design using Unity 


