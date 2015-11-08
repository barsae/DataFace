
# DataFace: Easy Database Interfacing for C#

DataFace helps your C# project have a clear and easy to manage database access layer.

### Example

```C#
using System;
using System.Collections.Generic
using DataFace.Core;
using DataFace.SqlServer;

public class MyRepository : BaseRepository {
	public MyRepository() : base(new SqlServerDatabaseConnection("myConnectionString")) {
    }
    
    public int ScalarExample(int sprocParameter) {
        return ExecuteStoredProcedure(new object[] { sprocParameter }).ToScalar<int>();
    }
    
    public List<RowModel> RowsExample() {
        return ExecuteStoredProcedure(new object[] {}).ToRows<RowModel>();
    }
    
    public MultipleResultSetModel MultipleResultSetExample() {
        return ExecuteStoredProcedure(new object[] {}).ToMultipleResultSetModel<MultipleResultSetModel>();
    }
}

public class RowModel {
	public string ExampleDataColumn { get; set; }
}

public class MultipleResultSetModel {
	[ResultSet(0, ResultSetType.Rows)]
	public List<RowModel> ExampleResultSet { get; set; }
}
```

In the above example, ```ScalarExample```, ```RowsExample```, and ```MultipleResultSetExample``` are stored procedures in a SQL Server database.  DataFace uses reflection to find the name of the stored procedures and parameters declared in your repository class.  It then provides helper methods for processing the results to scalar, a single row, many rows, or even multiple result sets.

### Usage

* Install DataFace as a [nuget package](https://www.nuget.org/packages/DataFace/) onto your solution.
* Create a repository model inheriting from DataFace.Core.BaseRepository
* In the constructor, choose which IDatabaseConnection to use (only SQL Server is supported for now)
* Fill out your model with stored procedures as methods of the repository.  Use ToRows, ToSingleRow, and other helper methods to process your database results.
* Write code that uses your new database access layer!

