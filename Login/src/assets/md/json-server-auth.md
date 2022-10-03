# A tener en cuenta
## Flujo de autorización
> JSON Server Auth proporciona protecciones genéricas como middleware de ruta
> Los casos comunes se inspira en Unix, pero en especialmente en la notación numérica
- Se agregan 4 para permisos de lectura 
- Se agregan 2 para permisos de escritura

---- Ruta -------- Permisos de recursos 
* El usuario público (no registrado) realiza las siguientes solicitudes:
--- GET /664/posts | 200 OK
--- POST /664/posts  |
    {text: 'blabla'} | 401 UNAUTHORIZED

* El usuario que ha iniciado sesión id: 1hace las siguientes solicitudes:
--- GET /600/users/1                  |
    Authorization: Bearer xxx.xxx.xxx | 200 OK
--- GET /600/users/23                 |
    Authorization: Bearer xxx.xxx.xxx | 403 FORBIDDEN