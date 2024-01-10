-- Create the schema that we'll use to populate data and watch the effect in the binlog
CREATE SCHEMA ecommerce_querying;

SET
    search_path TO ecommerce_querying;

-- enable PostGis 
-- CREATE
--     EXTENSION postgis;

-- enable uuidv4
create extension "uuid-ossp";

-- Create and populate our products using a single insert with many rows
CREATE TABLE products
(
    id          uuid         NOT NULL PRIMARY KEY,
    name        VARCHAR(255) NOT NULL,
    description VARCHAR(512),
    weight      FLOAT,
    is_deleted BOOLEAN NOT NULL
);

create unique index products_uindex on products (name);

ALTER TABLE products
    ADD COLUMN products_index_col tsvector
        GENERATED ALWAYS AS (to_tsvector('english', coalesce(name, '') || ' ' || coalesce(description, ''))) STORED;
