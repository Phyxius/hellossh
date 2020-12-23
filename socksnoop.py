#!/usr/bin/env python3
import socket
import sys
import os


while True:
    server = socket.socket(socket.AF_UNIX, socket.SOCK_STREAM)
    client = socket.socket(socket.AF_UNIX, socket.SOCK_STREAM)
    client_address = sys.argv[1]
    server_address = sys.argv[2]
    client.connect(client_address)
    os.unlink(server_address)
    server.bind(server_address)
    server.listen(1)
    connection, _ = server.accept()
    data = connection.recv(4096)
    print(">", data.hex())
    client.sendall(data)
    data = client.recv(4096)
    print("<", data.hex())
    connection.sendall(data)
    connection.close()
    client.close()
