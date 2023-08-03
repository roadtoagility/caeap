-- Create the schema that we'll use to populate data and watch the effect in the binlog
CREATE SCHEMA stock;
SET
    search_path TO stock;

-- enable PostGis 
CREATE
    EXTENSION postgis;

-- enable uuidv4
create extension "uuid-ossp";

CREATE TABLE IF NOT EXISTS stock.products_outbox
(
    id uuid NOT NULL,
    aggregate_id uuid NOT NULL,
    aggregation_type character varying(255) NOT NULL,
    event_type character varying(255) NOT NULL,
    event_data jsonb NOT NULL,
    event_time timestamp with time zone NOT NULL,
    CONSTRAINT products_outbox_pkey PRIMARY KEY (id)
);
-- this is not necessary because do we use the event source style and we are interest just in the insertion logs 
--ALTER TABLE products_on_hand REPLICA IDENTITY FULL;

CREATE PUBLICATION products_outbox_pub FOR TABLE products_outbox WITH (publish = 'insert');
--ALTER PUBLICATION products_outbox_pub ADD/DROP TABLE teble-name
--ALTER PUBLICATION products_outbox_pub SET (publish = 'insert');

-- SELECT EXISTS( SELCT 1 from pg_replication_slots where slot_name = @slotName);
-- create persisten slot to store current position of data stream
SELECT * FROM pg_create_logical_replication_slot('products_outbox_slot', 'pgoutput');

-- Create and populate our products using a single insert with many rows
CREATE TABLE products
(
    id          uuid         NOT NULL PRIMARY KEY,
    name        VARCHAR(255) NOT NULL,
    description VARCHAR(512),
    weight      FLOAT,
    is_deleted BOOLEAN NOT NULL,
    row_version BYTEA
);

INSERT INTO products
VALUES ('09ed82f6-4469-40ec-b601-943c7c848d6b', 'scooter', 'Small 2-wheel scooter', 3.14, false, '\x01000000'),
       ('72234aa3-a1d6-483f-821a-ec541e045372', 'car battery', '12V car battery', 8.1, false, '\x01000000'),
       ('c254e819-7976-4f37-bd86-e2a8a14c3762', '12-pack drill bits','12-pack of drill bits with sizes ranging from #40 to #3', 0.8, false, '\x01000000'),
       ('5fcb33ee-7592-4e14-8473-69329ada6d81', 'hammer', '12oz carpenter''s hammer', 0.75, false, '\x01000000'),
       ('d4a8d6ad-6680-491a-ac64-d7b48da522b3', 'hammer', '14oz carpenter''s hammer', 0.875, false, '\x01000000'),
       ('5db3e6c6-ad06-455c-a537-fb80f66777d3', 'hammer', '16oz carpenter''s hammer', 1.0, false, '\x01000000'),
       ('fde79f2c-ee82-4f76-a77f-0e0a033475c7', 'rocks', 'box of assorted rocks', 5.3, false, '\x01000000'),
       ('441e2178-ef95-4c81-b043-8d5156a9f035', 'jacket', 'water resistent black wind breaker', 0.1, false, '\x01000000'),
       ('e33a923b-48a2-45b4-ad32-9a2b436ce1bd', 'spare tire', '24 inch spare tire', 22.2, false, '\x01000000');

-- Create and populate the products on hand using multiple inserts
CREATE TABLE products_on_hand
(
    product_id uuid    NOT NULL PRIMARY KEY,
    quantity   INTEGER NOT NULL,
    FOREIGN KEY (product_id) REFERENCES products (id)
);

