-- Table: public.incident_handling

-- DROP TABLE public.incident_handling;

CREATE TABLE public.incident_handling
(
    line_descr character varying COLLATE pg_catalog."default",
    line_action character varying COLLATE pg_catalog."default",
    incident_id uuid,
    id uuid NOT NULL,
    line_timestamp timestamp with time zone,
    image_path character varying COLLATE pg_catalog."default"
)

TABLESPACE pg_default;

ALTER TABLE public.incident_handling
    OWNER to postgres;