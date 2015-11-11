
IF OBJECT_ID('ToScalar') IS NOT NULL DROP PROCEDURE ToScalar;
GO

CREATE PROCEDURE ToScalar
AS BEGIN
    SELECT Value = 142
END
GO

IF OBJECT_ID('ToMultipleResultSetModel') IS NOT NULL DROP PROCEDURE ToMultipleResultSetModel;
GO

CREATE PROCEDURE ToMultipleResultSetModel
AS BEGIN
    SELECT IntValue = 147, BoolValue = 1
    UNION ALL
    SELECT 192, 0

    SELECT StringValue = 'abc', DateTimeValue = CAST('2001-02-03' AS DATETIME)
    UNION ALL
    SELECT 'def', CAST('2004-05-06' AS DATETIME)
END
GO

IF OBJECT_ID('ToMultipleResultSetModel') IS NOT NULL DROP PROCEDURE SprocWithParameter;
GO

CREATE PROCEDURE SprocWithParameter
    @Parameter INT
AS BEGIN
    SELECT Parameter = @Parameter
END
GO

IF OBJECT_ID('CountOfSideEffects') IS NOT NULL DROP TABLE CountOfSideEffects;
GO

CREATE TABLE CountOfSideEffects (
    CountOfSideEffects INT NOT NULL
);
GO

INSERT INTO CountOfSideEffects VALUES (0);
GO

IF OBJECT_ID('SprocWithSideEffect') IS NOT NULL DROP PROCEDURE SprocWithSideEffect;
GO

CREATE PROCEDURE SprocWithSideEffect
AS BEGIN
    UPDATE CountOfSideEffects
    SET CountOfSideEffects = CountOfSideEffects + 1
END
GO

IF OBJECT_ID('GetCountOfSideEffects') IS NOT NULL DROP PROCEDURE GetCountOfSideEffects;
GO

CREATE PROCEDURE GetCountOfSideEffects
AS BEGIN
    SELECT CountOfSideEffects
    FROM CountOfSideEffects
END
GO

IF OBJECT_ID('testschema.SprocWithSchema') IS NOT NULL DROP PROCEDURE testschema.SprocWithSchema;
GO

IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'testschema') DROP SCHEMA testschema;
GO

CREATE SCHEMA testschema;
GO

CREATE PROCEDURE testschema.SprocWithSchema
AS BEGIN
    SELECT 123
END
GO
