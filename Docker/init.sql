--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4 (Debian 17.4-1.pgdg120+2)
-- Dumped by pg_dump version 17.4

-- Started on 2025-06-13 19:22:07

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 873 (class 1247 OID 16386)
-- Name: rolatyp; Type: TYPE; Schema: public; Owner: admin
--

CREATE TYPE public.rolatyp AS ENUM (
    'Administrator',
    'Uzytkownik'
);


ALTER TYPE public.rolatyp OWNER TO admin;

--
-- TOC entry 876 (class 1247 OID 16392)
-- Name: statustyp; Type: TYPE; Schema: public; Owner: admin
--

CREATE TYPE public.statustyp AS ENUM (
    'Sukces',
    'Niepowodzenie'
);


ALTER TYPE public.statustyp OWNER TO admin;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 228 (class 1259 OID 16469)
-- Name: Silnik; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."Silnik" (
    "ID" integer NOT NULL,
    "PojazdID" integer,
    "Pojemnosc" character varying(10),
    "Typ" character varying(20),
    "Moc" smallint,
    "Nazwa" character varying(255),
    "MomentObrotowy" integer,
    "RodzajPaliwa" character varying(50),
    "Cylindry" integer,
    "Skrzynia" character varying(50),
    "Biegi" integer,
    "NapedzType" character varying(50),
    "ZuzyciePaliva" character varying(10),
    "EmisjaCO2" integer,
    "Opis" text,
    "JestAktywny" boolean DEFAULT true
);


ALTER TABLE public."Silnik" OWNER TO admin;

--
-- TOC entry 227 (class 1259 OID 16468)
-- Name: Silnik_ID_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public."Silnik_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Silnik_ID_seq" OWNER TO admin;

--
-- TOC entry 3552 (class 0 OID 0)
-- Dependencies: 227
-- Name: Silnik_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public."Silnik_ID_seq" OWNED BY public."Silnik"."ID";


--
-- TOC entry 244 (class 1259 OID 16583)
-- Name: administrator; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.administrator (
    id integer NOT NULL,
    iduzytkownika integer,
    poziomuprawnien character varying(30),
    ostatnielogowanie timestamp without time zone
);


ALTER TABLE public.administrator OWNER TO admin;

--
-- TOC entry 243 (class 1259 OID 16582)
-- Name: administrator_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.administrator_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.administrator_id_seq OWNER TO admin;

--
-- TOC entry 3553 (class 0 OID 0)
-- Dependencies: 243
-- Name: administrator_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.administrator_id_seq OWNED BY public.administrator.id;


--
-- TOC entry 226 (class 1259 OID 16461)
-- Name: car_accessories; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.car_accessories (
    id character varying(255) NOT NULL,
    car_id character varying(255) NOT NULL,
    car_model character varying(255) NOT NULL,
    category character varying(50) NOT NULL,
    type character varying(50) NOT NULL,
    name character varying(255) NOT NULL,
    description text,
    price numeric(10,2) NOT NULL,
    manufacturer character varying(255),
    part_number character varying(100),
    is_original_bmw_part boolean NOT NULL,
    is_in_stock boolean NOT NULL,
    stock_quantity integer NOT NULL,
    image_url text,
    size character varying(100),
    pattern character varying(100),
    color character varying(100),
    material character varying(100),
    capacity integer,
    compatibility character varying(255),
    age_group character varying(50),
    max_load integer,
    is_universal boolean NOT NULL,
    installation_difficulty character varying(50),
    warranty character varying(100)
);


ALTER TABLE public.car_accessories OWNER TO admin;

--
-- TOC entry 225 (class 1259 OID 16451)
-- Name: car_interior_equipment; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.car_interior_equipment (
    id character varying(255) NOT NULL,
    type character varying(50) NOT NULL,
    value character varying(255) NOT NULL,
    description text,
    additional_price numeric(10,2) DEFAULT 0 NOT NULL,
    has_navigation boolean DEFAULT false,
    has_premium_sound boolean DEFAULT false,
    control_type character varying(50)
);


ALTER TABLE public.car_interior_equipment OWNER TO admin;

--
-- TOC entry 223 (class 1259 OID 16428)
-- Name: car_model_colors; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.car_model_colors (
    "Id" character varying(255) NOT NULL,
    "CarModelId" character varying(255) NOT NULL,
    "ColorName" character varying(100) NOT NULL,
    "Price" integer DEFAULT 0 NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "car_model_colors_Price_check" CHECK ((("Price" >= 0) AND ("Price" <= 60000)))
);


ALTER TABLE public.car_model_colors OWNER TO admin;

--
-- TOC entry 232 (class 1259 OID 16502)
-- Name: cechypojazdu; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.cechypojazdu (
    id integer NOT NULL,
    idpojazdu integer,
    cecha character varying(100)
);


ALTER TABLE public.cechypojazdu OWNER TO admin;

--
-- TOC entry 231 (class 1259 OID 16501)
-- Name: cechypojazdu_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.cechypojazdu_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.cechypojazdu_id_seq OWNER TO admin;

--
-- TOC entry 3554 (class 0 OID 0)
-- Dependencies: 231
-- Name: cechypojazdu_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.cechypojazdu_id_seq OWNED BY public.cechypojazdu.id;


--
-- TOC entry 234 (class 1259 OID 16514)
-- Name: elementwyposazenia; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.elementwyposazenia (
    id integer NOT NULL,
    typelementu character varying(50),
    wartosc character varying(100),
    opiselementu text,
    cenadodatkowa numeric(8,2)
);


ALTER TABLE public.elementwyposazenia OWNER TO admin;

--
-- TOC entry 233 (class 1259 OID 16513)
-- Name: elementwyposazenia_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.elementwyposazenia_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.elementwyposazenia_id_seq OWNER TO admin;

--
-- TOC entry 3555 (class 0 OID 0)
-- Dependencies: 233
-- Name: elementwyposazenia_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.elementwyposazenia_id_seq OWNED BY public.elementwyposazenia.id;


--
-- TOC entry 236 (class 1259 OID 16523)
-- Name: konfiguracja; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.konfiguracja (
    id integer NOT NULL,
    iduzytkownika integer,
    idpojazdu integer,
    modelsilnikid integer,
    datautworzenia timestamp without time zone,
    nazwakonfiguracji character varying(100),
    opiskonfiguracji text
);


ALTER TABLE public.konfiguracja OWNER TO admin;

--
-- TOC entry 235 (class 1259 OID 16522)
-- Name: konfiguracja_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.konfiguracja_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.konfiguracja_id_seq OWNER TO admin;

--
-- TOC entry 3556 (class 0 OID 0)
-- Dependencies: 235
-- Name: konfiguracja_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.konfiguracja_id_seq OWNED BY public.konfiguracja.id;


--
-- TOC entry 240 (class 1259 OID 16559)
-- Name: logowanie; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.logowanie (
    id integer NOT NULL,
    iduzytkownika integer,
    datalogowania timestamp without time zone,
    adresip character varying(45),
    status public.statustyp
);


ALTER TABLE public.logowanie OWNER TO admin;

--
-- TOC entry 239 (class 1259 OID 16558)
-- Name: logowanie_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.logowanie_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.logowanie_id_seq OWNER TO admin;

--
-- TOC entry 3557 (class 0 OID 0)
-- Dependencies: 239
-- Name: logowanie_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.logowanie_id_seq OWNED BY public.logowanie.id;


--
-- TOC entry 230 (class 1259 OID 16484)
-- Name: modelsilnik; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.modelsilnik (
    id integer NOT NULL,
    modelid integer,
    silnikid integer,
    cenadodatkowa numeric(10,2) DEFAULT 0
);


ALTER TABLE public.modelsilnik OWNER TO admin;

--
-- TOC entry 229 (class 1259 OID 16483)
-- Name: modelsilnik_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.modelsilnik_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.modelsilnik_id_seq OWNER TO admin;

--
-- TOC entry 3558 (class 0 OID 0)
-- Dependencies: 229
-- Name: modelsilnik_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.modelsilnik_id_seq OWNED BY public.modelsilnik.id;


--
-- TOC entry 220 (class 1259 OID 16407)
-- Name: pojazd; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.pojazd (
    id integer NOT NULL,
    model character varying(50),
    kolornadwozia character varying(30),
    typnadwozia character varying(50),
    wyposazeniewnetrza text,
    cena integer,
    opis text,
    ma4x4 boolean,
    jestelektryczny boolean,
    akcesoria text,
    zdjecie bytea,
    imageurl character varying(255),
    isactive boolean DEFAULT true
);


ALTER TABLE public.pojazd OWNER TO admin;

--
-- TOC entry 219 (class 1259 OID 16406)
-- Name: pojazd_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.pojazd_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.pojazd_id_seq OWNER TO admin;

--
-- TOC entry 3559 (class 0 OID 0)
-- Dependencies: 219
-- Name: pojazd_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.pojazd_id_seq OWNED BY public.pojazd.id;


