

DROP FUNCTION IF EXISTS ToScalar();

CREATE FUNCTION ToScalar()
RETURNS TABLE(Value INT)
AS $$
BEGIN
    RETURN QUERY SELECT 142 AS Value;
END;
$$ LANGUAGE plpgsql;

-- TODO: Multiple result sets in postgresql are over my head
--IF OBJECT_ID('ToMultipleResultSetModel') IS NOT NULL DROP PROCEDURE ToMultipleResultSetModel;
--GO

--CREATE PROCEDURE ToMultipleResultSetModel
--AS BEGIN
--    SELECT IntValue = 147, BoolValue = 1
--    UNION ALL
--    SELECT 192, 0

--    SELECT StringValue = 'abc', DateTimeValue = CAST('2001-02-03' AS DATETIME)
--    UNION ALL
--    SELECT 'def', CAST('2004-05-06' AS DATETIME)
--END
--GO

DROP FUNCTION IF EXISTS SprocWithParameter(INT);

CREATE FUNCTION SprocWithParameter(_Parameter INT)
RETURNS TABLE(Parameter INT)
AS $$
BEGIN
    RETURN QUERY SELECT _Parameter AS Parameter;
END;
$$ LANGUAGE plpgsql;
GO

DROP TABLE IF EXISTS CountOfSideEffects;

CREATE TABLE CountOfSideEffects (
    SideEffects INT NOT NULL
);
GO

INSERT INTO CountOfSideEffects (SideEffects) VALUES (0);
GO

DROP FUNCTION IF EXISTS SprocWithSideEffect();
GO

CREATE FUNCTION SprocWithSideEffect()
RETURNS VOID
AS $$
BEGIN
    UPDATE CountOfSideEffects
    SET SideEffects = SideEffects + 1;
END;
$$ LANGUAGE plpgsql;
GO

DROP FUNCTION IF EXISTS GetCountOfSideEffects();

CREATE FUNCTION GetCountOfSideEffects()
RETURNS TABLE(_SideEffects INT)
AS $$
BEGIN
    RETURN QUERY
    SELECT SideEffects AS _SideEffects
    FROM CountOfSideEffects;
END
$$ LANGUAGE plpgsql
GO

DROP SCHEMA IF EXISTS testschema CASCADE;
GO

CREATE SCHEMA testschema;
GO

CREATE FUNCTION testschema.SprocWithSchema()
RETURNS TABLE(_Value INT)
AS $$
BEGIN
    RETURN QUERY
    SELECT 123 AS _Value;
END;
$$ LANGUAGE plpgsql
GO
