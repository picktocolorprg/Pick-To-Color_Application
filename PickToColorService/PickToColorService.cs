using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Collections;

namespace PickToColorService
{
    public partial class PickToColorService : ServiceBase
    {
        string constr, sqlconn;
        //OleDbConnection Econ;
        SqlConnection con;
        private Timer Scheduler;
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public PickToColorService()
        {
            InitializeComponent();            
        }


        public void ScheduleService()
        {
            try
            {
                Scheduler = new Timer(new TimerCallback(SchedularCallback));
                
                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;

                //Get the Interval in Minutes from AppSettings.
                int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                //Set the Scheduled Time by adding the Interval to Current Time.
                scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                if (DateTime.Now > scheduledTime)
                {
                    //If Scheduled Time is passed set Schedule for the next Interval.
                    scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                }
        

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                //this.LogMessageToFile("Service scheduled to run after: " + schedule + " {0}");
                log.Info("Service scheduled to run after: " + schedule + " {0}");

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                Scheduler.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
               // LogMessageToFile("Service Error on: {0} " + ex.Message + ex.StackTrace);
                log.Error("Service Error on: {0} " + ex.Message + ex.StackTrace);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            //this.LogMessageToFile("Service started {0}");
            log4net.Config.XmlConfigurator.Configure();
            log.Info("Service started.");
            this.ScheduleService();             
        }

        protected override void OnStop()
        {
            log.Info("Service stopped.");
            this.Scheduler.Dispose();  
        }

        public void Start()
        {
            OnProcess();           
           
        }

        private void OnProcess()
        {         
            
            InsertExcelRecords();
            //DeleteImages();
            //DeleteSoundFiles();
            DeleteOrder();
        }

        private void InsertExcelRecords()
        {
            OleDbConnection objConnection=null;
      
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

            bool exists = System.IO.Directory.Exists(FolderPath);
            if (exists)
            {
                // get the previous day's file list
                string[] excelFiles = Directory.GetFiles(FolderPath, "*.xlsx")                      
                            .Select(path => Path.GetFileName(path))
                            .ToArray();


                for (int i = 0; i < excelFiles.Length; i++)
                {
                    try
                    {
                        string FilePath = FolderPath + "\\" + excelFiles[i].ToString();

                        constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", FilePath);
                        objConnection = new OleDbConnection(constr);

                        DataSet dsImport = new DataSet();

                        objConnection.Open();
                        DataTable dtSchema = objConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if ((null != dtSchema) && (dtSchema.Rows.Count > 0))
                        {
                            string firstSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
                            new OleDbDataAdapter("SELECT * FROM [" + firstSheetName + "]", objConnection).Fill(dsImport);
                        }

                        objConnection.Close();
                        if (dsImport.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtExcelData = GetData(dsImport.Tables[0]);
                            InsertDataTableintoSQLTable(dtExcelData, excelFiles[i].ToString());


                            string BackupFolderPath = ConfigurationManager.AppSettings["BackupFolderPath"];
                            string destinationFile = BackupFolderPath + "\\" + excelFiles[i].ToString();

                            // To move a file or folder to a new location:
                            bool backupFolderexists = System.IO.Directory.Exists(BackupFolderPath);

                            if (!backupFolderexists)
                            {
                                System.IO.Directory.CreateDirectory(BackupFolderPath);
                            }

                            if (File.Exists(destinationFile))
                            {
                                File.Delete(destinationFile);
                            }
                            File.Move(FilePath, destinationFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        log.Error("InsertExcelRecords: "+ex.Message.ToString());
                    }
                    finally
                    {
                        // Clean up.
                        if (objConnection != null)
                        {
                            objConnection.Close();
                            objConnection.Dispose();
                        }
                    } 
                }                  
            }           
        
        }

 
      private void DeleteOrder()
      {
        try
        {
            connection();
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();
                string query="Delete From Orders WHERE [CreatedDateTime] < DATEADD(day, -"+ ConfigurationManager.AppSettings["DeleteOrdersAfter"] +", GETDATE())";
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                   command.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
        }
        catch (SystemException ex)
        {
            log.Error("DeleteOrder: " + ex.Message.ToString());
        }
      }
     


        private DataTable GetData(DataTable dtExcelData)
        {            
            try
            {
                dtExcelData.Columns.Add("CustomerID", typeof(String));
                dtExcelData.Columns.Add("OrderTypeID", typeof(String));
                SqlCommand command;
                SqlDataAdapter da;
                foreach (DataRow drExcel in dtExcelData.Rows)
                {
                    

                    connection();
                    //check empty rows
                    if (string.IsNullOrEmpty(drExcel["Order_CD"].ToString()) && string.IsNullOrEmpty(drExcel["OrderType"].ToString()) && string.IsNullOrEmpty(drExcel["CustomerCode"].ToString()) && string.IsNullOrEmpty(drExcel["SKU"].ToString()))
                    {
                        drExcel.Delete();
                    }
                    else
                    {

                        // check duplicate records
                        string duplicateSQl = "Select Orders.* From Orders,OrderTypes Where Orders.OrderTypeID=1 AND OrderCD='" + drExcel["Order_CD"].ToString().Trim() + "'";
                        con.Open();
                        command = new SqlCommand(duplicateSQl, con);
                        da = new SqlDataAdapter(command);
                        DataTable dtOrder = new DataTable();
                        da.Fill(dtOrder);
                        con.Close();
                        if (dtOrder.Rows.Count > 0)
                        {
                            drExcel.Delete();
                        }
                        else
                        {

                            String customerSQL = "Select Top 1 Customers.CustomerID From Customers,OrderTypes Where Customers.[CustomerCode]='" + drExcel["CustomerCode"].ToString().Trim() + "'";

                            con.Open();
                            command = new SqlCommand(customerSQL, con);

                            da = new SqlDataAdapter(command);
                            DataTable dtCustOrder = new DataTable();
                            da.Fill(dtCustOrder);
                            con.Close();

                            if (dtCustOrder.Rows.Count > 0)
                            {
                                drExcel["CustomerID"] = Convert.ToInt32(dtCustOrder.Rows[0]["CustomerID"]);
                                drExcel["OrderTypeID"] = 1;
                            }
                        }
                    }                    

                }
                return dtExcelData;
               
            }
            catch(Exception ex)
            {
                log.Error("GetData: " + ex.Message.ToString());
            }
            return dtExcelData;
        }

        private void InsertDataTableintoSQLTable(DataTable dtExcelData,string fileName)
        {
            try
            {
                connection();
                //creating object of SqlBulkCopy  
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                //assigning Destination table name  
                objbulk.DestinationTableName = "Orders";
                //Mapping Table column 
                objbulk.ColumnMappings.Add("CustomerID", "CustomerID");
                objbulk.ColumnMappings.Add("Order_CD", "OrderCD");
                objbulk.ColumnMappings.Add("SKU", "SKU");
                objbulk.ColumnMappings.Add("Qty", "Quantity");
                objbulk.ColumnMappings.Add("OrderTypeID", "OrderTypeID");
                //inserting Datatable Records to DataBase  
                con.Open();
                objbulk.WriteToServer(dtExcelData);
                con.Close();
            }
            catch (Exception ex)
            {
                log.Error("InsertDataTableintoSQLTable: " + ex.Message.ToString());
                
            }
        }

        private void connection()
        {
            sqlconn = ConfigurationManager.ConnectionStrings["SqlCom"].ConnectionString;
            con = new SqlConnection(sqlconn);

        }
        private void SchedularCallback(object e)
        {
            OnProcess();       
            this.ScheduleService();
        }

      
       
    }
}
