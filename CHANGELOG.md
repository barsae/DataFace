
* 2.5.0.0: Nulls interpreted as strings no longer cause an exception.  They now convert to (string)null.
* 2.0.0.0: Breaking change, ExecuteStoredProcedure(new object[] { parameter }) is no longer supported.  Use ExecuteStoredProcedure(inputModel) instead.

