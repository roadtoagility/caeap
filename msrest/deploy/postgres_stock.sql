-- Create the schema that we'll use to populate data and watch the effect in the binlog
CREATE SCHEMA IF NOT EXISTS stock;

SET
    search_path TO stock;

-- enable uuidv4
create extension IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS audit
(
    id          UUID NOT NULL PRIMARY KEY,
    is_deleted  BOOLEAN NOT NULL,
    row_version INT NOT NULL,
    create_at   BIGINT NOT NULL,
    update_at   BIGINT NOT NULL
);


-- Create and populate our products using a single insert with many rows
-- Create and populate our products using a single insert with many rows
CREATE TABLE products
(
    id          uuid NOT NULL PRIMARY KEY,
    name        VARCHAR(255) NOT NULL,
    description VARCHAR(512),
    weight      FLOAT NOT NULL,
    price       DECIMAL NOT NULL,
    is_deleted  BOOLEAN NOT NULL,
    row_version BYTEA
);

INSERT INTO products
VALUES ('09ed82f6-4469-40ec-b601-943c7c848d6b', 'scooter', 'Small 2-wheel scooter', 3.14, 12,false, '\x01000000'),
       ('72234aa3-a1d6-483f-821a-ec541e045372', 'car battery', '12V car battery', 8.1, 1,false, '\x01000000'),
       ('c254e819-7976-4f37-bd86-e2a8a14c3762', '12-pack drill bits','12-pack of drill bits with sizes ranging from #40 to #3', 0.8, 156,false, '\x01000000'),
       ('5fcb33ee-7592-4e14-8473-69329ada6d81', 'hammer', '12oz carpenter''s hammer', 0.75, 34,false, '\x01000000'),
       ('d4a8d6ad-6680-491a-ac64-d7b48da522b3', 'hammer', '14oz carpenter''s hammer', 0.875, 43.3, false, '\x01000000'),
       ('5db3e6c6-ad06-455c-a537-fb80f66777d3', 'hammer', '16oz carpenter''s hammer', 1.0, 2.3, false, '\x01000000'),
       ('fde79f2c-ee82-4f76-a77f-0e0a033475c7', 'rocks', 'box of assorted rocks', 5.3, 1.2, false, '\x01000000'),
       ('441e2178-ef95-4c81-b043-8d5156a9f035', 'jacket', 'water resistent black wind breaker', 0.1, 1.0, false, '\x01000000'),
       ('e33a923b-48a2-45b4-ad32-9a2b436ce1bd', 'spare tire', '24 inch spare tire', 22.2, 1, false, '\x01000000');

-- INSERT INTO products
-- VALUES ('09ed82f6-4469-40ec-b601-943c7c848d6b', false,1,1687144819,1687144819,'09ed82f6-4469-40ec-b601-943c7c848d6b','scooter', 'Small 2-wheel scooter', 3.14,1),
--        ('72234aa3-a1d6-483f-821a-ec541e045372', false,1,1687144819,1687144819,'72234aa3-a1d6-483f-821a-ec541e045372','car battery', '12V car battery', 8.1, 1),
--        ('c254e819-7976-4f37-bd86-e2a8a14c3762', false,1,1687144819,1687144819,'c254e819-7976-4f37-bd86-e2a8a14c3762', '12-pack drill bits','12-pack of drill bits with sizes ranging from #40 to #3', 0.8,1),
--        ('5fcb33ee-7592-4e14-8473-69329ada6d81', false,1,1687144819,1687144819,'5fcb33ee-7592-4e14-8473-69329ada6d81','hammer', '12oz carpenter''s hammer', 0.75,1),
--        ('d4a8d6ad-6680-491a-ac64-d7b48da522b3', false,1,1687144819,1687144819,'d4a8d6ad-6680-491a-ac64-d7b48da522b3','hammer', '14oz carpenter''s hammer', 0.875,1),
--        ('5db3e6c6-ad06-455c-a537-fb80f66777d3', false,1,1687144819,1687144819,'5db3e6c6-ad06-455c-a537-fb80f66777d3','hammer', '16oz carpenter''s hammer', 1.0,1),
--        ('fde79f2c-ee82-4f76-a77f-0e0a033475c7', false,1,1687144819,1687144819,'fde79f2c-ee82-4f76-a77f-0e0a033475c7','rocks', 'box of assorted rocks', 5.3,1),
--        ('441e2178-ef95-4c81-b043-8d5156a9f035', false,1,1687144819,1687144819,'441e2178-ef95-4c81-b043-8d5156a9f035','jacket', 'water resistent black wind breaker', 0.1,1),
--        ('e33a923b-48a2-45b4-ad32-9a2b436ce1bd', false,1,1687144819,1687144819,'e33a923b-48a2-45b4-ad32-9a2b436ce1bd','spare tire', '24 inch spare tire', 22.2,1);
