﻿CREATE TABLE SUBSCRIBER (
    ID_SUBSCRIBER INT NOT NULL PRIMARY KEY CLUSTERED,
    ID_SUBSCRIPTION_PLAN INT NULL,
    EMAIL varchar(255) NOT NULL,
    USER_NAME varchar(255)
);