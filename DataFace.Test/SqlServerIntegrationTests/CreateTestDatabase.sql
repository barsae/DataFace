
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
