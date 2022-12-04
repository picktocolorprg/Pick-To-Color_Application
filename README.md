# 2. Pick-To-Color_Database
===============================
==============================
Pick To Color Database Script v1.0
------------------------------------------------------------------------------------------------------------------------------------
The database script will create Pick To Color database in your MS SQL database.
Open `DBScript.sql` file with MS SQL server and run this script.


# 1. Pick-To-Color_WebApplication
===============================
==============================
Pick To Color v1.0
------------------------------------------------------------------------------------------------------------------------------------
Pick To Color is an automation software that is intended to improve the efficiency of the order picking system at warehouses.
The warehouse will be divided into color coded regions to identify the location of products more efficiently. An item can be
located by using the information from the barcode-based invoices. Central logistics approached Ace innovative solutions to 
provide a system to automate their order picking system. Based on the requirements suggested by the central logistics company, 
Pick To Color software is developed to locate an item using a barcode system.

Technologies Used
-------------------------------------------------------------------------------------------------------------------------------------
Backend for this application is designed with ASP.net MVC, frontend with Bootstrap and jQuery. The database chosen is Microsoft SQL.

Requirements
-------------------------------------------------------------------------------------------------------------------------------------
Microsoft SQL, Visual Studio 2019 or above, Pick To Color Windows Service

Configuration Notes
-------------------------------------------------------------------------------------------------------------------------------------
Open the pickToColor.sln file with Visual Studio 2019 and open `Web.config` file. Please replace your database server name in Datasource field.
  <connectionStrings>
    
    <add providerName="System.Data.SqlClient" name="DataContext" connectionString="Data Source=<your database server name>;Initial Catalog=PICK_TO_COLOR;Integrated Security=True" />
  </connectionStrings>

Pick To Color Web Application Default User Credential
-------------------------------------------------------------------------------------------------------------------------------------
Default user credential for PTC web application.
`user name: admin`
`password: admin`

# 2. Pick-To-Color_WindowService
===============================
==============================
Pick To Color Window Service v1.0
------------------------------------------------------------------------------------------------------------------------------------
Pick To COlor Window Service will fetch order information excel files from shared folder and run every xx minutes. 
Then the service will insert order information into Pick To COlor Database. After completed, the service will move the excel file from
shared folder into BackupOrderFiles folder. All system status, information, errors and warnings logs will be stored in Log folder.

Create 3 folders
------------------------------------------------------------------------------------------------------------------------------------
Create 3 folders in your computer.
1. PTCSharedFolder
2. BackupOrderFiles
3. PTCLogFile

Configuration Notes
-------------------------------------------------------------------------------------------------------------------------------------
This window service is 64 bit version.
1. Go to ~\PickToColorService\bin\x64\Release and open `PickToColorService.exe.config` file.
2. Please replace your database server name in Datasource field.

 <connectionStrings>
  
    <add name="SqlCom" connectionString="Data Source=<your database server name>;Initial Catalog=PICK_TO_COLOR;Integrated Security=True"  providerName="System.Data.SqlClient"/>
  </connectionStrings>
 
3. Please set scheduler timer in IntervalMinutes field.
  <appSettings>  
  
      <add key ="IntervalMinutes" value ="1"/>  --- 1 for 1 minute
   </appSettings>
 4. Please set your created PTCLogFile, PTCSharedFolder and BackupOrderFiles folder in LogFolder,FolderPath and BackupFolderPath fields.
  <appSettings>  
 
    <add key="FolderPath" value="C:\PickToColor\PTCSharedFolder" />
    <add key="FolderPath" value="C:\PickToColor\PTCSharedFolder" />
    <add key="BackupFolderPath" value="C:\PickToColor\BackupOrderFiles" />
   </appSettings>  
5. System will delete old order data from database. Please set how many day data you want to keep in the database.    
  <appSettings>  
  
    <add key="DeleteOrdersAfter" value="360"/> -- system will keep only 360 days data.
   </appSettings> 
6. After change it, save and close the config file.
 
 
Installing Window Service
-------------------------------------------------------------------------------------------------------------------------------------

1. open command prompt with adminstrator mode (run as administrator)

2. Type the following command and enter:
```
cd c:\Windows\Microsoft.NET\Framework64\v4.0.30319
```

3.  Type the following command and enter:
```
InstallUtil.exe “C:\PickToColor\PickToColorService\bin\x64\Release\PickToColorService.exe”
```
4. Go to Windows's Service.msc and check if Pick To Color service is running. Otherwise, start the service.


Give Permission for AUTHORITY\SYSTEM user to access Pick To Color Database
-------------------------------------------------------------------------------------------------------------------------------------
1. Open SQL Management Studio
2. Connect to SQL Server instance 
3. Navigate to Security
4. Open the Logins folder
5. Double click NT AUTHORITY\SYSTEM icon
6. Click on the Server Roles icon in the Select a page pane
7. Tick the sysadmin checkbox in the Server roles: pane

Uninstalling Window Service
-------------------------------------------------------------------------------------------------------------------------------------
1. open command prompt with adminstrator mode (run as administrator)

2. Type the following command and enter:
```
cd c:\Windows\Microsoft.NET\Framework64\v4.0.30319
```
3.  Type the following command and enter:
```
InstallUtil.exe /u “C:\PickToColor\PickToColorService\bin\x64\Release\PickToColorService.exe”
```

Template excel file to upload order information
-------------------------------------------------------------------------------------------------------------------------------------
refer `ordersample.xlsx` excel file

Copyright 2022 ACE Innovatice Solutions. All rights reserved.
