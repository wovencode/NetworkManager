#pragma once

#include "platform.h"

#if PLATFORM_WIN
#   include <winsock2.h>
#   include <ws2tcpip.h>
#   pragma comment(lib, "ws2_32.lib")
#else
#   include <errno.h>
#   include <netdb.h>
#   include <netinet/tcp.h>
#   define WSAEWOULDBLOCK EAGAIN
#   define WSAECONNRESET ECONNRESET
#   define SOCKET_ERROR -1
#endif

#if PLATFORM_ANDROID
#include <netinet/in.h>
#endif

#define SUCCESS 0

#ifdef __cplusplus
extern "C" {
#endif

typedef struct
{
    union
    {
        struct sockaddr     addr;
        struct sockaddr_in  addr_in;
        struct sockaddr_in6 addr_in6;
    };
    int length;
} network_address;

// initialize & terminate //////////////////////////////////////////////////////
EXPORT_API int32_t network_initialize();
EXPORT_API int32_t network_terminate();

// create & close //////////////////////////////////////////////////////////////
EXPORT_API int32_t network_create_socket(int64_t* sock, network_address* address, int32_t* error);
EXPORT_API int32_t network_close(int64_t sock, int32_t* error);

// configuration ///////////////////////////////////////////////////////////////
EXPORT_API int32_t network_set_dualmode(int64_t sock, int enabled, int32_t* error);
EXPORT_API int32_t network_set_nonblocking(int64_t sock, int32_t* error);
EXPORT_API int32_t network_set_send_buffer_size(int64_t sock, int size, int32_t* error);
EXPORT_API int32_t network_set_receive_buffer_size(int64_t sock, int size, int32_t* error);
EXPORT_API int32_t network_set_nodelay(int64_t sock, int enabled, int32_t* error);
EXPORT_API int32_t network_set_keepalive(int64_t sock, int enabled, int32_t* error);
EXPORT_API int32_t network_get_send_buffer_size(int64_t sock, int* size, int32_t* error);
EXPORT_API int32_t network_get_receive_buffer_size(int64_t sock, int* size, int32_t* error);
EXPORT_API int32_t network_get_error(int64_t sock);

// client //////////////////////////////////////////////////////////////////////
EXPORT_API int32_t network_connect(int64_t sock, network_address* address, int32_t* error);

// server //////////////////////////////////////////////////////////////////////
EXPORT_API int32_t network_bind(int64_t sock, network_address* address, int32_t* error);
EXPORT_API int32_t network_listen(int64_t sock, int32_t* error);
EXPORT_API int32_t network_accept(int64_t sock, int64_t* client_sock, network_address* client_address, int32_t* error);
EXPORT_API int32_t network_get_peer_address(int64_t sock, network_address* address, int32_t* error);

// state ///////////////////////////////////////////////////////////////////////
EXPORT_API int32_t network_disconnected(int64_t sock);

// recv & send /////////////////////////////////////////////////////////////////
EXPORT_API int32_t network_available(int64_t sock, int32_t* error);
EXPORT_API int32_t network_recv(int64_t sock, void* buffer, int32_t len, int32_t* error);
EXPORT_API int32_t network_send(int64_t sock, void* buffer, int32_t len, int32_t* error);

#ifdef __cplusplus
}
#endif
