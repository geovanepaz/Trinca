CREATE DATABASE trinca
GO
CREATE DATABASe trinca_log
GO

USE trinca
CREATE TABLE participante (
    id INT IDENTITY(1 , 1) PRIMARY KEY,
    nome_completo VARCHAR(255) NOT NULL,
    apelido VARCHAR(255) NOT NULL ,
    email VARCHAR(255) UNIQUE NOT NULL
);

create table usuario (
    id INT IDENTITY(1 , 1) PRIMARY KEY,
    nome_completo VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    senha VARCHAR(255) NOT NULL
);

CREATE TABLE evento (
    id INT IDENTITY(1 , 1) PRIMARY KEY,
    descricao VARCHAR(255) NOT NULL,
    observacao VARCHAR(500) ,
    data_evento DATE NOT NULL
);

CREATE TABLE valor_participante (
    id INT IDENTITY(1 , 1) PRIMARY KEY,
    id_evento INT ,
id_participante INT,
    valor DECIMAL(4,2) NOT NULL,
    valor_sugerido DECIMAL(4,2),
CONSTRAINT fk_evento_valor_participante FOREIGN KEY (id_evento) REFERENCES evento (id),
CONSTRAINT fk_participante_valor_participante FOREIGN KEY (id_participante) REFERENCES participante (id),
CONSTRAINT combicao_unica UNIQUE (id_evento, id_participante)
);

use TRINCA_LOG
CREATE TABLE log_sistema_churrasco (
    id int IDENTITY(1 , 1) PRIMARY KEY,
application varchar(255),
tracetime datetime,
    loglevel varchar(50),
    message varchar(4000),
machinename varchar(20),
username varchar(30),
exceptionmessage varchar(4000)
);