--
-- PostgreSQL database dump
--

\restrict f3Lg0xJJjLHoLdny407Gl6Q7M4YMzWYPOIpay9aBu4mvgEU7IU2HUzwIVvUtzy7

-- Dumped from database version 16.14 (Ubuntu 16.14-0ubuntu0.24.04.1)
-- Dumped by pg_dump version 16.14 (Ubuntu 16.14-0ubuntu0.24.04.1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: SmtpOptions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."SmtpOptions" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Host" text NOT NULL,
    "Port" integer NOT NULL,
    "RealEmail" text NOT NULL,
    "Password" text NOT NULL
);


ALTER TABLE public."SmtpOptions" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Name: SmtpOptions PK_SmtpOptions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SmtpOptions"
    ADD CONSTRAINT "PK_SmtpOptions" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- PostgreSQL database dump complete
--

\unrestrict f3Lg0xJJjLHoLdny407Gl6Q7M4YMzWYPOIpay9aBu4mvgEU7IU2HUzwIVvUtzy7

