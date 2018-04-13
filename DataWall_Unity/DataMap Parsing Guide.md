# DataWall - DataMap Parsing Guide
This guide outlines the current file format used for the DataMap's data files as well as the types of data that are interpreted from the file data. In addition, this guide features an in-depth walkthrough of how the DataMap parses and stores data during runtime.

## Glossary
__Data Region__
* This is an example sentence.  
	
__Data Year__
* This is an example sentence.  
	
__Timeline Data__
* This is an example sentence.  

## Formatting Guide
### Global Guidelines
1. All data files parsed during runtime must be saved as .csv files.

* There are no headers for columns or rows.

* Each column holds data corresponding to a specific data year (8 columns total):
	* 2005 - 2012

* Each row holds data corresponding to a specific data region (11 rows total):
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
	* National (Totals)

### Example File
#### File Contents (.csv raw data)
This file hosts timeline data for three data regions over a three data year duration.

	1,2,3
	4,5,6
	7,8,9

Each file contains two types of data:
* __Explicit Data__
    * This type of data is retrieved during the parsing process and is directly representative of the file's raw data (i.e. no secondary calculations are required).
    * Example: Nominal Data for Arizona during 2005
* __Implicit Data__
    * This type of data is calculated after the parsing process as it is reliant on multiple inputs.
    * Example: Percentage Growth Data for Florida during 2007

#### Explicit (Direct) Data
*Nominal (Filter: by Data Region)*

| Data Year | Data Region | Value |
| :---: | :--- | :---: |
| 2005 | Arizona | 1 |
| 2006 | Arizona | 2 |
| 2007 | Arizona | 3 |
| 2005 | California | 4 |
| 2006 | California | 5 |
| 2007 | California | 6 |
| 2005 | Florida | 7 |
| 2006 | Florida | 8 |
| 2007 | Florida | 9 |

*Nominal (Filter: by Data Year)*

| Data Year | Data Region | Value |
| :---: | :--- | :---: |
| 2005 | Arizona | 1 |
| 2005 | California | 4 |
| 2005 | Florida | 7 |
| 2006 | Arizona | 2 |
| 2006 | California | 5 |
| 2006 | Florida | 8 |
| 2007 | Arizona | 3 |
| 2007 | California | 6 |
| 2007 | Florida | 9 |

#### Implicit (Calculated) Data
*__Growth__*  
Growth is a measure of how much a data region has changed comparative to the previous year. *Note: The first data year (2005) does not have a "previous" year, so growth is listed as "Not Available ('N/A')".*

*Growth(%) (Filter: by Data Region)*

| Data Year | Data Region | Value |
| :---: | :--- | ---: |
| 2005 | Arizona | N/A |
| 2006 | Arizona | 100.0% |
| 2007 | Arizona | 50.0% |
| 2005 | California | N/A |
| 2006 | California | 25.0% |
| 2007 | California | 20.0% |
| 2005 | Florida | N/A |
| 2006 | Florida | 14.3% |
| 2007 | Florida | 12.5% |

*Growth(%) (Filter: by Data Year)*

| Data Year | Data Region | Value |
| :---: | :--- | ---: |
| 2005 | Arizona | N/A |
| 2005 | California | N/A |
| 2005 | Florida | N/A |
| 2006 | Arizona | 100.0% |
| 2006 | California | 25.0% |
| 2006 | Florida | 14.3% |
| 2007 | Arizona | 50.0% |
| 2007 | California | 20.0% |
| 2007 | Florida | 12.5% |