import subprocess
import socket
import matplotlib.pyplot as plt 
import struct
import time

NIOS_CMD_SHELL_BAT = "D:/QuartusPrime18.1/nios2eds/Nios II Command Shell.bat"

def send_on_jtag(client_socket):
    # check if atleast one character is being sent down
    # assert (len(cmd) >= 1), "Please make the cmd a single character"

    # create a subprocess which will run the nios2-terminal
    process = subprocess.Popen(
        NIOS_CMD_SHELL_BAT,
        bufsize=0,
        stdin=subprocess.PIPE,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        universal_newlines=True
    )

    # send the cmd string to the nios2-terminal, read the output and terminate the process
    process.stdin.write("nios2-terminal <<< Plotting\n")
    process.stdin.flush()

    cnt = 50
    try:
        while True:
            cnt = cnt - 1
            output_line = process.stdout.readline()
            if(cnt == 0):
                cnt = 50
                try:
                    button = int(output_line[0])
                    value = int(output_line[1:])
                    data = struct.pack("2i",button,value)
                    client_socket.send(data)
                    
                except ValueError:
                    print("eee")
                    continue
                
                if process.poll() is not None:
                    break

    except KeyboardInterrupt:
        pass
    
    finally:
        process.terminate() 


print("We're in tcp client...");
#the server name and port client wishes to access
server_name = '13.43.110.192'
#'52.205.252.164'
server_port = 11000

def main():
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client_socket.connect((server_name, server_port))
    send_on_jtag(client_socket)

if __name__ == "__main__":
    main()