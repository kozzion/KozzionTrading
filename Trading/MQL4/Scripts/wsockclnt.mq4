#include <winsock.mqh>

#import "user32.dll"
bool GetAsyncKeyState(int Key);
#import
 
#define DEFAULT_PORT 2007
#property show_inputs
extern string protocol="TCP";
extern int port=DEFAULT_PORT;
extern string server_addr="127.0.0.1";
extern int maxloop=-1;


bool isalpha(int c) {
  string t="0123456789";
  return(StringFind(t,CharToStr(c))<0);
  
}

void Usage() {
    Print("\t- The defaults are TCP, 2007, localhost");
    Print("\t- Hit ESC to terminate client script...");
    Print("\t- maxloop is the number of loops to execute 0=endless.");
    Print("\t- server_ip is the IP address of server");
    Print("\t- port is the port to listen on");
    Print("Where:\n\t- protocol is one of TCP or UDP");
    Print("Usage: protocol port server iterations");
}
int conn_socket=0;
int  start() {
    int Buffer[32];
    int retval;
    int i, loopcount=0;
    int addr[1];
    int socket_type;
    int server[sockaddr_in];
    int hp;
    int wsaData[WSADATA];
 
    if (protocol=="TCP")
    {
      Alert("TCP");
      socket_type = SOCK_STREAM;    
    }
    else if (protocol=="UDP")
    {
      socket_type = SOCK_DGRAM;
    }
    else {
      Usage();
      return(-1);
    }   
     
    if (port==0) 
    {
  
       Usage();
       return(-1);
    }   
    Alert("WSAStartup");
    retval = WSAStartup(0x202, wsaData);
    Alert("WSAStartup Done");
    
    if (retval != 0) 
    {
       Alert("Client: WSAStartup() failed with error "+ retval);
       return(-1);
    }
    else
    {
       Alert("Client: WSAStartup() is OK.");
    }
 
    if (isalpha(StringGetChar(server_addr,0))){
        Print("Client: IP address should be given in numeric form in this version.");
        return(-1);
    } else { 
        addr[0] = inet_addr(server_addr); 
        hp = gethostbyaddr(server_addr, 4, AF_INET);
        Print("server addr:"+addr[0]+" hp"+hp);
    }
    if (hp == 0 ) {
       Print("Client: Cannot resolve address \""+server_addr+"\": Error :"+WSAGetLastError());
       return(-1);
    }
    
    int2struct(server,sin_addr,addr[0]);
    int2struct(server,sin_family,AF_INET);
    int2struct(server,sin_port,htons(port));
 
    conn_socket = socket(AF_INET, socket_type, 0); 
    if (conn_socket <0 ) {
        Print("Client: Error Opening socket: Error "+ WSAGetLastError());
        return(-1);
    } else
       Print("Client: socket() is OK.");
 
    Print("Client: Client connecting to: "+ server_addr);
    retval=connect(conn_socket, server, ArraySize(server)<<2);
    if (retval == SOCKET_ERROR) {
        Print("Client: connect() failed: ", WSAGetLastError());
        return(-1);
    }
    else
       Print("Client: connect() is OK.");
 
    while(!IsStopped()) {
      str2struct(Buffer,ArraySize(Buffer)<<18,"This is a test message from client "+loopcount);
      retval = send(conn_socket, Buffer, ArraySize(Buffer)<<2, 0);
      if (retval == SOCKET_ERROR) {
         Print("Client: send() failed: error ", WSAGetLastError());
         return(-1);
      } else
          Print("Client: send() is OK.");
      Print("Client: Sent data "+ struct2str(Buffer,ArraySize(Buffer)<<18));
 
      retval = recv(conn_socket, Buffer, ArraySize(Buffer)<<2, 0);
      if (retval == SOCKET_ERROR) {
         Print("Client: recv() failed: error ", WSAGetLastError());
         return(-1);
      } else
        Print("Client: recv() is OK.");
        
      if (retval == 0) {
        Print("Client: Server closed connection.");
        return(-1);
      }
 
      Print("Client: Received "+retval+" bytes, data \""+struct2str(Buffer,ArraySize(Buffer)<<18)+"\" from server.", retval, Buffer[0]);
      loopcount++;
      if (maxloop<0) break;
      if ((loopcount >= maxloop) && (maxloop >0))
         break;
      Sleep(1100);
      if(GetAsyncKeyState(27)) break;
    }
    return(0);
}

int deinit() {
  if (conn_socket>0) closesocket(conn_socket);
  WSACleanup();
}

