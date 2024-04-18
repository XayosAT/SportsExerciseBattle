-- Table: public.users

-- DROP TABLE IF EXISTS public.users;

CREATE TABLE IF NOT EXISTS public.users
(
    username character varying COLLATE pg_catalog."default" NOT NULL,
    password character varying COLLATE pg_catalog."default" NOT NULL,
    elo integer NOT NULL DEFAULT 100,
    bio character varying COLLATE pg_catalog."default",
    image character varying COLLATE pg_catalog."default",
    name character varying COLLATE pg_catalog."default",
    CONSTRAINT users_pkey PRIMARY KEY (username)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.users
    OWNER to postgres;