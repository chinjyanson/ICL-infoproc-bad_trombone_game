import socket
import struct
import threading
import time
import boto3
import json
from boto3.dynamodb.conditions import Key

print("We're in tcp server...")
server_ports = [11000, 12000, 13000, 14000]
filtercoefficients = [0.2,0.2,0.2,0.2,0.2]
FILTER_SIZE = 5
AVAL_UPPER_BOUND = 150
AVAL_LOWER_BOUND = -150
POS_UPPER_BOUND = 512
POS_LOWER_BOUND = 0
MAX_SPEED = 20

PLAYER_STATUS = [[0,0],[0,0]]
PLAYER1_NAME = "nil"
PLAYER2_NAME = "nil"
PLAYER1_EXIST = False
PLAYER1_SONG = "nil"
PLAYER2_SONG = "nil"
PLAYER1_RECENT_SCORE = 0
PLAYER2_RECENT_SCORE = 0

def scan_game(dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    try:
        table = dynamodb.Table('game')
        response = table.scan()
        return response['Items']
    except Exception as e:
        print("Error in scanning game:", e)
        return []

def put_game(song, username, score, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    try:
        table = dynamodb.Table('game')
        response = table.put_item(
           Item={
                'song': song,
                'username': username,
                'score': score
            }
        )
        return response
    except Exception as e:
        print("Error in putting game data:", e)
        return None

def query_game(song, username, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    try:
        table = dynamodb.Table('game')
        response = table.query(
            KeyConditionExpression=Key('song').eq(song) & Key('username').eq(username)
        )
        return response['Items']
    except Exception as e:
        print("Error in querying game data:", e)
        return []

def query_music_notes(song_name, artist_name, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')
    
    try:
        table = dynamodb.Table('Music')
        response = table.query(
            KeyConditionExpression=Key('name').eq(song_name) & Key('author').eq(artist_name)
        )
        if response['Items']:
            return response['Items']
        else:
            return []
    except Exception as e:
        print("Error in querying music notes:", e)
        return []

def update_pos(id,aval):
    if aval>AVAL_UPPER_BOUND: aval = AVAL_UPPER_BOUND
    if aval<AVAL_LOWER_BOUND: aval = AVAL_LOWER_BOUND
    aval = ((aval + 150) / 300) * 512
    diff = int(aval - PLAYER_STATUS[id][1])
    if abs(diff)>MAX_SPEED:
        diff = int(diff * 0.6)
    PLAYER_STATUS[id][1] += diff

def receive_controller(id, port):
    welcome_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    welcome_socket.bind(('0.0.0.0',port))
    welcome_socket.listen(1)
    print('Server running on port ', port)
    connection_socket, caddr = welcome_socket.accept()
    print('Connected to: ', port)
    mem = [None] * FILTER_SIZE
    count = 0
    
    while True:
        byte_data = connection_socket.recv(8)
        data = struct.unpack("2i",byte_data)
        PLAYER_STATUS[id][0] = data[0]
        aval = data[1]
        if count >= FILTER_SIZE: 
            count = 0
        mem[count] = aval
        count = count + 1
        
        filtered_aval = sum(x * y for x, y in zip(mem, filtercoefficients) if x is not None)
        
        update_pos(id,filtered_aval)
        
        #print("Controller " + str(id)+ " Position:",PLAYER_STATUS[id][1],"Button Status: ", PLAYER_STATUS[id][0])

def send_client_thread(connection_socket):
    global PLAYER1_RECENT_SCORE, PLAYER2_RECENT_SCORE
    initial_data = connection_socket.recv(1024)
    player = int(initial_data.decode())
    if player == 1:
        PLAYER1_RECENT_SCORE = 0
    elif player == 2:
        PLAYER2_RECENT_SCORE = 0
    initial_data = "recieved"
    connection_socket.send(initial_data.encode())
    while True:
        ping = connection_socket.recv(1024)
        ping_data = ping.decode()
        if(ping_data=="end"):
            break
        if player == 1:
            PLAYER1_RECENT_SCORE = int(ping_data)
            data = struct.pack("<?h?hh",PLAYER_STATUS[0][0],PLAYER_STATUS[0][1],PLAYER_STATUS[1][0],PLAYER_STATUS[1][1], PLAYER2_RECENT_SCORE)
        elif player == 2:
            PLAYER2_RECENT_SCORE = int(ping_data)
            data = struct.pack("<?h?hh",PLAYER_STATUS[0][0],PLAYER_STATUS[0][1],PLAYER_STATUS[1][0],PLAYER_STATUS[1][1], PLAYER1_RECENT_SCORE)
        connection_socket.send(data)
    print('Disconnected')
    connection_socket.close()
    return

def send_client(port):
    welcome_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    welcome_socket.bind(('0.0.0.0',port))
    welcome_socket.listen(1)
    print('Server running on port ', port)
    threads = []
    while True:
        connection_socket, caddr = welcome_socket.accept()
        threads.append(threading.Thread(target=send_client_thread, args = (connection_socket,)))
        threads[-1].start()
        print('Connected to: ', port)
    
    #print(PLAYER_STATUS[0][0], PLAYER_STATUS[0][1], PLAYER_STATUS[1][0], PLAYER_STATUS[1][1])

def update_database(connection_socket):
    global PLAYER1_EXIST, PLAYER1_NAME, PLAYER2_NAME, PLAYER1_SONG, PLAYER2_SONG, PLAYER1_RECENT_SCORE, PLAYER2_RECENT_SCORE
    cmsg = connection_socket.recv(4096)
    request_data = json.loads(cmsg)
    if request_data['type'] == "playerselect":
        
        if PLAYER1_EXIST == False:
            PLAYER1_NAME = request_data['username']
            response_msg = "player1"
            connection_socket.send(response_msg.encode())
            PLAYER1_EXIST = True
            print("Player1 name is: " + PLAYER1_NAME)
            connection_socket.close()
            return
        else:
            PLAYER2_NAME = request_data['username']
            response_msg = "player2"
            connection_socket.send(response_msg.encode())
            PLAYER1_EXIST = False
            print("Player2 name is: " + PLAYER2_NAME)
            connection_socket.close()
            return           
    
    elif request_data['type'] == 'menu':
        print("info from " + str(request_data['player']))
        while True:
            response_data = [PLAYER_STATUS[0][0], PLAYER_STATUS[1][0]]
            response_msg = json.dumps(response_data)
            connection_socket.send(response_msg.encode())
            cmsg = connection_socket.recv(4096)
            if request_data['player'] == 1:
                response_data = [PLAYER2_SONG, PLAYER2_NAME]
            else:
                response_data = [PLAYER1_SONG, PLAYER1_NAME]
            response_msg = json.dumps(response_data)
            connection_socket.send(response_msg.encode())
            cmsg = connection_socket.recv(4096)
            if cmsg.decode() == "start":
                break
        print("Game Start")
        connection_socket.close()
        return
            
    elif request_data['type'] == 'game':
        if request_data['playerno'] == 1:
            response_data = []
            put_response = put_game(request_data['song'], request_data['username1'], (request_data['score1']))
            PLAYER1_RECENT_SCORE = request_data['score1']
            print("Recieved Player 1 Score:" + PLAYER1_RECENT_SCORE)
        elif request_data['playerno'] == 2:
            put_response = put_game(request_data['song'], request_data['username2'], (request_data['score2']))
            PLAYER2_RECENT_SCORE = request_data['score2']
            print("Recieved Player 2 Score:" + PLAYER2_RECENT_SCORE)            
        time.sleep(2)
        scores = scan_game()
        gamelist = []
        for score in scores:
            gamelist.append([score['username'], int(score['score'])])
        sorted_game = sorted(gamelist, key=lambda x: x[1], reverse=True)
        winner = sorted_game[:5]
        userlist1 = []
        userlist2 = []
        user_scores = query_game(request_data['song'], request_data['username1'])
        for user_score in user_scores:
            userlist1.append(int(user_score['score']))
        userlist1.sort(reverse=True)
        sorted_user_game1 = [userlist1[0]]
        user_scores = query_game(request_data['song'], request_data['username2'])
        for user_score in user_scores:
            userlist2.append(int(user_score['score']))
        userlist2.sort(reverse=True)
        sorted_user_game2 = [userlist2[0]]
        response_data = [winner, sorted_user_game1,sorted_user_game2, [PLAYER1_RECENT_SCORE], [PLAYER2_RECENT_SCORE]]
        print(response_data)


    elif request_data['type'] == 'music':
        song_name = request_data['name']
        artist_name = request_data['author']
        notes = query_music_notes(song_name, artist_name)
        if notes:
            response_data = notes
        else:
            response_data = ["Error in querying music notes"]
        if request_data['player'] == 1:
            PLAYER1_SONG = request_data['name']
            print("Player 1 chose " + PLAYER1_SONG)
        elif request_data['player'] == 2:
            PLAYER2_SONG = request_data['name']
            print("Player 2 chose " + PLAYER2_SONG)
    response_msg = json.dumps(response_data)
    length = str(len(response_msg))
    connection_socket.send(length.encode())
    cmsg = connection_socket.recv(4096)
    connection_socket.send(response_msg.encode())
    time.sleep(5)
    cmsg = connection_socket.recv(4096)
    return
    

def access_database(port):
    welcome_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    welcome_socket.bind(('0.0.0.0',port))
    welcome_socket.listen(1)
    print('Server running on port ', port)
    threads = []
    while True:
        connection_socket, caddr = welcome_socket.accept()
        threads.append(threading.Thread(target=update_database, args = (connection_socket,)))
        threads[-1].start()
        print('Connected to: ', port)
        
        
        

t1 = threading.Thread(target=receive_controller, args=(0,server_ports[0]))
t2 = threading.Thread(target=receive_controller, args=(1,server_ports[1]))
t3 = threading.Thread(target=send_client, args=(server_ports[2],))
t4 = threading.Thread(target=access_database, args=(server_ports[3],))

t1.start()
t2.start()
t3.start()
t4.start()

    