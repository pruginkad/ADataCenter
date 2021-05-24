-- Database: incidentdb

-- DROP DATABASE incidentdb;

CREATE DATABASE incidentdb
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Russian_Russia.1251'
    LC_CTYPE = 'Russian_Russia.1251'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

COMMENT ON DATABASE incidentdb
    IS 'incidentdb';
-------------------------------------------------------------------------------------------------------------------------
    -- Table: public.incidents

-- DROP TABLE public.incidents;

CREATE TABLE public.incidents
(
    name character varying COLLATE pg_catalog."default",
    action character varying COLLATE pg_catalog."default",
    objid character varying COLLATE pg_catalog."default",
    objtype character varying COLLATE pg_catalog."default",
    "timestamp" timestamp with time zone,
    id uuid NOT NULL,
    src_id uuid NOT NULL,
    user_id character varying COLLATE pg_catalog."default"
)

TABLESPACE pg_default;

ALTER TABLE public.incidents
    OWNER to postgres;


-----------------------------------------------------------------------------------------------------------------------------------

-- Table: public.incident_handling

-- DROP TABLE public.incident_handling;

CREATE TABLE public.incident_handling
(
    line_descr character varying COLLATE pg_catalog."default",
    line_action character varying COLLATE pg_catalog."default",
    incident_id uuid,
    id uuid NOT NULL,
    line_timestamp timestamp without time zone,
    image_path character varying COLLATE pg_catalog."default"
)

TABLESPACE pg_default;

ALTER TABLE public.incident_handling
    OWNER to postgres;