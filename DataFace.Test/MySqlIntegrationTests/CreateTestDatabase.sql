 
DROP PROCEDURE IF EXISTS ToScalar;
GO

CREATE PROCEDURE ToScalar ()
BEGIN
    SELECT 142 AS Value;
END
GO

DROP PROCEDURE IF EXISTS ToFirstOrDefaultScalar;
GO

CREATE PROCEDURE ToFirstOrDefaultScalar ()
BEGIN
    SELECT NULL AS Value;
END
GO

DROP PROCEDURE IF EXISTS ToScalars;
GO

CREATE PROCEDURE ToScalars ()
BEGIN
    SELECT 142 AS Value
    UNION ALL
    SELECT 143
    UNION ALL
    SELECT 144;
END
GO

DROP TABLE IF EXISTS InsertableTable;
GO
CREATE TABLE InsertableTable(
    IdentityColumn INT PRIMARY KEY AUTO_INCREMENT,
    ValueColumn VARCHAR(50)
);
GO

DROP PROCEDURE IF EXISTS InsertRecord;
GO

CREATE PROCEDURE InsertRecord ()
BEGIN
    INSERT INTO InsertableTable (`ValueColumn`) VALUES ('TestValue');

    SELECT LAST_INSERT_ID() AS IdentityColumn;
END
GO

DROP PROCEDURE IF EXISTS ToMultipleResultSetModel;
GO

CREATE PROCEDURE ToMultipleResultSetModel ()
BEGIN
    SELECT 147 AS IntValue, 1 AS BoolValue
    UNION ALL
    SELECT 192, 0;

    SELECT 'abc' AS StringValue, CAST('2001-02-03' AS DATETIME) DateTimeValue
    UNION ALL
    SELECT 'def', CAST('2004-05-06' AS DATETIME);
END
GO

DROP PROCEDURE IF EXISTS SprocWithParameter;
GO

CREATE PROCEDURE SprocWithParameter (
    Parameter INT
)
BEGIN
    SELECT Parameter AS Parameter;
END
GO

DROP TABLE IF EXISTS CountOfSideEffects;
GO

CREATE TABLE CountOfSideEffects (
    CountOfSideEffects INT NOT NULL
);
GO

INSERT INTO CountOfSideEffects VALUES (0);
GO

DROP PROCEDURE IF EXISTS SprocWithSideEffect;
GO

CREATE PROCEDURE SprocWithSideEffect ()
BEGIN
    UPDATE CountOfSideEffects
    SET CountOfSideEffects = CountOfSideEffects + 1;
END
GO

DROP PROCEDURE IF EXISTS GetCountOfSideEffects;
GO

CREATE PROCEDURE GetCountOfSideEffects ()
BEGIN
    SELECT CountOfSideEffects
    FROM CountOfSideEffects;
END
GO