--
-- TOC entry 224 (class 1259 OID 16440)
-- Name: pojazd_zdjecie; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.pojazd_zdjecie (
    "Id" character varying(255) NOT NULL,
    "CarModelId" character varying(255) NOT NULL,
    "ImageUrl" character varying(500) NOT NULL,
    "Color" character varying(100) DEFAULT ''::character varying NOT NULL,
    "DisplayOrder" integer DEFAULT 1 NOT NULL,
    "IsMainImage" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public.pojazd_zdjecie OWNER TO admin;

--
-- TOC entry 242 (class 1259 OID 16571)
-- Name: rejestracja; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.rejestracja (
    id integer NOT NULL,
    iduzytkownika integer,
    datarejestracji timestamp without time zone,
    adresip character varying(45),
    status public.statustyp
);


ALTER TABLE public.rejestracja OWNER TO admin;

--
-- TOC entry 241 (class 1259 OID 16570)
-- Name: rejestracja_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.rejestracja_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.rejestracja_id_seq OWNER TO admin;

--
-- TOC entry 3560 (class 0 OID 0)
-- Dependencies: 241
-- Name: rejestracja_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.rejestracja_id_seq OWNED BY public.rejestracja.id;


--
-- TOC entry 238 (class 1259 OID 16547)
-- Name: udostepnieniekonfiguracji; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.udostepnieniekonfiguracji (
    id integer NOT NULL,
    idkonfiguracji integer,
    link character varying(255),
    odbiorca character varying(100),
    dataudostepnienia timestamp without time zone
);


ALTER TABLE public.udostepnieniekonfiguracji OWNER TO admin;

--
-- TOC entry 237 (class 1259 OID 16546)
-- Name: udostepnieniekonfiguracji_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.udostepnieniekonfiguracji_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.udostepnieniekonfiguracji_id_seq OWNER TO admin;

--
-- TOC entry 3561 (class 0 OID 0)
-- Dependencies: 237
-- Name: udostepnieniekonfiguracji_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.udostepnieniekonfiguracji_id_seq OWNED BY public.udostepnieniekonfiguracji.id;


--
-- TOC entry 222 (class 1259 OID 16417)
-- Name: user_configurations; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.user_configurations (
    id integer NOT NULL,
    user_id integer NOT NULL,
    configuration_name character varying(255) NOT NULL,
    car_model_id character varying(255),
    car_model_name character varying(255),
    engine_id character varying(255),
    engine_name character varying(255),
    exterior_color character varying(50),
    exterior_color_name character varying(100),
    selected_accessories text,
    selected_interior_equipment text,
    total_price numeric(12,2) DEFAULT 0 NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp with time zone,
    is_active boolean DEFAULT true
);


ALTER TABLE public.user_configurations OWNER TO admin;

--
-- TOC entry 221 (class 1259 OID 16416)
-- Name: user_configurations_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.user_configurations_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.user_configurations_id_seq OWNER TO admin;

--
-- TOC entry 3562 (class 0 OID 0)
-- Dependencies: 221
-- Name: user_configurations_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.user_configurations_id_seq OWNED BY public.user_configurations.id;


--
-- TOC entry 218 (class 1259 OID 16398)
-- Name: uzytkownik; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.uzytkownik (
    id integer NOT NULL,
    imienazwisko character varying(100),
    email character varying(100),
    haslo character varying(255),
    refreshtoken character varying(255),
    refreshtokenexpires timestamp without time zone,
    rola character varying(255)
);


ALTER TABLE public.uzytkownik OWNER TO admin;

--
-- TOC entry 217 (class 1259 OID 16397)
-- Name: uzytkownik_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE public.uzytkownik_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.uzytkownik_id_seq OWNER TO admin;

--
-- TOC entry 3563 (class 0 OID 0)
-- Dependencies: 217
-- Name: uzytkownik_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE public.uzytkownik_id_seq OWNED BY public.uzytkownik.id;


--
-- TOC entry 3303 (class 2604 OID 16472)
-- Name: Silnik ID; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."Silnik" ALTER COLUMN "ID" SET DEFAULT nextval('public."Silnik_ID_seq"'::regclass);


--
-- TOC entry 3313 (class 2604 OID 16586)
-- Name: administrator id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.administrator ALTER COLUMN id SET DEFAULT nextval('public.administrator_id_seq'::regclass);


--
-- TOC entry 3307 (class 2604 OID 16505)
-- Name: cechypojazdu id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.cechypojazdu ALTER COLUMN id SET DEFAULT nextval('public.cechypojazdu_id_seq'::regclass);


--
-- TOC entry 3308 (class 2604 OID 16517)
-- Name: elementwyposazenia id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.elementwyposazenia ALTER COLUMN id SET DEFAULT nextval('public.elementwyposazenia_id_seq'::regclass);


--
-- TOC entry 3309 (class 2604 OID 16526)
-- Name: konfiguracja id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.konfiguracja ALTER COLUMN id SET DEFAULT nextval('public.konfiguracja_id_seq'::regclass);


--
-- TOC entry 3311 (class 2604 OID 16562)
-- Name: logowanie id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.logowanie ALTER COLUMN id SET DEFAULT nextval('public.logowanie_id_seq'::regclass);


--
-- TOC entry 3305 (class 2604 OID 16487)
-- Name: modelsilnik id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.modelsilnik ALTER COLUMN id SET DEFAULT nextval('public.modelsilnik_id_seq'::regclass);


--
-- TOC entry 3288 (class 2604 OID 16410)
-- Name: pojazd id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.pojazd ALTER COLUMN id SET DEFAULT nextval('public.pojazd_id_seq'::regclass);


--
-- TOC entry 3312 (class 2604 OID 16574)
-- Name: rejestracja id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.rejestracja ALTER COLUMN id SET DEFAULT nextval('public.rejestracja_id_seq'::regclass);


--
-- TOC entry 3310 (class 2604 OID 16550)
-- Name: udostepnieniekonfiguracji id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.udostepnieniekonfiguracji ALTER COLUMN id SET DEFAULT nextval('public.udostepnieniekonfiguracji_id_seq'::regclass);


--
-- TOC entry 3290 (class 2604 OID 16420)
-- Name: user_configurations id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.user_configurations ALTER COLUMN id SET DEFAULT nextval('public.user_configurations_id_seq'::regclass);


--
-- TOC entry 3287 (class 2604 OID 16401)
-- Name: uzytkownik id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.uzytkownik ALTER COLUMN id SET DEFAULT nextval('public.uzytkownik_id_seq'::regclass);


--
-- TOC entry 3530 (class 0 OID 16469)
-- Dependencies: 228
-- Data for Name: Silnik; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."Silnik" ("ID", "PojazdID", "Pojemnosc", "Typ", "Moc", "Nazwa", "MomentObrotowy", "RodzajPaliwa", "Cylindry", "Skrzynia", "Biegi", "NapedzType", "ZuzyciePaliva", "EmisjaCO2", "Opis", "JestAktywny") FROM stdin;
1	1	2.0	L4	184	20i xDrive	300	Benzyna	4	Automatyczna	8	AWD	7.5	171	Podstawowy silnik benzynowy	t
2	1	3.0	L6	374	M40i xDrive	500	Benzyna	6	Automatyczna	8	AWD	9.0	204	Silnik wysokowydajny M Performance	t
3	2	2.0	L4	156	320i	250	Benzyna	4	Manualna	6	RWD	6.5	149	Ekonomiczny silnik benzynowy	t
4	2	2.0	L4	190	320d	400	Diesel	4	Automatyczna	8	RWD	4.5	118	Wydajny silnik diesla	t
5	3	\N	Elektryczny	340	eDrive40	430	Elektryczny	\N	Automatyczna	1	RWD	\N	0	Silnik elektryczny o dużej mocy	t
6	3	\N	Elektryczny	544	M50 xDrive	795	Elektryczny	\N	Automatyczna	1	AWD	\N	0	Podwójny silnik elektryczny M	t
7	4	3.0	L6	381	40i xDrive	520	Benzyna	6	Automatyczna	8	AWD	10.5	237	Mocny silnik 6-cylindrowy	t
8	5	3.0	L6	510	M Competition	650	Benzyna	6	Automatyczna	8	AWD	10.8	245	Silnik M TwinPower Turbo	t
\.


--
-- TOC entry 3546 (class 0 OID 16583)
-- Dependencies: 244
-- Data for Name: administrator; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.administrator (id, iduzytkownika, poziomuprawnien, ostatnielogowanie) FROM stdin;
\.


--
-- TOC entry 3528 (class 0 OID 16461)
-- Dependencies: 226
-- Data for Name: car_accessories; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.car_accessories (id, car_id, car_model, category, type, name, description, price, manufacturer, part_number, is_original_bmw_part, is_in_stock, stock_quantity, image_url, size, pattern, color, material, capacity, compatibility, age_group, max_load, is_universal, installation_difficulty, warranty) FROM stdin;
acc-001	1	BMW X3	Zewnętrzne	AlloyWheels	Felgi M Performance 20"	Lekkie felgi aluminiowe z technologią M	8000.00	BMW M	\N	t	t	50	\N	20	Y-spoke	Czarne	Aluminium kute	\N	X3 G01	\N	\N	f	Średnia	3 lata
acc-002	1	BMW X3	Zewnętrzne	AlloyWheels	Felgi sportowe 19"	Felgi z podwójnymi szprychami	6000.00	BMW	\N	t	t	30	\N	19	Double-spoke	Srebrne	Aluminium	\N	X3 G01	\N	\N	f	Średnia	3 lata
acc-003	1	BMW X3	Zewnętrzne	AlloyWheels	Felgi zimowe 18"	Dedykowane felgi na sezon zimowy	5000.00	BMW	\N	t	t	40	\N	18	V-spoke	Srebrne	Aluminium	\N	X3 G01	\N	\N	f	Średnia	3 lata
acc-025	2	BMW Series 3	Zewnętrzne	AlloyWheels	Felgi M Sport 19"	Sportowe felgi M z czarnymi akcentami	7000.00	BMW M	\N	t	t	25	\N	19	M Double-spoke	Bicolor	Aluminium kute	\N	Series 3 G20	\N	\N	f	Średnia	3 lata
acc-026	2	BMW Series 3	Zewnętrzne	AlloyWheels	Felgi Turbine 18"	Aerodynamiczne felgi turbinowe	5500.00	BMW	\N	t	t	35	\N	18	Turbine	Grafitowe	Aluminium	\N	Series 3 G20	\N	\N	f	Średnia	3 lata
acc-027	3	BMW i4	Zewnętrzne	AlloyWheels	Felgi Aero 20"	Aerodynamiczne felgi dla zwiększonego zasięgu	8500.00	BMW i	\N	t	t	20	\N	20	Aero	Srebrno-niebieskie	Aluminium lekkie	\N	i4 G26	\N	\N	f	Średnia	3 lata
acc-028	4	BMW X5	Zewnętrzne	AlloyWheels	Felgi X5 M 22"	Największe felgi M Performance	12000.00	BMW M	\N	t	t	15	\N	22	Star-spoke	Czarne matowe	Aluminium kute	\N	X5 G05	\N	\N	f	Trudna	3 lata
acc-029	5	BMW M3	Zewnętrzne	AlloyWheels	Felgi M Competition 20"	Ultralekkie felgi torowe	15000.00	BMW M	\N	t	t	10	\N	20	M Y-spoke	Złote	Kute aluminium	\N	M3 G80	\N	\N	f	Trudna	3 lata
acc-004	1	BMW X3	Wewnętrzne	FloorMats	Dywaniki welurowe M	Wysokiej jakości dywaniki z logo M	800.00	BMW M	\N	t	t	100	\N	\N	\N	Czarne	Welur premium	\N	X3 G01	\N	\N	f	Łatwa	2 lata
acc-005	1	BMW X3	Wewnętrzne	FloorMats	Dywaniki gumowe	Wodoodporne dywaniki całoroczne	400.00	BMW	\N	t	t	150	\N	\N	\N	Czarne	Guma	\N	X3 G01	\N	\N	f	Łatwa	2 lata
acc-030	2	BMW Series 3	Wewnętrzne	FloorMats	Dywaniki M Performance	Sportowe dywaniki z alcantary	900.00	BMW M	\N	t	t	80	\N	\N	\N	Czarne z czerwonymi przeszyciami	Alcantara	\N	Series 3 G20	\N	\N	f	Łatwa	2 lata
acc-031	4	BMW X5	Wewnętrzne	FloorMats	Dywaniki luksusowe	Dywaniki z długim włosiem	1200.00	BMW Individual	\N	t	t	50	\N	\N	\N	Beżowe	Welur długowłosy	\N	X5 G05	\N	\N	f	Łatwa	2 lata
acc-006	2	BMW Series 3	Zewnętrzne	Spoiler	Tylny spoiler M Performance	Aerodynamiczny spoiler z włókna węglowego	3500.00	BMW M	\N	t	t	20	\N	\N	\N	Carbon	Włókno węglowe	\N	Series 3 G20	\N	\N	f	Średnia	3 lata
acc-007	2	BMW Series 3	Zewnętrzne	Spoiler	Przedni splitter M	Przedni element aerodynamiczny	2800.00	BMW M	\N	t	t	25	\N	\N	\N	Carbon	Włókno węglowe	\N	Series 3 G20	\N	\N	f	Średnia	3 lata
acc-032	5	BMW M3	Zewnętrzne	Spoiler	Tylne skrzydło M Performance	Regulowane skrzydło torowe	8000.00	BMW M	\N	t	t	5	\N	\N	\N	Carbon	Włókno węglowe	\N	M3 G80	\N	\N	f	Trudna	3 lata
acc-008	3	BMW i4	Ładowanie	ChargingCable	Kabel ładowania Wallbox	Kabel do szybkiego ładowania 11kW	2000.00	BMW i	\N	t	t	40	\N	\N	\N	\N	\N	\N	i4, iX3, iX	\N	\N	f	Łatwa	2 lata
acc-009	3	BMW i4	Ładowanie	ChargingStation	BMW Wallbox Plus	Domowa stacja ładowania 22kW	5000.00	BMW i	\N	t	t	30	\N	\N	\N	\N	\N	22	Wszystkie modele elektryczne	\N	\N	f	Profesjonalna	5 lat
acc-033	3	BMW i4	Ładowanie	ChargingCable	Kabel Mode 3	Kabel do publicznych stacji ładowania	800.00	BMW i	\N	t	t	60	\N	\N	\N	\N	\N	\N	i4, iX3, iX	\N	\N	f	Łatwa	2 lata
acc-010	1	BMW X3	Transport	RoofBox	Box dachowy 420L	Aerodynamiczny box na narty	3000.00	BMW	\N	t	t	20	\N	\N	\N	Czarny	ABS	420	X3, X5, Series 3 Touring	\N	75	f	Średnia	3 lata
acc-011	1	BMW X3	Transport	BikeRack	Bagażnik rowerowy	Na 2 rowery, montaż na hak	1800.00	BMW	\N	t	t	35	\N	\N	\N	Czarny	Stal	2	Modele z hakiem	\N	60	f	Łatwa	3 lata
acc-034	4	BMW X5	Transport	RoofBox	Box dachowy 520L	Największy box w ofercie BMW	3800.00	BMW	\N	t	t	15	\N	\N	\N	Srebrny	ABS	520	X5, X7	\N	90	f	Średnia	3 lata
acc-012	1	BMW X3	Ochrona	MudFlaps	Chlapacze przednie i tylne	Ochrona przed kamieniami	350.00	BMW	\N	t	t	80	\N	\N	\N	Czarne	Guma	\N	X3 G01	\N	\N	f	Łatwa	2 lata
acc-013	2	BMW Series 3	Ochrona	PaintProtection	Folia ochronna PPF	Ochrona przedniego zderzaka	2500.00	BMW	\N	t	t	40	\N	\N	\N	Transparentna	Poliuretan	\N	Series 3 G20	\N	\N	f	Profesjonalna	5 lat
acc-035	5	BMW M3	Ochrona	PaintProtection	Pełna folia PPF	Ochrona całego nadwozia	15000.00	XPEL	\N	f	t	10	\N	\N	\N	Transparentna	Poliuretan samo-regenerujący	\N	M3 G80	\N	\N	f	Profesjonalna	10 lat
acc-014	1	BMW X3	Bezpieczeństwo	ChildSeat	Fotelik Junior Seat	Grupa 2/3, 15-36kg	1500.00	BMW	\N	t	t	50	\N	\N	\N	Czarno-szary	Tkanina	\N	Uniwersalny ISOFIX	3-12 lat	36	t	Łatwa	2 lata
acc-015	1	BMW X3	Bezpieczeństwo	ChildSeat	Fotelik Baby Seat	Grupa 0+, 0-13kg	1800.00	BMW	\N	t	t	40	\N	\N	\N	Czarny	Tkanina	\N	Uniwersalny ISOFIX	0-15 miesięcy	13	t	Łatwa	2 lata
acc-016	2	BMW Series 3	Komfort	SunShade	Rolety przeciwsłoneczne	Zestaw na tylne szyby	600.00	BMW	\N	t	t	70	\N	\N	\N	Czarne	Tkanina	\N	Series 3 G20	\N	\N	f	Łatwa	2 lata
acc-017	1	BMW X3	Komfort	CoolBox	Lodówka samochodowa	Pojemność 16L, zasilanie 12V	1200.00	BMW	\N	t	t	30	\N	\N	\N	Szara	Plastik	16	Uniwersalna 12V	\N	\N	t	Łatwa	2 lata
acc-018	2	BMW Series 3	Elektronika	DashCam	BMW Advanced Car Eye 3.0	Kamera przednia i tylna Full HD	2500.00	BMW	\N	t	t	45	\N	\N	\N	Czarna	Plastik	\N	Uniwersalna	\N	\N	t	Średnia	2 lata
acc-019	1	BMW X3	Elektronika	PhoneHolder	Uchwyt na telefon	Montaż w kratce nawiewu	200.00	BMW	\N	t	t	200	\N	\N	\N	Czarny	Plastik/Guma	\N	Uniwersalny	\N	\N	t	Łatwa	1 rok
acc-020	5	BMW M3	M Performance	ExhaustSystem	Układ wydechowy M Performance	Tytanowy układ z klapami	12000.00	BMW M	\N	t	t	8	\N	\N	\N	Tytan	Tytan	\N	M3 G80	\N	\N	f	Profesjonalna	3 lata
acc-021	5	BMW M3	M Performance	CarbonPackage	Pakiet Carbon M	Lusterka, dyfuzor, listwy	18000.00	BMW M	\N	t	t	5	\N	\N	\N	Carbon	Włókno węglowe	\N	M3 G80	\N	\N	f	Profesjonalna	3 lata
acc-022	1	BMW X3	Zimowe	SnowChains	Łańcuchy śniegowe	Automatyczne zakładanie	800.00	BMW	\N	t	t	60	\N	\N	\N	Srebrne	Stal	\N	X3 G01	\N	\N	f	Średnia	5 lat
acc-023	4	BMW X5	Zimowe	SkiBox	Box na narty i snowboard	Na 6 par nart lub 4 snowboardy	2200.00	BMW	\N	t	t	25	\N	\N	\N	Biały	ABS	\N	X5 G05	\N	\N	f	Średnia	3 lata
acc-024	3	BMW i4	Lifestyle	UmbrellaSet	Zestaw parasoli BMW	Dwa parasolki w drzwiach	400.00	BMW	\N	t	t	100	\N	\N	\N	Czarne z logo BMW	Tkanina wodoodporna	\N	Uniwersalne	\N	\N	t	Łatwa	1 rok
\.


--
-- TOC entry 3527 (class 0 OID 16451)
-- Dependencies: 225
-- Data for Name: car_interior_equipment; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.car_interior_equipment (id, type, value, description, additional_price, has_navigation, has_premium_sound, control_type) FROM stdin;
int-001	Seats	Skóra Vernasca	Wysokiej jakości skóra z przeszyciami	5000.00	f	f	\N
int-002	Seats	Skóra Merino	Ekskluzywna skóra pełnoziarnista	12000.00	f	f	\N
int-003	Seats	Skóra Dakota	Standardowa skóra perforowana	3500.00	f	f	\N
int-004	Seats	Alcantara/Sensatec	Kombinacja alcantary i skóry ekologicznej	2500.00	f	f	\N
int-005	Seats	Tkanina/Sensatec	Kombinacja tkaniny i skóry ekologicznej	0.00	f	f	\N
int-006	Seats	BMW Individual Merino	Najwyższej klasy skóra z rozszerzonym zakresem	18000.00	f	f	\N
int-007	Seats	M Sport fotele kubełkowe	Sportowe fotele z zintegrowanymi zagłówkami	8000.00	f	f	\N
int-008	Seats	Fotele multikonturowe	Fotele z masażem i wentylacją	15000.00	f	f	\N
int-009	Dashboard	Deska aluminium	Wykończenie aluminium szczotkowane	2000.00	f	f	\N
int-010	Dashboard	Deska carbon	Wykończenie z włókna węglowego	4000.00	f	f	\N
int-011	Dashboard	Deska drewno Fineline	Naturalne drewno Fineline	3000.00	f	f	\N
int-012	Dashboard	Deska drewno orzech	Amerykański orzech	3500.00	f	f	\N
int-013	Dashboard	Deska piano black	Czarne wykończenie na wysoki połysk	1500.00	f	f	\N
int-014	Dashboard	BMW Individual drewno	Ekskluzywne drewno Pianolack	6000.00	f	f	\N
int-015	Dashboard	M Performance carbon	Prawdziwe włókno węglowe z logo M	5000.00	f	f	\N
int-016	Multimedia	BMW Live Cockpit Plus	System z wyświetlaczem 10.25" i 8.8"	2000.00	t	f	iDrive 6.0
int-017	Multimedia	BMW Live Cockpit Professional	System z dwoma wyświetlaczami 12.3"	3500.00	t	f	iDrive 7.0
int-018	Multimedia	BMW Curved Display	Zakrzywiony wyświetlacz 14.9" + 12.3"	5000.00	t	f	iDrive 8.0
int-019	Multimedia	Head-Up Display	Wyświetlacz przezierny w polu widzenia	3000.00	f	f	\N
int-020	Multimedia	BMW Gesture Control	Sterowanie gestami	1500.00	f	f	\N
int-021	Multimedia	Wireless Charging	Ładowanie bezprzewodowe telefonu	800.00	f	f	\N
int-022	Audio	System Hi-Fi	Standardowy system audio 10 głośników	0.00	f	f	\N
int-023	Audio	Harman Kardon	System audio premium 16 głośników	4000.00	f	t	\N
int-024	Audio	Bowers & Wilkins	System audio Diamond surround 20 głośników	8000.00	f	t	\N
int-025	Audio	Bowers & Wilkins 4D	System audio 4D z wibracjami w fotelach	12000.00	f	t	\N
int-026	Lighting	Oświetlenie ambientowe	Podstawowe oświetlenie LED	500.00	f	f	\N
int-027	Lighting	Oświetlenie ambientowe rozszerzone	11 kolorów do wyboru	1200.00	f	f	\N
int-028	Lighting	BMW Individual oświetlenie	Personalizowane oświetlenie konturowe	2500.00	f	f	\N
int-029	SteeringWheel	Kierownica skórzana	Standardowa kierownica obszyta skórą	0.00	f	f	\N
int-030	SteeringWheel	Kierownica M Sport	Sportowa kierownica z grubszym wieńcem	1000.00	f	f	\N
int-031	SteeringWheel	Kierownica M Performance	Alcantara z oznaczeniem 12 godziny	2500.00	f	f	\N
int-032	SteeringWheel	Kierownica podgrzewana	Z funkcją ogrzewania	600.00	f	f	\N
int-033	Headliner	Podsufitka standardowa	Jasna podsufitka materiałowa	0.00	f	f	\N
int-034	Headliner	Podsufitka Anthrazit	Ciemna podsufitka BMW Individual	1500.00	f	f	\N
int-035	Headliner	Podsufitka Alcantara	Podsufitka obszyta alcantarą	3000.00	f	f	\N
int-036	ComfortPackage	Pakiet Comfort	Keyless, elektr. klapa, czujniki parkowania	4500.00	f	f	\N
int-037	ComfortPackage	Pakiet Comfort Plus	Comfort + asystent parkowania, kamera 360	7500.00	f	f	\N
int-038	ComfortPackage	Pakiet Premium	Wszystkie systemy komfortu i asystenci	12000.00	f	f	\N
int-039	Climate	Klimatyzacja 2-strefowa	Automatyczna klimatyzacja dwustrefowa	0.00	f	f	\N
int-040	Climate	Klimatyzacja 3-strefowa	Z dodatkową strefą dla pasażerów z tyłu	1500.00	f	f	\N
int-041	Climate	Klimatyzacja 4-strefowa	Indywidualna kontrola dla każdego fotela	2500.00	f	f	\N
int-042	Glass	Szyby przyciemniane	Tylne szyby przyciemniane	800.00	f	f	\N
int-043	Glass	Szyby akustyczne	Szyby z podwyższoną izolacją akustyczną	1200.00	f	f	\N
int-044	Glass	Szklany dach panoramiczny	Dwuczęściowy dach szklany	3500.00	f	f	\N
int-045	Glass	Sky Lounge Panorama	Dach panoramiczny z oświetleniem LED	5000.00	f	f	\N
int-046	Additional	Ambient Air	Aromatyzacja wnętrza	1000.00	f	f	\N
int-047	Additional	Travel & Comfort System	System mocowań w zagłówkach	600.00	f	f	\N
int-048	Additional	Pakiet dla palących	Popielniczka i zapalniczka	200.00	f	f	\N
int-049	Additional	Gniazda 230V	Gniazda elektryczne z tyłu	400.00	f	f	\N
int-050	Additional	Refrigerator	Lodówka w konsoli środkowej	2500.00	f	f	\N
\.


--
-- TOC entry 3525 (class 0 OID 16428)
-- Dependencies: 223
-- Data for Name: car_model_colors; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.car_model_colors ("Id", "CarModelId", "ColorName", "Price", "CreatedAt", "UpdatedAt") FROM stdin;
color-003	1	Szary	2000	2025-06-13 07:51:19.540971+00	\N
color-004	1	Niebieski	3000	2025-06-13 07:51:19.540971+00	\N
color-009	3	Szary	2000	2025-06-13 07:51:19.540971+00	\N
eb90e69d-eb22-4e36-96b1-8740aede34cc	4	Niebieski	10000	2025-06-13 08:02:21.196259+00	\N
27a20bf6-c4ee-4745-865b-7eec44a00b2e	4	Czarny	5000	2025-06-13 08:05:40.029468+00	\N
d9c7c73d-81ec-4e1b-8071-d9b2fecfe1d5	2	Granatowy	10000	2025-06-13 08:11:57.412625+00	\N
color-007	2	Czerwony	0	2025-06-13 07:51:19.540971+00	2025-06-13 08:11:57.412589+00
color-005	2	Czarny	5000	2025-06-13 07:51:19.540971+00	2025-06-13 08:11:57.412589+00
color-006	2	Biały	0	2025-06-13 07:51:19.540971+00	2025-06-13 08:12:05.222745+00
edc3fc28-213e-4d15-88e4-27853d13cd6e	3	Czerwony	14999	2025-06-13 08:17:37.659526+00	\N
color-010	3	Biały	0	2025-06-13 07:51:19.540971+00	2025-06-13 08:19:46.676809+00
color-008	3	Niebieski	9998	2025-06-13 07:51:19.540971+00	2025-06-13 08:19:46.676814+00
color-001	1	Czarny	4999	2025-06-13 07:51:19.540971+00	2025-06-13 08:24:02.060584+00
ac56940c-3bc0-4215-9d10-f285dce3222d	1	Czerwony	15000	2025-06-13 08:26:07.356899+00	\N
0b148465-4c00-4990-83e3-3d4a94ccb77e	4	Zielony	25000	2025-06-13 08:29:03.241783+00	\N
844d443f-5615-44a8-9435-6dd2f2e96436	4	Czerwony	10000	2025-06-13 08:30:36.756425+00	\N
0a5219a5-77d7-4e42-9858-8b7c250631c9	5	Zielony	25000	2025-06-13 08:31:38.585717+00	\N
4d04deec-a3c7-4ffb-973e-8b53605b0156	5	Czerwony	9999	2025-06-13 08:32:17.679843+00	\N
839b3d15-e92c-466c-895e-2b004f8e3d25	4	Biały	0	2025-06-13 08:02:21.196262+00	2025-06-13 12:36:14.965596+00
color-002	1	Biały	0	2025-06-13 07:51:19.540971+00	2025-06-13 12:43:16.77949+00
\.


--
-- TOC entry 3534 (class 0 OID 16502)
-- Dependencies: 232
-- Data for Name: cechypojazdu; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.cechypojazdu (id, idpojazdu, cecha) FROM stdin;
1	3	Tempomat adaptacyjny
2	2	System bezkluczykowy
3	2	Kamera cofania
4	1	System Start-Stop
5	2	LED Matrix
6	3	Asystent parkowania
7	1	Asystent parkowania
8	1	Tempomat adaptacyjny
9	2	Panoramiczny dach
10	2	Podgrzewane fotele
11	3	System Start-Stop
12	2	Asystent parkowania
13	1	Podgrzewane fotele
14	3	LED Matrix
15	1	Panoramiczny dach
16	2	Tempomat adaptacyjny
17	3	System bezkluczykowy
18	3	Kamera cofania
19	1	LED Matrix
20	3	Podgrzewane fotele
21	2	System Start-Stop
22	1	System bezkluczykowy
23	1	Kamera cofania
24	3	Panoramiczny dach
\.


--
-- TOC entry 3536 (class 0 OID 16514)
-- Dependencies: 234
-- Data for Name: elementwyposazenia; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.elementwyposazenia (id, typelementu, wartosc, opiselementu, cenadodatkowa) FROM stdin;
\.


--
-- TOC entry 3538 (class 0 OID 16523)
-- Dependencies: 236
-- Data for Name: konfiguracja; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.konfiguracja (id, iduzytkownika, idpojazdu, modelsilnikid, datautworzenia, nazwakonfiguracji, opiskonfiguracji) FROM stdin;
\.


--
-- TOC entry 3542 (class 0 OID 16559)
-- Dependencies: 240
-- Data for Name: logowanie; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.logowanie (id, iduzytkownika, datalogowania, adresip, status) FROM stdin;
\.


--
-- TOC entry 3532 (class 0 OID 16484)
-- Dependencies: 230
-- Data for Name: modelsilnik; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.modelsilnik (id, modelid, silnikid, cenadodatkowa) FROM stdin;
1	5	8	50000.00
2	4	7	20000.00
3	1	1	0.00
4	3	6	50000.00
5	3	5	30000.00
6	2	3	0.00
7	1	2	50000.00
8	2	4	0.00
\.


--
-- TOC entry 3522 (class 0 OID 16407)
-- Dependencies: 220
-- Data for Name: pojazd; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.pojazd (id, model, kolornadwozia, typnadwozia, wyposazeniewnetrza, cena, opis, ma4x4, jestelektryczny, akcesoria, zdjecie, imageurl, isactive) FROM stdin;
4	BMW X5	Szary	SUV	Skóra Nappa	350000	Duży luksusowy SUV	t	f	\N	\N	ca424797-384a-4c5e-9614-e17c9e4ddc63.png	t
5	BMW M3	Czerwony	Sedan	Skóra sportowa M	400000	Wysokowydajny sedan sportowy	f	f	\N	\N	633949a9-e355-4b3f-adb1-9148dffa61c5.png	t
1	BMW X3	Czarny	SUV	Skóra premium	250000	Luksusowy SUV klasy średniej	t	f	\N	\N	6df56686-f6e7-4872-8bbc-16dec39a543b.png	t
3	BMW i4	Niebieski	Sedan	Ekologiczna skóra	300000	Elektryczny sedan premium	f	t	\N	\N	96c8a50c-bd67-469e-af07-fae2e55cd45b.png	t
2	BMW Series 3	Biały	Combi	Alcantara sport	180000	Sportowy sedan	f	f	\N	\N	04f59be8-bc35-4f32-ae0c-ff7b1c9fcd9c.png	t
\.


--
-- TOC entry 3526 (class 0 OID 16440)
-- Dependencies: 224
-- Data for Name: pojazd_zdjecie; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.pojazd_zdjecie ("Id", "CarModelId", "ImageUrl", "Color", "DisplayOrder", "IsMainImage", "CreatedAt", "UpdatedAt") FROM stdin;
5f1ae8e0-4f91-45dc-b7b8-9edfdaed6e49	1	/uploads/e1fa37a9-f2f4-46d9-a347-a42d865413d4.png	Czarny	9	f	2025-06-13 12:41:58.604755+00	\N
e326d486-3a0e-437b-9793-f1b0ce12d25b	1	/uploads/0a0e1273-7ee2-4a68-b917-850e4aada1b6.png	Czarny	10	f	2025-06-13 12:42:03.353762+00	\N
70a5a731-57a5-48e4-8742-a1c632fd1134	1	/uploads/6164a36f-e4de-4b1a-93b6-2011dada1418.png	Czarny	11	f	2025-06-13 12:42:08.617797+00	\N
fd0436e1-bfb5-48af-9188-010084165d0a	1	/uploads/76e8cacf-fba7-42c4-a04c-9a62ceaf9734.png	Czarny	12	f	2025-06-13 12:42:12.683469+00	\N
5fc7669e-79b3-46db-8a9b-3d94bebd3d4b	1	/uploads/3b9ff50c-ceb0-415c-9a8e-cb7a09e08dff.png	Czarny	13	f	2025-06-13 12:42:16.601977+00	\N
ad376c43-759c-4a39-af5a-ee1c30e4548a	1	/uploads/101a418b-0572-4844-af29-7a45e49c5dad.png	Czarny	14	f	2025-06-13 12:42:19.931193+00	\N
aea6af3b-a1f1-481b-a0ad-b3afadd830ca	1	/uploads/e9ef2716-cf6d-413e-b631-d7cb112a31f9.png	Czarny	15	f	2025-06-13 12:42:24.390355+00	\N
d30de8a0-dd82-4f6d-999f-42d754aca0a5	1	/uploads/e76e40b9-7532-44d9-8c42-5ddd5eea599d.png	Czarny	16	f	2025-06-13 12:42:30.729628+00	\N
62d555ba-8198-4082-b334-4d5224c7a24e	1	/uploads/258155de-21c2-4b09-8697-c15628134ff3.png	Biały	17	f	2025-06-13 12:42:40.815052+00	\N
0de2c1d4-0c9c-4e35-b6d6-18154a347bfe	1	/uploads/f7f8fbf5-9db2-4b8c-9e75-f653a0d4970b.png	Biały	18	f	2025-06-13 12:42:45.009191+00	\N
e0b7ba51-74c9-4e4e-9313-b13eae39d719	1	/uploads/b9fe7319-fa79-45de-9f54-2568db33b5ba.png	Biały	19	f	2025-06-13 12:42:49.989441+00	\N
4231f734-a9d4-4abb-b1d5-65bece489491	1	/uploads/eb5a2c05-07d6-4f59-b5c0-a4d0ba942b6e.png	Biały	20	f	2025-06-13 12:42:54.629699+00	\N
5a8a64c7-cee7-416d-ae16-d76b62e11d5f	1	/uploads/72a48d87-1661-43ab-b4a5-af566248e2c9.png	Biały	21	f	2025-06-13 12:42:59.352731+00	\N
8d92cf00-c7bb-4834-81c2-3880464567ca	1	/uploads/47ca5643-9472-41c3-b727-49ba4fb6eb61.png	Biały	22	f	2025-06-13 12:43:03.890475+00	\N
a6552edc-d512-469a-a7c5-21bd4f721e39	1	/uploads/36f6f562-0015-4c18-abf4-a513a256437f.png	Biały	23	f	2025-06-13 12:43:08.092677+00	\N
e414270d-ff55-44c5-aac1-44a469d051b1	1	/uploads/c1fc75c8-b50f-433a-9341-d972a6b68ac1.png	Biały	24	f	2025-06-13 12:43:14.335034+00	\N
5d116bcd-e80b-4191-b709-a2d2414f8f08	3	/uploads/24c81fec-802f-4c2a-999f-c849366c82fe.png	Granatowy	14	f	2025-06-13 12:46:24.853094+00	\N
62a8a6de-d33d-4229-9a04-2e3bed013f1e	2	/uploads/cb183db6-2bb9-4e4d-b730-33894dcc47dd.png	Niebieski	16	f	2025-06-13 12:55:37.887983+00	2025-06-13 12:57:07.811215+00
3ba9fb24-69ee-4ec8-a012-a0f33fd304db	3	/uploads/2a50b1d9-a7a9-4c13-9ef7-53b6013d6e3f.png	Biały	24	f	2025-06-13 12:48:30.260387+00	\N
3126888c-41d2-4178-a653-c1c81c11220c	2	/uploads/0f6cec0f-c8ce-490f-a839-b4cab0b3e436.png	Niebieski	19	f	2025-06-13 12:58:12.708437+00	2025-06-13 12:58:28.780156+00
19429786-a42a-4969-93f7-2bc8deb5e169	2	/uploads/29bda876-825a-4c0e-a088-3c78d3deefd4.png	Niebieski	21	f	2025-06-13 12:55:16.878033+00	2025-06-13 12:58:48.293757+00
9a3aed97-c0a9-4312-874a-7907bd0e4c7a	2	/uploads/be18b412-cfe4-4f0e-849d-aae534df322f.png	Niebieski	20	f	2025-06-13 12:55:05.4254+00	2025-06-13 12:58:51.588885+00
2e456cd5-b9ca-424e-a977-9b9295c9b4f3	3	/uploads/b8c2f02a-7199-4d64-ae0b-848d042d9634.png	Biały	19	f	2025-06-13 12:48:54.219758+00	2025-06-13 12:50:20.225237+00
d6fc7caa-ba1c-40a9-a58e-c410de698161	3	/uploads/0ab13d1f-0447-40b3-9a76-e8fb9b49dcbc.png	Czerwony	1	t	2025-06-13 12:44:37.085356+00	\N
92a8943c-cb6f-45b3-8f71-9b23748ef6c4	3	/uploads/fe2466e0-531d-49d2-beab-e26083ab70aa.png	Czerwony	2	f	2025-06-13 12:44:42.286869+00	\N
295ea64f-c944-4972-9587-ed1ade43a0a1	3	/uploads/0aa70499-7975-4791-94a9-4b34527bd30e.png	Czerwony	3	f	2025-06-13 12:44:48.004305+00	\N
f25561dc-8d94-43fb-a650-81b94932a818	3	/uploads/d8b36339-caa7-4963-af69-431b364d69ed.png	Czerwony	4	f	2025-06-13 12:44:53.823173+00	\N
8903be75-a061-42a8-9087-f172dd57adac	3	/uploads/0791ff17-e6c0-48f3-80a4-cfbd035fce75.png	Czerwony	5	f	2025-06-13 12:45:01.133993+00	\N
8eec8ef3-f8b5-481f-8f39-f2ebdbc93086	3	/uploads/693d5b61-49d8-4a71-aaef-83bb39f724c8.png	Czerwony	6	f	2025-06-13 12:45:08.494792+00	\N
cf83d24f-8db5-4b45-880e-8fe6639d0f1c	3	/uploads/e60fa976-6d78-4a78-970b-53cd4f21daca.png	Czerwony	7	f	2025-06-13 12:45:15.243046+00	\N
fe1b2df0-ebcf-4e91-b18b-29898d3f7875	3	/uploads/d18831e5-f149-4b07-b148-7d514372857e.png	Czerwony	8	f	2025-06-13 12:45:22.235146+00	\N
078e5fcc-867f-470e-bb54-dc8b18c1018f	3	/uploads/56b380e0-41d2-4037-940b-8f8ec0cf8e1b.png	Granatowy	9	f	2025-06-13 12:45:36.517876+00	\N
55b99648-fdb9-4486-8c7c-94915c549b42	3	/uploads/9d2f7078-8d7b-42ae-9d69-0478f2b7f975.png	Granatowy	10	f	2025-06-13 12:45:42.91614+00	\N
6f8aad2e-b731-4154-9abb-d1cd4fdeeb25	3	/uploads/e9f595d7-de3e-4e9b-9c07-de69d7c1481a.png	Granatowy	12	f	2025-06-13 12:46:13.268079+00	\N
5fa910de-b796-4751-a307-7e908620ef7e	3	/uploads/192bd629-b808-4c67-a28a-b22fb47adcc9.png	Granatowy	16	f	2025-06-13 12:46:39.247708+00	\N
e06793ed-2371-4846-a314-154308f5ed7a	3	/uploads/a3110a94-427a-42d6-a2df-1c51eb614892.png	Biały	20	f	2025-06-13 12:47:36.971504+00	\N
5d4f7deb-81e3-4783-9926-e5ded34dff8d	3	/uploads/8c94d608-0367-4a6d-b74a-1b2f305cfad0.png	Biały	21	f	2025-06-13 12:48:03.884919+00	\N
f4ad22f0-6ee6-4128-939b-793207d5a954	3	/uploads/d834dbf4-4e13-4d05-a9d3-f24e09bc80e4.png	Biały	20	f	2025-06-13 12:49:39.592692+00	2025-06-13 12:50:16.450017+00
01cc0be9-1de9-4c4b-a287-d835391ab2ce	3	/uploads/026e2915-d5c6-4a95-b423-dca36ef06a3c.png	Granatowy	11	f	2025-06-13 12:46:06.887869+00	\N
72616b55-b61b-46f3-904f-ccfecd078905	3	/uploads/4996159e-59ab-4dcd-955e-0d83b5b0b213.png	Granatowy	15	f	2025-06-13 12:46:33.665724+00	\N
976bd49a-2e83-4f01-8fc2-55f0db54a72d	2	/uploads/7ee2d7c9-bab1-4ba5-b9ea-ead196947d8c.png	Niebieski	17	f	2025-06-13 12:54:25.046168+00	2025-06-13 12:57:10.264477+00
83bf86a4-4192-4b55-83ac-76fedb0f56a5	3	/uploads/0090b25e-d7c4-4f4f-9417-cdfbc36df71b.png	Biały	22	f	2025-06-13 12:48:15.911335+00	\N
aa634b9d-f480-4b6f-bdae-5084535f6375	2	/uploads/a690b2d7-b79b-4802-a2a0-47e64612aa4c.png	Biały	1	t	2025-06-13 12:51:38.192018+00	\N
cfc8b5ef-bcb1-45c6-b28d-c91c2f37359b	2	/uploads/82f66321-c56a-4145-a230-cae09f2a5708.png	Biały	2	f	2025-06-13 12:51:44.590058+00	\N
9c16a0da-4fbd-4712-bf41-bf07d30ce169	2	/uploads/a345476b-4a0a-4b76-a065-492250507808.png	Biały	3	f	2025-06-13 12:51:51.533995+00	\N
86069207-cbf7-4925-8976-b51eb8fd8277	2	/uploads/f019d2cd-4ad2-4419-a69f-0e1dfd1bac6e.png	Biały	4	f	2025-06-13 12:51:58.2637+00	\N
baa735b4-cd8d-46eb-838e-d4df4829438f	2	/uploads/7432be27-8911-4028-8b4b-b50280549c43.png	Biały	5	f	2025-06-13 12:52:04.914828+00	\N
96bf3d47-8ed3-4080-8fb1-ad770bed649c	2	/uploads/32474bef-6741-4e3d-8551-2e3fff313ea4.png	Biały	6	f	2025-06-13 12:52:14.948916+00	\N
8d9239b4-5f7d-4f0a-9433-561660244ee4	2	/uploads/3675fbdd-70a9-4b5a-b9d7-a4db0ddf8f63.png	Biały	7	f	2025-06-13 12:52:21.154313+00	\N
103e0310-dc2a-4606-ae2b-4674fa22d429	2	/uploads/538fd89e-b441-4bd6-80f1-e19ce4bfa9d9.png	Biały	8	f	2025-06-13 12:52:29.35389+00	\N
f71d6e83-c5db-4f8f-9a2d-a2a862af23cb	2	/uploads/cf1c15ea-026c-493a-87a4-0e1a3ce69fe8.png	Czarny	9	f	2025-06-13 12:52:44.69734+00	\N
7c85fce3-4383-4e2d-a782-0273217d99fd	2	/uploads/3621d925-b696-4030-8518-456e95fba7bc.png	Czarny	10	f	2025-06-13 12:52:51.968678+00	\N
801dcf02-0a40-4d47-8ceb-c8cbbd52e744	2	/uploads/c53d9ecb-a89b-4267-9f99-a8bc524c5233.png	Czarny	11	f	2025-06-13 12:53:00.959321+00	\N
a3c8c633-bcf2-444f-a070-2b8dca3de589	2	/uploads/24bfe6d2-c8cd-4afd-b53b-149d17d4263d.png	Czarny	12	f	2025-06-13 12:53:10.084447+00	\N
87584317-fe38-418d-bcd3-344f7ab94e48	2	/uploads/d9b5eeaa-317a-4d4b-b4e0-ca55d26c425b.png	Czarny	13	f	2025-06-13 12:53:19.091863+00	\N
15c7e963-8b01-4e56-9d50-90a165f2e37c	2	/uploads/4bfe4969-532c-4787-a54f-8bba58f6a221.png	Czarny	14	f	2025-06-13 12:53:30.446336+00	\N
87be7066-ad15-46f8-86ea-f52c3185f666	2	/uploads/ff283554-b94e-4a6f-ae6e-e43a9148b0c1.png	Czarny	15	f	2025-06-13 12:53:41.335494+00	\N
5eadb16f-90ee-42b2-b584-6c75e3c1c02f	2	/uploads/83015144-fa35-4182-bdc3-dbcf134deee2.png	Czarny	16	f	2025-06-13 12:53:50.332899+00	\N
fc76c2f3-9c76-46df-8828-1902579da6a2	2	/uploads/7aa9e1ab-cb74-4fa1-9273-f91e8e24032c.png	Niebieski	16	f	2025-06-13 12:54:11.238296+00	2025-06-13 12:57:17.199181+00
a0bf749f-9c4f-48a1-a265-7bccf43c4f90	2	/uploads/cc892bd6-4e26-466a-8fe7-b209fa3746af.png	Niebieski	18	f	2025-06-13 12:54:32.840075+00	2025-06-13 12:57:40.425769+00
946152b8-88d3-4335-acde-1646019ee667	3	/uploads/eaf86eae-6b50-47a4-a8fa-cdc483687259.png	Granatowy	13	f	2025-06-13 12:46:18.903047+00	\N
4c0d6a43-6f8d-4c6d-9ac4-d9f76674da42	4	/uploads/18f69716-1597-440c-abdf-47c0888de07f.png	Biały	1	t	2025-06-13 12:29:49.328089+00	\N
f879d38e-1c5e-4fef-8aad-8953cb09a94b	4	/uploads/d5aee94c-08dc-4edd-9591-9287c6361614.png	Biały	2	f	2025-06-13 12:29:59.327384+00	\N
d0690dc5-c460-4f0b-82b4-dff082b9bad3	4	/uploads/3ed56e32-2a6c-4a14-8641-9b2602e297c0.png	Biały	3	f	2025-06-13 12:30:14.481907+00	\N
3910cb7b-99e9-4053-b6f7-fb4404879c95	4	/uploads/4ca4e152-139d-4b5b-a41b-022fad8aa6e4.png	Biały	4	f	2025-06-13 12:30:23.676509+00	\N
4b10ea2a-f5ee-416f-b3e4-41cdfd70a8f1	4	/uploads/223ac5b2-bfc3-4d27-9b6a-c7dee344b647.png	Biały	5	f	2025-06-13 12:30:33.351639+00	\N
8b062b86-5d5d-41d9-9478-c37c87401845	4	/uploads/15f56f62-408b-434d-bf52-c12df754220f.png	Biały	6	f	2025-06-13 12:30:46.241307+00	\N
a65ebfb0-bd71-4f56-8694-a1f33e8deabf	3	/uploads/5cb4d197-a9f4-4a11-af68-5ec0685341df.png	Biały	17	f	2025-06-13 12:46:57.009706+00	\N
2c4d9dbc-7197-41a7-93f1-889ce4a7273b	3	/uploads/670f7cec-fa08-44b6-9ad9-599fd715d9ad.png	Biały	23	f	2025-06-13 12:48:23.75596+00	\N
897f636d-33d9-481e-8bd0-47c5c7d2a188	4	/uploads/5eb911de-4cab-45c1-b072-9c436f45b534.png	Biały	7	f	2025-06-13 12:31:24.335425+00	\N
44c0ca75-a727-47b2-addd-a09f86f4bf36	4	/uploads/f607793d-5202-4847-863d-c23d54e243d6.png	Biały	8	f	2025-06-13 12:31:33.070595+00	\N
6db371c5-5316-4f1e-8ecd-5adbd6a125f1	4	/uploads/65758903-da9a-411e-a6a4-0282a6fe4fa3.png	Granatowy	9	f	2025-06-13 12:32:08.51793+00	\N
01b838fe-3845-48e1-8aa0-2e680ddd3849	4	/uploads/e8124fed-56b2-4808-9347-e71285d3c688.png	Granatowy	10	f	2025-06-13 12:32:16.87809+00	\N
0cc96b6d-d135-43aa-b6fa-bbbe79e6d2c8	4	/uploads/99b396ee-6338-461e-bd94-4646407848bd.png	Granatowy	11	f	2025-06-13 12:32:25.522326+00	\N
0446a636-558d-470c-ad83-293d9bb8bc43	4	/uploads/ba57daf4-9bbd-42c1-8ea9-920cb5f9bfe2.png	Granatowy	12	f	2025-06-13 12:32:34.995328+00	\N
91715d7f-d0aa-4efd-9446-5f0a6b6ba252	4	/uploads/25aedcf7-cfe4-4e79-bd65-33cd1223b275.png	Granatowy	13	f	2025-06-13 12:32:43.946972+00	\N
258fc833-4b03-4e28-ae14-5e3a99045f32	4	/uploads/0b3a3efe-2fc7-4306-bedb-aedecc2ca1c8.png	Granatowy	14	f	2025-06-13 12:32:54.841385+00	\N
c07150ee-490d-403a-b47e-7dac791c733b	4	/uploads/18750df6-b244-4ddb-95c9-590e920cbba3.png	Granatowy	15	f	2025-06-13 12:33:09.368432+00	\N
12e441a3-1d70-4520-8f2f-11d5ec3fd117	4	/uploads/828922e5-dc06-4c39-a37f-5e61e060e2e7.png	Granatowy	16	f	2025-06-13 12:33:19.005426+00	\N
454659cf-8acd-4057-a037-3cf363cac717	4	/uploads/acad59bf-ecd2-4218-bb06-6aea23bab315.png	Czarny	17	f	2025-06-13 12:33:39.257165+00	\N
0d425e6e-ea37-41a8-925b-0ce649c6f31b	4	/uploads/bb64a4d9-494c-46df-b907-439cb97ad5c2.png	Czarny	18	f	2025-06-13 12:33:50.32886+00	\N
354c994f-22de-4eeb-a404-dab3a1302664	4	/uploads/05c95e37-6784-479c-a9f6-f133c1806cc9.png	Czarny	19	f	2025-06-13 12:34:55.738682+00	\N
2a1da993-2c0d-4032-b606-d3c53c260a99	4	/uploads/9b353320-157b-4528-9cd9-1e0d5250a572.png	Czarny	20	f	2025-06-13 12:35:06.727672+00	\N
3668c1cd-cd58-4af7-8bd8-d762f1249cf7	4	/uploads/6258fbac-5a34-40ff-9163-0d71f1966da6.png	Czarny	21	f	2025-06-13 12:35:34.970784+00	\N
7a6544f2-3582-4935-b6f9-6c935c0c5308	4	/uploads/a486de4b-8ba1-4215-b0ce-d233cdc555ec.png	Czarny	22	f	2025-06-13 12:35:49.310469+00	\N
9298d27f-b35f-4441-807e-c13732e55162	4	/uploads/5a44fa61-48dd-4319-8093-bda7e63d0190.png	Czarny	23	f	2025-06-13 12:36:00.654627+00	\N
e45b5154-6497-4719-855d-f0e197c4b6ec	4	/uploads/d2c31f76-ffcf-40bb-afb9-affadb80dab5.png	Czarny	24	f	2025-06-13 12:36:12.006861+00	\N
1ebc16a1-b5d3-474f-903e-d1f01e2404c5	5	/uploads/89d07f53-6f1c-4665-9e36-5944bb02535e.png	Zielony	9	f	2025-06-13 12:37:54.997034+00	\N
57a044f4-1f37-44a9-ba91-d660afe80700	5	/uploads/65e1e2e5-ab8d-4503-8c9b-e9c8b9ed9d94.png	Zielony	10	f	2025-06-13 12:37:58.692865+00	\N
1e173a76-8e30-4dfc-96ec-c7e0b55405f7	5	/uploads/7e929117-6630-470d-b94c-66e418e60afc.png	Zielony	11	f	2025-06-13 12:38:02.023227+00	\N
56fba29a-aadb-4df8-bb94-24b42446ed9f	5	/uploads/520c8a38-14c6-4b0b-901d-026c73ebb1e6.png	Zielony	12	f	2025-06-13 12:38:05.616816+00	\N
1c13dbe9-f100-4d91-8c3a-cdaac14ef3ab	5	/uploads/a3071eb5-8ea5-43ab-b5f2-9886cac72f3a.png	Zielony	13	f	2025-06-13 12:38:08.503268+00	\N
1f47e5a5-90a9-4daf-812d-35b0c525bf3b	5	/uploads/eaabd184-b5ab-4994-a109-ac53cfad67b4.png	Zielony	14	f	2025-06-13 12:38:11.293376+00	\N
2f253f54-a752-4f99-b956-d92f83f86dc2	5	/uploads/f5fade51-b539-4eed-9aa3-214dbe691c5c.png	Zielony	15	f	2025-06-13 12:38:16.593095+00	\N
8eaa2314-cac3-465a-8282-30207e9908d0	5	/uploads/8c1b8487-60c7-4e22-b10f-477ca62dd669.png	Zielony	16	f	2025-06-13 12:38:20.228516+00	\N
8be74280-a4d8-4606-85da-d9025aed7aa0	5	/uploads/abde7cb8-9056-48d4-af08-374f27d03a12.png	Czerwony	9	f	2025-06-13 12:38:52.355912+00	\N
8f28aa6a-42be-498e-a6cf-44043012b7d2	5	/uploads/c72aa750-45ac-481c-86df-41dff7566fd2.png	Czerwony	10	f	2025-06-13 12:38:58.082367+00	\N
6b1bbbfe-f809-42b1-a179-d2e826c9e22b	5	/uploads/ff6c063f-e051-409d-ba5b-bd1fbc5b95dc.png	Czerwony	11	f	2025-06-13 12:39:02.499829+00	\N
380b94fb-8672-4c9f-9dcc-21758d0a1333	5	/uploads/1c48402d-7589-4731-aba8-134c3f452c7e.png	Czerwony	12	f	2025-06-13 12:39:06.235896+00	\N
854b0b78-c63d-41b3-b876-4718015db7ac	5	/uploads/6ac94973-0518-4bec-ad90-46078688ddfb.png	Czerwony	13	f	2025-06-13 12:39:09.294617+00	\N
cb0ee4e0-1459-4cb9-9b5b-92d28e7b77e6	5	/uploads/d92dea1e-087c-4025-a164-6c035e83e46d.png	Czerwony	14	f	2025-06-13 12:39:12.410814+00	\N
8379ceaa-a5c4-4cc5-b74f-52a977c415f7	5	/uploads/b26810b6-4b99-4280-a98c-c0781c530d8f.png	Czerwony	15	f	2025-06-13 12:39:15.638489+00	\N
84f955ef-6708-4402-bceb-98a7b66e2c9d	5	/uploads/166680a8-58da-432e-b00f-36f04bc0345a.png	Czerwony	16	f	2025-06-13 12:39:18.538252+00	\N
c480777d-2571-4248-9d26-88542593a530	1	/uploads/2434a0c4-b729-4eb1-8421-40753e0441be.png	Czerwony	1	t	2025-06-13 12:41:22.421288+00	\N
483a54a9-3747-4780-815c-02d941db846d	1	/uploads/d71918c4-c420-4c9a-a5d2-f4e03238793f.png	Czerwony	2	f	2025-06-13 12:41:26.582393+00	\N
b891aae4-d2a6-4fae-92da-3d22333cae0b	1	/uploads/99f8ffa0-8e82-463b-bc22-608d192ea9ca.png	Czerwony	3	f	2025-06-13 12:41:29.721962+00	\N
fca9f7ca-3f2b-43f2-88e2-f29aaebd6b60	1	/uploads/d8d27477-29a8-4ed1-a062-2be578959cb9.png	Czerwony	4	f	2025-06-13 12:41:34.682935+00	\N
7a5fd51d-aa08-40e9-bcd1-eba736e6ce79	1	/uploads/5ac8a7ce-721a-4ebf-9102-a7fcb5c4314a.png	Czerwony	5	f	2025-06-13 12:41:37.942652+00	\N
ef181108-18e2-4b82-9246-964535ddf622	1	/uploads/300344fd-c6ae-4d26-9bf6-ebea7c2151e3.png	Czerwony	6	f	2025-06-13 12:41:41.432493+00	\N
9a20407e-fe2c-4ed1-811b-c6de73825158	1	/uploads/e01544f4-980a-4ac0-902c-d40208fadbe9.png	Czerwony	7	f	2025-06-13 12:41:45.811277+00	\N
974e76c3-7ac5-426f-a12e-dcef6d45b549	1	/uploads/dc95692e-b834-4dd6-bd7b-317fd160c627.png	Czerwony	8	f	2025-06-13 12:41:49.524803+00	\N
00e50813-62c5-43ec-a43c-5bd6b82d0f11	2	/uploads/1bcba34d-15a8-4117-888a-863df742ef81.png	Niebieski	20	f	2025-06-13 12:54:54.481778+00	2025-06-13 12:59:09.039371+00
\.


--
-- TOC entry 3544 (class 0 OID 16571)
-- Dependencies: 242
-- Data for Name: rejestracja; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.rejestracja (id, iduzytkownika, datarejestracji, adresip, status) FROM stdin;
\.


--
-- TOC entry 3540 (class 0 OID 16547)
-- Dependencies: 238
-- Data for Name: udostepnieniekonfiguracji; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.udostepnieniekonfiguracji (id, idkonfiguracji, link, odbiorca, dataudostepnienia) FROM stdin;
\.


--
-- TOC entry 3524 (class 0 OID 16417)
-- Dependencies: 222
-- Data for Name: user_configurations; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.user_configurations (id, user_id, configuration_name, car_model_id, car_model_name, engine_id, engine_name, exterior_color, exterior_color_name, selected_accessories, selected_interior_equipment, total_price, created_at, updated_at, is_active) FROM stdin;
1	3	BMW X3 - 13.06.2025	1	BMW X3	1	20i xDrive	#000000	Czarny	[{"Id":"acc-028","Name":"Felgi X5 M 22\\u0022","Price":12000.00,"Type":"AlloyWheels"}]	[{"Id":"int-047","Type":"Additional","Value":"Travel \\u0026 Comfort System","Description":"System mocowa\\u0144 w zag\\u0142\\u00F3wkach","AdditionalPrice":600.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00},{"Id":"int-041","Type":"Climate","Value":"Klimatyzacja 4-strefowa","Description":"Indywidualna kontrola dla ka\\u017Cdego fotela","AdditionalPrice":2500.00},{"Id":"int-038","Type":"ComfortPackage","Value":"Pakiet Premium","Description":"Wszystkie systemy komfortu i asystenci","AdditionalPrice":12000.00},{"Id":"int-014","Type":"Dashboard","Value":"BMW Individual drewno","Description":"Ekskluzywne drewno Pianolack","AdditionalPrice":6000.00},{"Id":"int-044","Type":"Glass","Value":"Szklany dach panoramiczny","Description":"Dwucz\\u0119\\u015Bciowy dach szklany","AdditionalPrice":3500.00},{"Id":"int-035","Type":"Headliner","Value":"Podsufitka Alcantara","Description":"Podsufitka obszyta alcantar\\u0105","AdditionalPrice":3000.00},{"Id":"int-028","Type":"Lighting","Value":"BMW Individual o\\u015Bwietlenie","Description":"Personalizowane o\\u015Bwietlenie konturowe","AdditionalPrice":2500.00},{"Id":"int-017","Type":"Multimedia","Value":"BMW Live Cockpit Professional","Description":"System z dwoma wy\\u015Bwietlaczami 12.3\\u0022","AdditionalPrice":3500.00},{"Id":"int-002","Type":"Seats","Value":"Sk\\u00F3ra Merino","Description":"Ekskluzywna sk\\u00F3ra pe\\u0142noziarnista","AdditionalPrice":12000.00},{"Id":"int-031","Type":"SteeringWheel","Value":"Kierownica M Performance","Description":"Alcantara z oznaczeniem 12 godziny","AdditionalPrice":2500.00}]	322100.00	2025-06-13 07:54:03.4758+00	2025-06-13 07:54:37.368448+00	f
3	3	BMW M3 - 13.06.2025	5	BMW M3	8	M Competition	#008000	Zielony	[{"Id":"acc-028","Name":"Felgi X5 M 22\\u0022","Price":12000.00,"Type":"AlloyWheels"},{"Id":"acc-004","Name":"Dywaniki welurowe M","Price":800.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"},{"Id":"acc-033","Name":"Kabel Mode 3","Price":800.00,"Type":"ChargingCable"},{"Id":"acc-009","Name":"BMW Wallbox Plus","Price":5000.00,"Type":"ChargingStation"},{"Id":"acc-034","Name":"Box dachowy 520L","Price":3800.00,"Type":"RoofBox"},{"Id":"acc-011","Name":"Baga\\u017Cnik rowerowy","Price":1800.00,"Type":"BikeRack"},{"Id":"acc-012","Name":"Chlapacze przednie i tylne","Price":350.00,"Type":"MudFlaps"},{"Id":"acc-035","Name":"Pe\\u0142na folia PPF","Price":15000.00,"Type":"PaintProtection"},{"Id":"acc-015","Name":"Fotelik Baby Seat","Price":1800.00,"Type":"ChildSeat"},{"Id":"acc-016","Name":"Rolety przeciws\\u0142oneczne","Price":600.00,"Type":"SunShade"},{"Id":"acc-017","Name":"Lod\\u00F3wka samochodowa","Price":1200.00,"Type":"CoolBox"},{"Id":"acc-018","Name":"BMW Advanced Car Eye 3.0","Price":2500.00,"Type":"DashCam"},{"Id":"acc-019","Name":"Uchwyt na telefon","Price":200.00,"Type":"PhoneHolder"},{"Id":"acc-020","Name":"Uk\\u0142ad wydechowy M Performance","Price":12000.00,"Type":"ExhaustSystem"},{"Id":"acc-021","Name":"Pakiet Carbon M","Price":18000.00,"Type":"CarbonPackage"}]	[{"Id":"int-047","Type":"Additional","Value":"Travel \\u0026 Comfort System","Description":"System mocowa\\u0144 w zag\\u0142\\u00F3wkach","AdditionalPrice":600.00},{"Id":"int-023","Type":"Audio","Value":"Harman Kardon","Description":"System audio premium 16 g\\u0142o\\u015Bnik\\u00F3w","AdditionalPrice":4000.00},{"Id":"int-040","Type":"Climate","Value":"Klimatyzacja 3-strefowa","Description":"Z dodatkow\\u0105 stref\\u0105 dla pasa\\u017Cer\\u00F3w z ty\\u0142u","AdditionalPrice":1500.00},{"Id":"int-038","Type":"ComfortPackage","Value":"Pakiet Premium","Description":"Wszystkie systemy komfortu i asystenci","AdditionalPrice":12000.00},{"Id":"int-013","Type":"Dashboard","Value":"Deska piano black","Description":"Czarne wyko\\u0144czenie na wysoki po\\u0142ysk","AdditionalPrice":1500.00},{"Id":"int-043","Type":"Glass","Value":"Szyby akustyczne","Description":"Szyby z podwy\\u017Cszon\\u0105 izolacj\\u0105 akustyczn\\u0105","AdditionalPrice":1200.00},{"Id":"int-035","Type":"Headliner","Value":"Podsufitka Alcantara","Description":"Podsufitka obszyta alcantar\\u0105","AdditionalPrice":3000.00},{"Id":"int-026","Type":"Lighting","Value":"O\\u015Bwietlenie ambientowe","Description":"Podstawowe o\\u015Bwietlenie LED","AdditionalPrice":500.00},{"Id":"int-021","Type":"Multimedia","Value":"Wireless Charging","Description":"\\u0141adowanie bezprzewodowe telefonu","AdditionalPrice":800.00},{"Id":"int-007","Type":"Seats","Value":"M Sport fotele kube\\u0142kowe","Description":"Sportowe fotele z zintegrowanymi zag\\u0142\\u00F3wkami","AdditionalPrice":8000.00},{"Id":"int-030","Type":"SteeringWheel","Value":"Kierownica M Sport","Description":"Sportowa kierownica z grubszym wie\\u0144cem","AdditionalPrice":1000.00}]	592950.00	2025-06-13 08:33:55.287942+00	2025-06-13 08:35:58.965916+00	f
2	3	BMW X3 - 13.06.2025	1	BMW X3	1	20i xDrive	#000000	Czarny	[{"Id":"acc-028","Name":"Felgi X5 M 22\\u0022","Price":12000.00,"Type":"AlloyWheels"},{"Id":"acc-024","Name":"Zestaw parasoli BMW","Price":400.00,"Type":"UmbrellaSet"},{"Id":"acc-023","Name":"Box na narty i snowboard","Price":2200.00,"Type":"SkiBox"},{"Id":"acc-022","Name":"\\u0141a\\u0144cuchy \\u015Bniegowe","Price":800.00,"Type":"SnowChains"}]	[{"Id":"int-047","Type":"Additional","Value":"Travel \\u0026 Comfort System","Description":"System mocowa\\u0144 w zag\\u0142\\u00F3wkach","AdditionalPrice":600.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00},{"Id":"int-041","Type":"Climate","Value":"Klimatyzacja 4-strefowa","Description":"Indywidualna kontrola dla ka\\u017Cdego fotela","AdditionalPrice":2500.00},{"Id":"int-038","Type":"ComfortPackage","Value":"Pakiet Premium","Description":"Wszystkie systemy komfortu i asystenci","AdditionalPrice":12000.00},{"Id":"int-014","Type":"Dashboard","Value":"BMW Individual drewno","Description":"Ekskluzywne drewno Pianolack","AdditionalPrice":6000.00},{"Id":"int-044","Type":"Glass","Value":"Szklany dach panoramiczny","Description":"Dwucz\\u0119\\u015Bciowy dach szklany","AdditionalPrice":3500.00},{"Id":"int-035","Type":"Headliner","Value":"Podsufitka Alcantara","Description":"Podsufitka obszyta alcantar\\u0105","AdditionalPrice":3000.00},{"Id":"int-028","Type":"Lighting","Value":"BMW Individual o\\u015Bwietlenie","Description":"Personalizowane o\\u015Bwietlenie konturowe","AdditionalPrice":2500.00},{"Id":"int-017","Type":"Multimedia","Value":"BMW Live Cockpit Professional","Description":"System z dwoma wy\\u015Bwietlaczami 12.3\\u0022","AdditionalPrice":3500.00},{"Id":"int-002","Type":"Seats","Value":"Sk\\u00F3ra Merino","Description":"Ekskluzywna sk\\u00F3ra pe\\u0142noziarnista","AdditionalPrice":12000.00},{"Id":"int-031","Type":"SteeringWheel","Value":"Kierownica M Performance","Description":"Alcantara z oznaczeniem 12 godziny","AdditionalPrice":2500.00}]	325500.00	2025-06-13 07:54:21.660141+00	2025-06-13 08:36:03.793048+00	f
4	3	BMW Series 3 - 13.06.2025	2	BMW Series 3	3	320i	#000080	Granatowy	[{"Id":"acc-003","Name":"Felgi zimowe 18\\u0022","Price":5000.00,"Type":"AlloyWheels"},{"Id":"acc-030","Name":"Dywaniki M Performance","Price":900.00,"Type":"FloorMats"}]	[{"Id":"int-046","Type":"Additional","Value":"Ambient Air","Description":"Aromatyzacja wn\\u0119trza","AdditionalPrice":1000.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00},{"Id":"int-040","Type":"Climate","Value":"Klimatyzacja 3-strefowa","Description":"Z dodatkow\\u0105 stref\\u0105 dla pasa\\u017Cer\\u00F3w z ty\\u0142u","AdditionalPrice":1500.00}]	210400.00	2025-06-13 08:34:32.947673+00	2025-06-13 08:36:09.219823+00	f
5	3	BMW X5 - 13.06.2025	4	BMW X5	7	40i xDrive	#000080	Granatowy	[{"Id":"acc-002","Name":"Felgi sportowe 19\\u0022","Price":6000.00,"Type":"AlloyWheels"},{"Id":"acc-004","Name":"Dywaniki welurowe M","Price":800.00,"Type":"FloorMats"},{"Id":"acc-006","Name":"Tylny spoiler M Performance","Price":3500.00,"Type":"Spoiler"}]	[{"Id":"int-046","Type":"Additional","Value":"Ambient Air","Description":"Aromatyzacja wn\\u0119trza","AdditionalPrice":1000.00},{"Id":"int-024","Type":"Audio","Value":"Bowers \\u0026 Wilkins","Description":"System audio Diamond surround 20 g\\u0142o\\u015Bnik\\u00F3w","AdditionalPrice":8000.00}]	391800.00	2025-06-13 08:37:40.900801+00	2025-06-13 08:43:40.917278+00	f
10	3	BMW Series 3 - 13.06.2025	2	BMW Series 3	3	320i	#000080	Granatowy	[{"Id":"acc-003","Name":"Felgi zimowe 18\\u0022","Price":5000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	196600.00	2025-06-13 08:40:44.768682+00	2025-06-13 08:43:22.61901+00	f
9	3	BMW i4 - 13.06.2025	3	BMW i4	5	eDrive40	#000080	Granatowy	[{"Id":"acc-003","Name":"Felgi zimowe 18\\u0022","Price":5000.00,"Type":"AlloyWheels"},{"Id":"acc-030","Name":"Dywaniki M Performance","Price":900.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	338800.00	2025-06-13 08:40:24.111933+00	2025-06-13 08:43:24.497378+00	f
8	3	BMW X5 - 13.06.2025	4	BMW X5	7	40i xDrive	#000080	Granatowy	[{"Id":"acc-025","Name":"Felgi M Sport 19\\u0022","Price":7000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	381100.00	2025-06-13 08:40:05.678685+00	2025-06-13 08:43:27.18786+00	f
7	3	BMW M3 - 13.06.2025	5	BMW M3	8	M Competition	#008000	Zielony	[{"Id":"acc-002","Name":"Felgi sportowe 19\\u0022","Price":6000.00,"Type":"AlloyWheels"},{"Id":"acc-030","Name":"Dywaniki M Performance","Price":900.00,"Type":"FloorMats"}]	[{"Id":"int-046","Type":"Additional","Value":"Ambient Air","Description":"Aromatyzacja wn\\u0119trza","AdditionalPrice":1000.00}]	482900.00	2025-06-13 08:39:41.637038+00	2025-06-13 08:43:35.585268+00	f
11	3	BMW X3 - 13.06.2025	1	BMW X3	1	20i xDrive	#ff0000	Czerwony	[{"Id":"acc-002","Name":"Felgi sportowe 19\\u0022","Price":6000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	272600.00	2025-06-13 08:41:11.014143+00	2025-06-13 08:43:20.608419+00	f
12	3	BMW X5 - 13.06.2025	4	BMW X5	7	40i xDrive	#000080	Granatowy	[{"Id":"acc-001","Name":"Felgi M Performance 20\\u0022","Price":8000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"},{"Id":"acc-033","Name":"Kabel Mode 3","Price":800.00,"Type":"ChargingCable"},{"Id":"acc-034","Name":"Box dachowy 520L","Price":3800.00,"Type":"RoofBox"},{"Id":"acc-035","Name":"Pe\\u0142na folia PPF","Price":15000.00,"Type":"PaintProtection"},{"Id":"acc-015","Name":"Fotelik Baby Seat","Price":1800.00,"Type":"ChildSeat"}]	[{"Id":"int-046","Type":"Additional","Value":"Ambient Air","Description":"Aromatyzacja wn\\u0119trza","AdditionalPrice":1000.00}]	412100.00	2025-06-13 08:42:49.150914+00	2025-06-13 08:43:30.182606+00	f
13	3	BMW X5 - 13.06.2025	4	BMW X5	7	40i xDrive	#000080	Granatowy	[{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"},{"Id":"acc-033","Name":"Kabel Mode 3","Price":800.00,"Type":"ChargingCable"},{"Id":"acc-034","Name":"Box dachowy 520L","Price":3800.00,"Type":"RoofBox"},{"Id":"acc-035","Name":"Pe\\u0142na folia PPF","Price":15000.00,"Type":"PaintProtection"},{"Id":"acc-015","Name":"Fotelik Baby Seat","Price":1800.00,"Type":"ChildSeat"},{"Id":"acc-003","Name":"Felgi zimowe 18\\u0022","Price":5000.00,"Type":"AlloyWheels"}]	[{"Id":"int-046","Type":"Additional","Value":"Ambient Air","Description":"Aromatyzacja wn\\u0119trza","AdditionalPrice":1000.00}]	409100.00	2025-06-13 08:43:02.180173+00	2025-06-13 08:43:32.43345+00	f
6	3	test	5	BMW M3	8	M Competition	#008000	Zielony	[{"Id":"acc-025","Name":"Felgi M Sport 19\\u0022","Price":7000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"},{"Id":"acc-033","Name":"Kabel Mode 3","Price":800.00,"Type":"ChargingCable"},{"Id":"acc-009","Name":"BMW Wallbox Plus","Price":5000.00,"Type":"ChargingStation"},{"Id":"acc-034","Name":"Box dachowy 520L","Price":3800.00,"Type":"RoofBox"},{"Id":"acc-011","Name":"Baga\\u017Cnik rowerowy","Price":1800.00,"Type":"BikeRack"},{"Id":"acc-012","Name":"Chlapacze przednie i tylne","Price":350.00,"Type":"MudFlaps"},{"Id":"acc-035","Name":"Pe\\u0142na folia PPF","Price":15000.00,"Type":"PaintProtection"},{"Id":"acc-015","Name":"Fotelik Baby Seat","Price":1800.00,"Type":"ChildSeat"},{"Id":"acc-016","Name":"Rolety przeciws\\u0142oneczne","Price":600.00,"Type":"SunShade"},{"Id":"acc-017","Name":"Lod\\u00F3wka samochodowa","Price":1200.00,"Type":"CoolBox"},{"Id":"acc-018","Name":"BMW Advanced Car Eye 3.0","Price":2500.00,"Type":"DashCam"},{"Id":"acc-019","Name":"Uchwyt na telefon","Price":200.00,"Type":"PhoneHolder"},{"Id":"acc-020","Name":"Uk\\u0142ad wydechowy M Performance","Price":12000.00,"Type":"ExhaustSystem"},{"Id":"acc-021","Name":"Pakiet Carbon M","Price":18000.00,"Type":"CarbonPackage"},{"Id":"acc-022","Name":"\\u0141a\\u0144cuchy \\u015Bniegowe","Price":800.00,"Type":"SnowChains"},{"Id":"acc-023","Name":"Box na narty i snowboard","Price":2200.00,"Type":"SkiBox"},{"Id":"acc-024","Name":"Zestaw parasoli BMW","Price":400.00,"Type":"UmbrellaSet"}]	[{"Id":"int-048","Type":"Additional","Value":"Pakiet dla pal\\u0105cych","Description":"Popielniczka i zapalniczka","AdditionalPrice":200.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00},{"Id":"int-041","Type":"Climate","Value":"Klimatyzacja 4-strefowa","Description":"Indywidualna kontrola dla ka\\u017Cdego fotela","AdditionalPrice":2500.00},{"Id":"int-038","Type":"ComfortPackage","Value":"Pakiet Premium","Description":"Wszystkie systemy komfortu i asystenci","AdditionalPrice":12000.00},{"Id":"int-010","Type":"Dashboard","Value":"Deska carbon","Description":"Wyko\\u0144czenie z w\\u0142\\u00F3kna w\\u0119glowego","AdditionalPrice":4000.00}]	588350.00	2025-06-13 08:39:11.522308+00	2025-06-13 08:43:38.815398+00	f
17	3	test	3	BMW i4	5	eDrive40	#ff0000	Czerwony	[{"Id":"acc-026","Name":"Felgi Turbine 18\\u0022","Price":5500.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"},{"Id":"acc-033","Name":"Kabel Mode 3","Price":800.00,"Type":"ChargingCable"},{"Id":"acc-009","Name":"BMW Wallbox Plus","Price":5000.00,"Type":"ChargingStation"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00},{"Id":"int-040","Type":"Climate","Value":"Klimatyzacja 3-strefowa","Description":"Z dodatkow\\u0105 stref\\u0105 dla pasa\\u017Cer\\u00F3w z ty\\u0142u","AdditionalPrice":1500.00}]	379399.00	2025-06-13 10:06:02.424571+00	2025-06-13 10:12:36.629908+00	f
18	3	BMW Series 3 - 13.06.2025	2	BMW Series 3	3	320i	#000080	Granatowy	[{"Id":"acc-026","Name":"Felgi Turbine 18\\u0022","Price":5500.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	197100.00	2025-06-13 10:12:47.717602+00	2025-06-13 10:50:21.955556+00	f
16	3	BMW i4 - 13.06.2025	3	BMW i4	5	eDrive40	#000080	Granatowy	[{"Id":"acc-002","Name":"Felgi sportowe 19\\u0022","Price":6000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"}]	[{"Id":"int-050","Type":"Additional","Value":"Refrigerator","Description":"Lod\\u00F3wka w konsoli \\u015Brodkowej","AdditionalPrice":2500.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00}]	362200.00	2025-06-13 09:47:10.254058+00	2025-06-13 11:24:25.090935+00	f
19	3	BMW X5 - 13.06.2025	4	BMW X5	7	40i xDrive	#000080	Granatowy	[{"Id":"acc-002","Name":"Felgi sportowe 19\\u0022","Price":6000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	380100.00	2025-06-13 11:25:19.230563+00	2025-06-13 11:50:48.690802+00	f
15	3	BMW X3 - 13.06.2025	1	BMW X3	1	20i xDrive	#ff0000	Czerwony	[{"Id":"acc-003","Name":"Felgi zimowe 18\\u0022","Price":5000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	279600.00	2025-06-13 09:44:49.615184+00	2025-06-13 12:27:43.796738+00	f
14	3	BMW M3 - 13.06.2025	5	BMW M3	8	M Competition	#008000	Zielony	[{"Id":"acc-002","Name":"Felgi sportowe 19\\u0022","Price":6000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	482600.00	2025-06-13 08:43:53.21098+00	2025-06-13 12:27:46.124407+00	f
20	3	BMW M3 - 13.06.2025	5	BMW M3	8	M Competition	#008000	Zielony	[{"Id":"acc-002","Name":"Felgi sportowe 19\\u0022","Price":6000.00,"Type":"AlloyWheels"},{"Id":"acc-004","Name":"Dywaniki welurowe M","Price":800.00,"Type":"FloorMats"}]	[{"Id":"int-046","Type":"Additional","Value":"Ambient Air","Description":"Aromatyzacja wn\\u0119trza","AdditionalPrice":1000.00}]	482800.00	2025-06-13 11:25:44.073075+00	2025-06-13 11:50:45.497607+00	f
27	3	BMW Series 3 - 13.06.2025	2	BMW Series 3	3	320i	#ffffff	Biały	[]	[]	180000.00	2025-06-13 12:24:24.238424+00	2025-06-13 12:27:24.046494+00	f
25	3	BMW X3 - 13.06.2025	1	BMW X3	1	20i xDrive	#ff0000	Czerwony	[{"Id":"acc-026","Name":"Felgi Turbine 18\\u0022","Price":5500.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"},{"Id":"acc-033","Name":"Kabel Mode 3","Price":800.00,"Type":"ChargingCable"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00},{"Id":"int-040","Type":"Climate","Value":"Klimatyzacja 3-strefowa","Description":"Z dodatkow\\u0105 stref\\u0105 dla pasa\\u017Cer\\u00F3w z ty\\u0142u","AdditionalPrice":1500.00},{"Id":"int-037","Type":"ComfortPackage","Value":"Pakiet Comfort Plus","Description":"Comfort \\u002B asystent parkowania, kamera 360","AdditionalPrice":7500.00},{"Id":"int-009","Type":"Dashboard","Value":"Deska aluminium","Description":"Wyko\\u0144czenie aluminium szczotkowane","AdditionalPrice":2000.00},{"Id":"int-044","Type":"Glass","Value":"Szklany dach panoramiczny","Description":"Dwucz\\u0119\\u015Bciowy dach szklany","AdditionalPrice":3500.00},{"Id":"int-034","Type":"Headliner","Value":"Podsufitka Anthrazit","Description":"Ciemna podsufitka BMW Individual","AdditionalPrice":1500.00},{"Id":"int-026","Type":"Lighting","Value":"O\\u015Bwietlenie ambientowe","Description":"Podstawowe o\\u015Bwietlenie LED","AdditionalPrice":500.00},{"Id":"int-020","Type":"Multimedia","Value":"BMW Gesture Control","Description":"Sterowanie gestami","AdditionalPrice":1500.00},{"Id":"int-006","Type":"Seats","Value":"BMW Individual Merino","Description":"Najwy\\u017Cszej klasy sk\\u00F3ra z rozszerzonym zakresem","AdditionalPrice":18000.00}]	328900.00	2025-06-13 12:15:09.772042+00	2025-06-13 12:27:29.936431+00	f
24	3	BMW X5 - 13.06.2025	4	BMW X5	7	40i xDrive	#ffffff	Biały	[]	[]	370000.00	2025-06-13 12:14:23.246492+00	2025-06-13 12:27:33.100317+00	f
23	3	BMW Series 3 - 13.06.2025	2	BMW Series 3	3	320i	#000080	Granatowy	[{"Id":"acc-003","Name":"Felgi zimowe 18\\u0022","Price":5000.00,"Type":"AlloyWheels"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00}]	215400.00	2025-06-13 11:51:24.564558+00	2025-06-13 12:27:36.006342+00	f
22	3	BMW i4 - 13.06.2025	3	BMW i4	5	eDrive40	#000080	Granatowy	[{"Id":"acc-001","Name":"Felgi M Performance 20\\u0022","Price":8000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"}]	[{"Id":"int-050","Type":"Additional","Value":"Refrigerator","Description":"Lod\\u00F3wka w konsoli \\u015Brodkowej","AdditionalPrice":2500.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00}]	364200.00	2025-06-13 11:51:05.954893+00	2025-06-13 12:27:39.104149+00	f
21	3	BMW Series 3 - 13.06.2025	2	BMW Series 3	3	320i	#000080	Granatowy	[{"Id":"acc-026","Name":"Felgi Turbine 18\\u0022","Price":5500.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	197100.00	2025-06-13 11:26:03.801708+00	2025-06-13 12:27:41.729189+00	f
26	3	BMW X3 - 13.06.2025	1	BMW X3	1	20i xDrive	#ff0000	Czerwony	[{"Id":"acc-026","Name":"Felgi Turbine 18\\u0022","Price":5500.00,"Type":"AlloyWheels"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"},{"Id":"acc-033","Name":"Kabel Mode 3","Price":800.00,"Type":"ChargingCable"},{"Id":"acc-009","Name":"BMW Wallbox Plus","Price":5000.00,"Type":"ChargingStation"},{"Id":"acc-034","Name":"Box dachowy 520L","Price":3800.00,"Type":"RoofBox"}]	[{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00},{"Id":"int-040","Type":"Climate","Value":"Klimatyzacja 3-strefowa","Description":"Z dodatkow\\u0105 stref\\u0105 dla pasa\\u017Cer\\u00F3w z ty\\u0142u","AdditionalPrice":1500.00},{"Id":"int-037","Type":"ComfortPackage","Value":"Pakiet Comfort Plus","Description":"Comfort \\u002B asystent parkowania, kamera 360","AdditionalPrice":7500.00},{"Id":"int-009","Type":"Dashboard","Value":"Deska aluminium","Description":"Wyko\\u0144czenie aluminium szczotkowane","AdditionalPrice":2000.00},{"Id":"int-044","Type":"Glass","Value":"Szklany dach panoramiczny","Description":"Dwucz\\u0119\\u015Bciowy dach szklany","AdditionalPrice":3500.00},{"Id":"int-034","Type":"Headliner","Value":"Podsufitka Anthrazit","Description":"Ciemna podsufitka BMW Individual","AdditionalPrice":1500.00},{"Id":"int-026","Type":"Lighting","Value":"O\\u015Bwietlenie ambientowe","Description":"Podstawowe o\\u015Bwietlenie LED","AdditionalPrice":500.00},{"Id":"int-020","Type":"Multimedia","Value":"BMW Gesture Control","Description":"Sterowanie gestami","AdditionalPrice":1500.00},{"Id":"int-006","Type":"Seats","Value":"BMW Individual Merino","Description":"Najwy\\u017Cszej klasy sk\\u00F3ra z rozszerzonym zakresem","AdditionalPrice":18000.00}]	336100.00	2025-06-13 12:15:54.013616+00	2025-06-13 12:27:27.578214+00	f
29	3	BMW M3 - 13.06.2025	5	BMW M3	8	M Competition	#008000	Zielony	[{"Id":"acc-003","Name":"Felgi zimowe 18\\u0022","Price":5000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	481600.00	2025-06-13 12:39:35.544386+00	\N	t
30	3	BMW M3 - 13.06.2025	5	BMW M3	8	M Competition	#008000	Zielony	[{"Id":"acc-003","Name":"Felgi zimowe 18\\u0022","Price":5000.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"},{"Id":"acc-033","Name":"Kabel Mode 3","Price":800.00,"Type":"ChargingCable"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	490400.00	2025-06-13 12:39:44.303424+00	2025-06-13 12:39:55.747714+00	f
33	3	BMW X3 - 13.06.2025	1	BMW X3	2	M40i xDrive	#000000	Czarny	[{"Id":"acc-001","Name":"Felgi M Performance 20\\u0022","Price":8000.00,"Type":"AlloyWheels"},{"Id":"acc-004","Name":"Dywaniki welurowe M","Price":800.00,"Type":"FloorMats"}]	[{"Id":"int-046","Type":"Additional","Value":"Ambient Air","Description":"Aromatyzacja wn\\u0119trza","AdditionalPrice":1000.00}]	314799.00	2025-06-13 17:09:39.371114+00	\N	t
28	3	BMW i4 - 13.06.2025	3	BMW i4	5	eDrive40	#000080	Granatowy	[{"Id":"acc-026","Name":"Felgi Turbine 18\\u0022","Price":5500.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00}]	339600.00	2025-06-13 12:27:57.460697+00	2025-06-13 17:10:03.135908+00	f
31	3	BMW Series 3 - 13.06.2025	2	BMW Series 3	3	320i	#0000ff	Niebieski	[{"Id":"acc-026","Name":"Felgi Turbine 18\\u0022","Price":5500.00,"Type":"AlloyWheels"},{"Id":"acc-031","Name":"Dywaniki luksusowe","Price":1200.00,"Type":"FloorMats"},{"Id":"acc-032","Name":"Tylne skrzyd\\u0142o M Performance","Price":8000.00,"Type":"Spoiler"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00}]	210100.00	2025-06-13 12:59:38.251114+00	2025-06-13 17:10:05.991058+00	f
34	3	BMW X3 - 13.06.2025	1	BMW X3	1	20i xDrive	#000000	Czarny	[{"Id":"acc-001","Name":"Felgi M Performance 20\\u0022","Price":8000.00,"Type":"AlloyWheels"},{"Id":"acc-004","Name":"Dywaniki welurowe M","Price":800.00,"Type":"FloorMats"}]	[{"Id":"int-046","Type":"Additional","Value":"Ambient Air","Description":"Aromatyzacja wn\\u0119trza","AdditionalPrice":1000.00}]	264799.00	2025-06-13 17:09:54.811773+00	2025-06-13 17:10:08.150044+00	f
32	3	tomasz	1	BMW X3	1	20i xDrive	#ff0000	Czerwony	[{"Id":"acc-026","Name":"Felgi Turbine 18\\u0022","Price":5500.00,"Type":"AlloyWheels"},{"Id":"acc-004","Name":"Dywaniki welurowe M","Price":800.00,"Type":"FloorMats"},{"Id":"acc-006","Name":"Tylny spoiler M Performance","Price":3500.00,"Type":"Spoiler"},{"Id":"acc-008","Name":"Kabel \\u0142adowania Wallbox","Price":2000.00,"Type":"ChargingCable"},{"Id":"acc-009","Name":"BMW Wallbox Plus","Price":5000.00,"Type":"ChargingStation"}]	[{"Id":"int-049","Type":"Additional","Value":"Gniazda 230V","Description":"Gniazda elektryczne z ty\\u0142u","AdditionalPrice":400.00},{"Id":"int-025","Type":"Audio","Value":"Bowers \\u0026 Wilkins 4D","Description":"System audio 4D z wibracjami w fotelach","AdditionalPrice":12000.00},{"Id":"int-040","Type":"Climate","Value":"Klimatyzacja 3-strefowa","Description":"Z dodatkow\\u0105 stref\\u0105 dla pasa\\u017Cer\\u00F3w z ty\\u0142u","AdditionalPrice":1500.00}]	295700.00	2025-06-13 13:06:06.499505+00	2025-06-13 17:10:13.737823+00	f
\.


--
-- TOC entry 3520 (class 0 OID 16398)
-- Dependencies: 218
-- Data for Name: uzytkownik; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.uzytkownik (id, imienazwisko, email, haslo, refreshtoken, refreshtokenexpires, rola) FROM stdin;
1	Jan Kowalski	test@example.com	$2a$11$rBr6M8cnqH8c4I7gQUfmz.Qa1qITl5dSl0hGCiI1pGYyJQbXpPvG6	\N	\N	Uzytkownik
2	Admin BMW	admin@bmw.com	$2a$11$rBr6M8cnqH8c4I7gQUfmz.Qa1qITl5dSl0hGCiI1pGYyJQbXpPvG6	\N	\N	Administrator
3	Kacper Admin	kacper.kosal@op.pl	$2a$11$BUbG5wBdXLmkHLyxx6NhqO9k2CAMvZuOD8ArwB8aYq9tkTrLw91dy	KY64m//xhqinjFqGP/tu7ardQOTZ7sRp5GtrUilU/vc=	2025-06-28 15:36:13.032568	Administrator
\.


--
-- TOC entry 3564 (class 0 OID 0)
-- Dependencies: 227
-- Name: Silnik_ID_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public."Silnik_ID_seq"', 9, true);


--
-- TOC entry 3565 (class 0 OID 0)
-- Dependencies: 243
-- Name: administrator_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.administrator_id_seq', 1, false);


--
-- TOC entry 3566 (class 0 OID 0)
-- Dependencies: 231
-- Name: cechypojazdu_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.cechypojazdu_id_seq', 24, true);


--
-- TOC entry 3567 (class 0 OID 0)
-- Dependencies: 233
-- Name: elementwyposazenia_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.elementwyposazenia_id_seq', 1, false);


--
-- TOC entry 3568 (class 0 OID 0)
-- Dependencies: 235
-- Name: konfiguracja_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.konfiguracja_id_seq', 1, false);


--
-- TOC entry 3569 (class 0 OID 0)
-- Dependencies: 239
-- Name: logowanie_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.logowanie_id_seq', 1, false);


--
-- TOC entry 3570 (class 0 OID 0)
-- Dependencies: 229
-- Name: modelsilnik_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.modelsilnik_id_seq', 8, true);


--
-- TOC entry 3571 (class 0 OID 0)
-- Dependencies: 219
-- Name: pojazd_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.pojazd_id_seq', 6, true);


--
-- TOC entry 3572 (class 0 OID 0)
-- Dependencies: 241
-- Name: rejestracja_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.rejestracja_id_seq', 1, false);


--
-- TOC entry 3573 (class 0 OID 0)
-- Dependencies: 237
-- Name: udostepnieniekonfiguracji_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.udostepnieniekonfiguracji_id_seq', 1, false);


--
-- TOC entry 3574 (class 0 OID 0)
-- Dependencies: 221
-- Name: user_configurations_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.user_configurations_id_seq', 34, true);


--
-- TOC entry 3575 (class 0 OID 0)
-- Dependencies: 217
-- Name: uzytkownik_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.uzytkownik_id_seq', 3, true);


--
-- TOC entry 3346 (class 2606 OID 16477)
-- Name: Silnik Silnik_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."Silnik"
    ADD CONSTRAINT "Silnik_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 3362 (class 2606 OID 16588)
-- Name: administrator administrator_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.administrator
    ADD CONSTRAINT administrator_pkey PRIMARY KEY (id);


--
-- TOC entry 3337 (class 2606 OID 16467)
-- Name: car_accessories car_accessories_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.car_accessories
    ADD CONSTRAINT car_accessories_pkey PRIMARY KEY (id);


--
-- TOC entry 3335 (class 2606 OID 16460)
-- Name: car_interior_equipment car_interior_equipment_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.car_interior_equipment
    ADD CONSTRAINT car_interior_equipment_pkey PRIMARY KEY (id);


--
-- TOC entry 3324 (class 2606 OID 16437)
-- Name: car_model_colors car_model_colors_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.car_model_colors
    ADD CONSTRAINT car_model_colors_pkey PRIMARY KEY ("Id");


--
-- TOC entry 3350 (class 2606 OID 16507)
-- Name: cechypojazdu cechypojazdu_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.cechypojazdu
    ADD CONSTRAINT cechypojazdu_pkey PRIMARY KEY (id);


--
-- TOC entry 3352 (class 2606 OID 16521)
-- Name: elementwyposazenia elementwyposazenia_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.elementwyposazenia
    ADD CONSTRAINT elementwyposazenia_pkey PRIMARY KEY (id);


--
-- TOC entry 3354 (class 2606 OID 16530)
-- Name: konfiguracja konfiguracja_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.konfiguracja
    ADD CONSTRAINT konfiguracja_pkey PRIMARY KEY (id);


--
-- TOC entry 3358 (class 2606 OID 16564)
-- Name: logowanie logowanie_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.logowanie
    ADD CONSTRAINT logowanie_pkey PRIMARY KEY (id);


--
-- TOC entry 3348 (class 2606 OID 16490)
-- Name: modelsilnik modelsilnik_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.modelsilnik
    ADD CONSTRAINT modelsilnik_pkey PRIMARY KEY (id);


--
-- TOC entry 3318 (class 2606 OID 16415)
-- Name: pojazd pojazd_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.pojazd
    ADD CONSTRAINT pojazd_pkey PRIMARY KEY (id);


--
-- TOC entry 3333 (class 2606 OID 16450)
-- Name: pojazd_zdjecie pojazd_zdjecie_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.pojazd_zdjecie
    ADD CONSTRAINT pojazd_zdjecie_pkey PRIMARY KEY ("Id");


--
-- TOC entry 3360 (class 2606 OID 16576)
-- Name: rejestracja rejestracja_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.rejestracja
    ADD CONSTRAINT rejestracja_pkey PRIMARY KEY (id);


--
-- TOC entry 3356 (class 2606 OID 16552)
-- Name: udostepnieniekonfiguracji udostepnieniekonfiguracji_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.udostepnieniekonfiguracji
    ADD CONSTRAINT udostepnieniekonfiguracji_pkey PRIMARY KEY (id);


--
-- TOC entry 3328 (class 2606 OID 16439)
-- Name: car_model_colors unique_car_model_color; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.car_model_colors
    ADD CONSTRAINT unique_car_model_color UNIQUE ("CarModelId", "ColorName");


--
-- TOC entry 3322 (class 2606 OID 16427)
-- Name: user_configurations user_configurations_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.user_configurations
    ADD CONSTRAINT user_configurations_pkey PRIMARY KEY (id);


--
-- TOC entry 3316 (class 2606 OID 16405)
-- Name: uzytkownik uzytkownik_pkey; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.uzytkownik
    ADD CONSTRAINT uzytkownik_pkey PRIMARY KEY (id);


--
-- TOC entry 3338 (class 1259 OID 16601)
-- Name: idx_car_accessories_car_id; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_accessories_car_id ON public.car_accessories USING btree (car_id);


--
-- TOC entry 3339 (class 1259 OID 16602)
-- Name: idx_car_accessories_car_model; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_accessories_car_model ON public.car_accessories USING btree (car_model);


--
-- TOC entry 3340 (class 1259 OID 16603)
-- Name: idx_car_accessories_category; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_accessories_category ON public.car_accessories USING btree (category);


--
-- TOC entry 3341 (class 1259 OID 16607)
-- Name: idx_car_accessories_is_in_stock; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_accessories_is_in_stock ON public.car_accessories USING btree (is_in_stock);


--
-- TOC entry 3342 (class 1259 OID 16606)
-- Name: idx_car_accessories_is_original_bmw_part; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_accessories_is_original_bmw_part ON public.car_accessories USING btree (is_original_bmw_part);


--
-- TOC entry 3343 (class 1259 OID 16605)
-- Name: idx_car_accessories_price; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_accessories_price ON public.car_accessories USING btree (price);


--
-- TOC entry 3344 (class 1259 OID 16604)
-- Name: idx_car_accessories_type; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_accessories_type ON public.car_accessories USING btree (type);


--
-- TOC entry 3325 (class 1259 OID 16596)
-- Name: idx_car_model_colors_carmodelid; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_model_colors_carmodelid ON public.car_model_colors USING btree ("CarModelId");


--
-- TOC entry 3326 (class 1259 OID 16597)
-- Name: idx_car_model_colors_colorname; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_car_model_colors_colorname ON public.car_model_colors USING btree ("ColorName");


--
-- TOC entry 3329 (class 1259 OID 16598)
-- Name: idx_pojazd_zdjecie_carmodelid; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_pojazd_zdjecie_carmodelid ON public.pojazd_zdjecie USING btree ("CarModelId");


--
-- TOC entry 3330 (class 1259 OID 16600)
-- Name: idx_pojazd_zdjecie_color; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_pojazd_zdjecie_color ON public.pojazd_zdjecie USING btree ("Color");


--
-- TOC entry 3331 (class 1259 OID 16599)
-- Name: idx_pojazd_zdjecie_displayorder; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_pojazd_zdjecie_displayorder ON public.pojazd_zdjecie USING btree ("DisplayOrder");


--
-- TOC entry 3319 (class 1259 OID 16595)
-- Name: idx_user_configurations_created_at; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_user_configurations_created_at ON public.user_configurations USING btree (created_at);


--
-- TOC entry 3320 (class 1259 OID 16594)
-- Name: idx_user_configurations_user_id; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX idx_user_configurations_user_id ON public.user_configurations USING btree (user_id);


--
-- TOC entry 3363 (class 2606 OID 16478)
-- Name: Silnik Silnik_PojazdID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."Silnik"
    ADD CONSTRAINT "Silnik_PojazdID_fkey" FOREIGN KEY ("PojazdID") REFERENCES public.pojazd(id);


--
-- TOC entry 3373 (class 2606 OID 16589)
-- Name: administrator administrator_iduzytkownika_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.administrator
    ADD CONSTRAINT administrator_iduzytkownika_fkey FOREIGN KEY (iduzytkownika) REFERENCES public.uzytkownik(id);


--
-- TOC entry 3366 (class 2606 OID 16508)
-- Name: cechypojazdu cechypojazdu_idpojazdu_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.cechypojazdu
    ADD CONSTRAINT cechypojazdu_idpojazdu_fkey FOREIGN KEY (idpojazdu) REFERENCES public.pojazd(id);


--
-- TOC entry 3367 (class 2606 OID 16536)
-- Name: konfiguracja konfiguracja_idpojazdu_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.konfiguracja
    ADD CONSTRAINT konfiguracja_idpojazdu_fkey FOREIGN KEY (idpojazdu) REFERENCES public.pojazd(id);


--
-- TOC entry 3368 (class 2606 OID 16531)
-- Name: konfiguracja konfiguracja_iduzytkownika_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.konfiguracja
    ADD CONSTRAINT konfiguracja_iduzytkownika_fkey FOREIGN KEY (iduzytkownika) REFERENCES public.uzytkownik(id);


--
-- TOC entry 3369 (class 2606 OID 16541)
-- Name: konfiguracja konfiguracja_modelsilnikid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.konfiguracja
    ADD CONSTRAINT konfiguracja_modelsilnikid_fkey FOREIGN KEY (modelsilnikid) REFERENCES public.modelsilnik(id);


--
-- TOC entry 3371 (class 2606 OID 16565)
-- Name: logowanie logowanie_iduzytkownika_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.logowanie
    ADD CONSTRAINT logowanie_iduzytkownika_fkey FOREIGN KEY (iduzytkownika) REFERENCES public.uzytkownik(id);


--
-- TOC entry 3364 (class 2606 OID 16491)
-- Name: modelsilnik modelsilnik_modelid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.modelsilnik
    ADD CONSTRAINT modelsilnik_modelid_fkey FOREIGN KEY (modelid) REFERENCES public.pojazd(id);


--
-- TOC entry 3365 (class 2606 OID 16496)
-- Name: modelsilnik modelsilnik_silnikid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.modelsilnik
    ADD CONSTRAINT modelsilnik_silnikid_fkey FOREIGN KEY (silnikid) REFERENCES public."Silnik"("ID");


--
-- TOC entry 3372 (class 2606 OID 16577)
-- Name: rejestracja rejestracja_iduzytkownika_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.rejestracja
    ADD CONSTRAINT rejestracja_iduzytkownika_fkey FOREIGN KEY (iduzytkownika) REFERENCES public.uzytkownik(id);


--
-- TOC entry 3370 (class 2606 OID 16553)
-- Name: udostepnieniekonfiguracji udostepnieniekonfiguracji_idkonfiguracji_fkey; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.udostepnieniekonfiguracji
    ADD CONSTRAINT udostepnieniekonfiguracji_idkonfiguracji_fkey FOREIGN KEY (idkonfiguracji) REFERENCES public.konfiguracja(id);


-- Completed on 2025-06-13 19:22:07

--
-- PostgreSQL database dump complete
--

BEGIN;

-- 1) Silnik → pojazd
ALTER TABLE public."Silnik"
  DROP CONSTRAINT IF EXISTS "Silnik_PojazdID_fkey",
  ADD CONSTRAINT "Silnik_PojazdID_fkey"
    FOREIGN KEY ("PojazdID")
    REFERENCES public.pojazd(id)
    ON DELETE CASCADE;

-- 2) cechypojazdu → pojazd
ALTER TABLE public.cechypojazdu
  DROP CONSTRAINT IF EXISTS cechypojazdu_idpojazdu_fkey,
  ADD CONSTRAINT cechypojazdu_idpojazdu_fkey
    FOREIGN KEY (idpojazdu)
    REFERENCES public.pojazd(id)
    ON DELETE CASCADE;

-- 3) konfiguracja → pojazd
ALTER TABLE public.konfiguracja
  DROP CONSTRAINT IF EXISTS konfiguracja_idpojazdu_fkey,
  ADD CONSTRAINT konfiguracja_idpojazdu_fkey
    FOREIGN KEY (idpojazdu)
    REFERENCES public.pojazd(id)
    ON DELETE CASCADE;

-- 4) modelsilnik → pojazd
ALTER TABLE public.modelsilnik
  DROP CONSTRAINT IF EXISTS modelsilnik_modelid_fkey,
  ADD CONSTRAINT modelsilnik_modelid_fkey
    FOREIGN KEY (modelid)
    REFERENCES public.pojazd(id)
    ON DELETE CASCADE;

COMMIT;


