# DataWall - DataMap Parsing Guide
This guide outlines the file format used for the DataMap data files as well as the types of data that are interpreted from these files. In addition, this guide features an in-depth walkthrough of how the DataMap parses and stores data during runtime.

## Glossary
__Data Region__
* There are 11 total Data Regions:
	* Arizona
	* California
	* Florida
	* Georgia
	* Illinois
	* New Jersey
	* New York
	* NorthCarolina
	* Texas
	* Washington
	* National

__Data Year__
* There are 8 total Data Years:
	* 2005
	* 2006
	* 2007
	* 2008
	* 2009
	* 2010
	* 2011
	* 2012

__Timeline Data__  
* Timeline Data is a collection of the data from each Data Year for a specific Data Region.

## Global Guidelines
1. All data files parsed during runtime are saved as .csv files.
* There are no column headers or row headers.
* Each column holds data corresponding to a specific Data Year (8 columns total):
* Each row holds data corresponding to a specific Data Region (11 rows total):

## Formatting
### Example File Contents (.csv raw data)
This example file hosts timeline data for three data regions over a three data year duration.

	1,2,3
	4,5,6
	7,8,9

Each file contains two types of data:

1. __Explicit Data__
    * This type of data is retrieved during the parsing process and is directly representative of the file's raw data (i.e. the file's unmodified contents).
    * Example: Total Immigration (Nominal) for Arizona during 2005
2. __Implicit Data__
    * This type of data is calculated after the parsing process as it is reliant on data from multiple files.
    * Example: Growth Data (Percentage) for Florida during 2007

### __Explicit Data__
__Total (From Mexico)__  
Total is a measure of the total immigration from Mexico for each data year for every data region.

_Nominal_

| Data Region | Data Year | Value |
| :--- | :---: | ---: |
| Arizona | 2005 | 1 |
| Arizona | 2006 | 2 |
| Arizona | 2007 | 3 |
| California | 2005 | 4 |
| California | 2006 | 5 |
| California | 2007 | 6 |
| Florida | 2005 | 7 |
| Florida | 2006 | 8 |
| Florida | 2007 | 9 |

_Percentage_

| Data Region | Data Year | Value |
| :--- | :---: | ---: |
| Arizona | 2005 | 2.22 |
| Arizona | 2006 | 4.44 |
| Arizona | 2007 | 6.67 |
| California | 2005 | 8.89 |
| California | 2006 | 11.11 |
| California | 2007 | 13.33 |
| Florida | 2005 | 15.56 |
| Florida | 2006 | 17.78 |
| Florida | 2007 | 20 |

### __Implicit (Calculated) Data__
__Growth__  
Growth is a measure of how much a data region has changed comparative to the previous year.  
Note: Because the first data year does not have a "previous" year, its data will be denoted with an "X" instead.

_Nominal_

| Data Region | Data Year | Value |
| :--- | :---: | ---: |
| Arizona | 2005 | X |
| Arizona | 2006 | 1 |
| Arizona | 2007 | 1 |
| California | 2005 | X |
| California | 2006 | 1 |
| California | 2007 | 1 |
| Florida | 2005 | X |
| Florida | 2006 | 1 |
| Florida | 2007 | 1 |

_Percentage_

| Data Region | Data Year | Value |
| :--- | :---: | ---: |
| Arizona | 2005 | X |
| Arizona | 2006 | 100.00 |
| Arizona | 2007 | 50.00 |
| California | 2005 | X |
| California | 2006 | 25.00 |
| California | 2007 | 20.00 |
| Florida | 2005 | X |
| Florida | 2006 | 14.29 |
| Florida | 2007 | 12.50 |

## Parsing Walkthrough
There are four steps that are followed when the DataMap executes a full parse:
1. Determine filepath for target data
2. Create, send, and await a Unity Web Request to the filepath
3. Filter parsed data to match current filters
4. Populate UI with filtered data

_Note: Most filter combinations require repeating some steps. For example, the filter combination Arizona>Total>Unauthorized technically requires two full parses one parse for each number format (Nominal, Percentage)._

### Determining The Filepath  
Currently, the filepath is determined by adding together the current filters being applied.  

1. Path Root: `https://raw.githubusercontent.com/RGRoland/DataWall/gh-pages/Files/DataWall-Files/DataMap/`
2. Format Filter: `Total`
3. Number Format: `Nominal`
4. Legality Filter: `Unauthorized`
5. File Extension: `.csv`

_Example Filepath:_  
`https://raw.githubusercontent.com/RGRoland/DataWall/gh-pages/Files/DataWall-Files/DataMap/TotalNominalUnauthorized.csv`

### Web Request
After calculating the filepath, we then create a UnityWebRequest.Get request (the web equivalent to a System.IO.StreamReader). 

Let's create a UnityWebRequest using the example filepath we created in the previous step:
```csharp
string filePath = System.IO.Path.Combine(
	"https://raw.githubusercontent.com/RGRoland/DataWall/gh-pages/Files/DataWall-Files/DataMap/",
	FilterFlags.CurrentFilters.Filter_Format.ToString()		// Total
	+ numberFormat 											// Nominal
	+ FilterFlags.CurrentFilters.Filter_Legality.ToString()	// Unauthorized
	+ ".csv");

// www is of type UnityEngine.Networking.UnityWebRequest
var www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
```

With the request configured, we then `yield return` the request which pauses the coroutine finished communicating.
```csharp
yield return www.SendWebRequest()
```

When the request is finished, we then need to access the data we requested by using the request's download handler.
```csharp
string result = www.downloadHandler.text;
```

Now that we have our data, it's time to filter out what we don't need.

_Additional info on the UnityWebRequest API can be found [here][UnityWebRequest API]._

### Data Filtering
Filtering out unwanted data is a fairly simple process since a lot of the filtering is actually done during the filepath determination process.

```csharp
// This code converts our initial data (an array of timeline datas for each data 
// region) to an array of timeline data for a singular data region.
result = result[FilterFlags.CurrentFilters.Filter_State.ToIndex()].Split(',');

// Example:
// Initial data
// 1,2,3
// 4,5,6
// 
// State index = 1
// 
// Filtered data
// 4,5,6
```

_Note: For parses that include an implicit data format, different and/or additional filtering steps are taken before moving on._

### Populate UI
After all the data is filtered, we just input the filtered data and the current number format into the DataParser's UI creation function.

```csharp
UI_AddCategoryBox(result, numberFormat);
```

After all parses are complete, the results are displayed and navigable with the DataMap.

(The complete DataParser code used throughout this guide can be found [here][DataParser.cs].)

[DataParser.cs]: https://github.com/RGRoland/DataWall/blob/gh-pages/DataWall_Unity/Assets/Scripts/DataParser.cs
[UnityWebRequest API]: https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html