INSERT INTO products_on_hand
VALUES ('09ed82f6-4469-40ec-b601-943c7c848d6b', 3);
INSERT INTO products_on_hand
VALUES ('72234aa3-a1d6-483f-821a-ec541e045372', 8);
INSERT INTO products_on_hand
VALUES ('c254e819-7976-4f37-bd86-e2a8a14c3762', 18);
INSERT INTO products_on_hand
VALUES ('5fcb33ee-7592-4e14-8473-69329ada6d81', 4);
INSERT INTO products_on_hand
VALUES ('d4a8d6ad-6680-491a-ac64-d7b48da522b3', 5);
INSERT INTO products_on_hand
VALUES ('5db3e6c6-ad06-455c-a537-fb80f66777d3', 0);
INSERT INTO products_on_hand
VALUES ('fde79f2c-ee82-4f76-a77f-0e0a033475c7', 44);
INSERT INTO products_on_hand
VALUES ('441e2178-ef95-4c81-b043-8d5156a9f035', 2);
INSERT INTO products_on_hand
VALUES ('e33a923b-48a2-45b4-ad32-9a2b436ce1bd', 5);

-- Create some customers ...
CREATE TABLE customers
(
    id         uuid         NOT NULL PRIMARY KEY,
    first_name VARCHAR(255) NOT NULL,
    last_name  VARCHAR(255) NOT NULL,
    email      VARCHAR(255) NOT NULL UNIQUE
);

INSERT INTO customers
VALUES ('8bf35186-b69c-4789-8b45-a0e8dcaa1212', 'Sally', 'Thomas', 'sally.thomas@acme.com'),
       ('2bd05da3-1a6b-499c-bfcf-c71f5a660ec0', 'George', 'Bailey', 'gbailey@foobar.com'),
       ('4e00e823-4ab3-4a32-ae69-8a47a9b4c9bd', 'Edward', 'Walker', 'ed@walker.com'),
       ('5d9c16fa-9f7a-444c-be61-804f474588e5', 'Anne', 'Kretchmar', 'annek@noanswer.org');

-- Create some very simple orders
CREATE TABLE orders
(
    id         uuid    NOT NULL PRIMARY KEY,
    order_date DATE    NOT NULL,
    purchaser  uuid    NOT NULL,
    quantity   INTEGER NOT NULL,
    product_id uuid    NOT NULL,
    FOREIGN KEY (purchaser) REFERENCES customers (id),
    FOREIGN KEY (product_id) REFERENCES products (id)
);

INSERT INTO orders
VALUES ('a05e7cf4-2379-4c2f-87cf-d087088aa1de', '2016-01-16', '8bf35186-b69c-4789-8b45-a0e8dcaa1212', 1, '5db3e6c6-ad06-455c-a537-fb80f66777d3'),
       ('d3de8109-eaf1-4af5-85a2-98acd385659d', '2016-01-17', '2bd05da3-1a6b-499c-bfcf-c71f5a660ec0', 2, '441e2178-ef95-4c81-b043-8d5156a9f035'),
       ('8dc516b7-04d3-4ee5-8770-8e789b5582ac', '2016-02-19', '4e00e823-4ab3-4a32-ae69-8a47a9b4c9bd', 2, 'd4a8d6ad-6680-491a-ac64-d7b48da522b3'),
       ('3d9b0048-3b53-475f-add7-c0286c28ccb2', '2016-02-21', '5d9c16fa-9f7a-444c-be61-804f474588e5', 1, '72234aa3-a1d6-483f-821a-ec541e045372');

-- Create table with Spatial/Geometry type
CREATE TABLE geom
(
    id uuid     NOT NULL PRIMARY KEY,
    g  GEOMETRY NOT NULL,
    h  GEOMETRY
);

INSERT INTO geom
VALUES ('511a0563-fa63-4698-a9c2-fa3dfac23ba1', ST_GeomFromText('POINT(1 1)')),
       ('8cabfe03-9500-4f8c-a6dc-c89847af92fb', ST_GeomFromText('LINESTRING(2 1, 6 6)')),
       ('28db3095-204a-446c-b041-c43960dde815', ST_GeomFromText('POLYGON((0 5, 2 5, 2 7, 0 7, 0 5))'));
