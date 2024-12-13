# Converter-Server
API on C# with ASP.NET and PostgreSQL for web image converter

SQL script to create tables:
```sql
create table public.sessions
(
    sessionid serial
        primary key,
    userid    integer,
    datetime  timestamp with time zone,
    amount    integer,
    active    boolean
);

alter table public.sessions
    owner to aboba;

create table public.users
(
    id       serial
        primary key,
    login    varchar(80),
    password varchar(80),
    premium  boolean,
    salt     varchar(40)
);

alter table public.users
    owner to aboba;


```

API link:
https://deciding-logically-piglet.ngrok-free.app
