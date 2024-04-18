-- Table: public.push_up_records

-- DROP TABLE IF EXISTS public.push_up_records;

CREATE TABLE IF NOT EXISTS public.push_up_records
(
    record_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    fk_user_id character varying COLLATE pg_catalog."default" NOT NULL,
    count integer NOT NULL,
    duration integer NOT NULL,
    date_time timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT push_up_records FOREIGN KEY (fk_user_id)
        REFERENCES public.users (username) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.push_up_records
    OWNER to postgres;