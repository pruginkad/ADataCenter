-- Table: public.incidents

-- DROP TABLE public.incidents;

CREATE TABLE public.incidents
(
    name character varying COLLATE pg_catalog."default",
    action character varying COLLATE pg_catalog."default",
    objid character varying COLLATE pg_catalog."default",
    objtype character varying COLLATE pg_catalog."default",
    "timestamp" timestamp without time zone,
    id uuid NOT NULL,
    user_id character varying COLLATE pg_catalog."default"
)

TABLESPACE pg_default;

ALTER TABLE public.incidents
    OWNER to postgres;