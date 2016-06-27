
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

Often, a stored procedure will contain a number of input parameters, such as in the ```InputModel``` class below.
While DataFace supports the ability to explicitly list each parameter in the object array input for the ```ExecuteStoredProcedure``` method, it is typically more efficient to use generics, and let DataFace process the input models on your behalf.
This is accomplished through reflection. DataFace reflects on the input model type, and produces a parameter for each property within the model.

```

public class MyRepository : BaseRepository {
	public MyRepository() : base(new SqlServerDatabaseConnection("myConnectionString")) {
    }
    
    public int ScalarExample(InputModel input) {
        return ExecuteStoredProcedure<InputModel>(input).ToScalar<int>();
    }
	   
    public List<RowModel> RowsExample(InputModel input) {
        return ExecuteStoredProcedure<InputModel>(input).ToRows<RowModel>();
    }
    
    public MultipleResultSetModel MultipleResultSetExample(InputModel input) {
        return ExecuteStoredProcedure<InputModel>(input).ToMultipleResultSetModel<MultipleResultSetModel>();
    }
}

public class InputModel {
	public int Item1 {get; set;}
	public int Item2 {get; set;}
	public int Item3 {get; set;}
	public int Item4 {get; set;}
	public int Item5 {get; set;}
	public int Item6 {get; set;}
	public int Item7 {get; set;}
	public int Item8 {get; set;}
}

```

### Usage

* Install DataFace as a [nuget package](https://www.nuget.org/packages/DataFace/) onto your solution
* Create a repository model inheriting from DataFace.Core.BaseRepository
* In the constructor, choose which IDatabaseConnection to use (only SQL Server is supported for now)
* Fill out your model with stored procedures as methods of the repository.  Use ToRows, ToSingleRow, and other helper methods to process your database results.
* Write code that uses your new database access layer!

