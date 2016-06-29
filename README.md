
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
    
    public int ScalarExample() {
        return ExecuteStoredProcedure().ToScalar<int>();
    }
	   
    public List<RowModel> RowsExample() {
        return ExecuteStoredProcedure().ToRows<RowModel>();
    }
    
    public MultipleResultSetModel MultipleResultSetExample() {
        return ExecuteStoredProcedure().ToMultipleResultSetModel<MultipleResultSetModel>();
    }
}

public class RowModel {
	public string ExampleDataColumn { get; set; }
}

public class MultipleResultSetModel {
	[ResultSet(0, ResultSetType.Rows)]
	public List<RowModel> FirstResultSet { get; set; }

	[ResultSet(0, ResultSetType.Rows)]
	public List<RowModel> SecondResultSet { get; set; }
}

```
In the above example, ```ScalarExample```, ```RowsExample```, and ```MultipleResultSetExample``` are stored procedures in a SQL Server database.  DataFace uses reflection to find the name of the stored procedures and parameters declared in your repository class.  It then provides helper methods for processing the results to scalar, a single row, many rows, or even multiple result sets.

To pass parameters to a stored procedure, pass in an ```InputModel``` (see example below).  DataFace will reflect on the input model type, and produce a parameter for each property within the model.

```

public class MyRepository : BaseRepository {
	public MyRepository() : base(new SqlServerDatabaseConnection("myConnectionString")) {
    }
    
    public int ScalarExample(InputModel input) {
        return ExecuteStoredProcedure(input).ToScalar<int>();
    }
	   
    public List<RowModel> RowsExample(InputModel input) {
        return ExecuteStoredProcedure(input).ToRows<RowModel>();
    }
    
    public MultipleResultSetModel MultipleResultSetExample(InputModel input) {
        return ExecuteStoredProcedure(input).ToMultipleResultSetModel<MultipleResultSetModel>();
    }
}

public class InputModel {
	public int Item1 { get; set; }
	public int Item2 { get; set; }
	public int Item3 { get; set; }
}

```

### Usage

* Install DataFace as a [nuget package](https://www.nuget.org/packages/DataFace/) onto your solution
* Create a repository model inheriting from DataFace.Core.BaseRepository
* In the constructor, choose which IDatabaseConnection to use (only SQL Server is supported for now)
* Fill out your model with stored procedures as methods of the repository.  Use ToRows, ToSingleRow, and other helper methods to process your database results.
* Write code that uses your new database access layer!

