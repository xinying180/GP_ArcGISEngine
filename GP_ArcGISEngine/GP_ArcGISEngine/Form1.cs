using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.AnalysisTools;
using ESRI.ArcGIS.SpatialAnalystTools;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.Carto;
using System.Diagnostics;

namespace GP_ArcGISEngine
{
    public partial class Form1 : Form
    {
        string FileGDBPath;
        string tbxPath;
        
        public Form1()
        {
            InitializeComponent();
            FileGDBPath = Application.StartupPath + "\\test.gdb";
            tbxPath = Application.StartupPath + "\\mytoolbox.tbx";
        }


        private void iVariantArray方式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeoProcessor2 gp = new GeoProcessorClass();
            gp.OverwriteOutput = true;

            IGeoProcessorResult result = new GeoProcessorResultClass();

            // Create a variant array to hold the parameter values.
            IVariantArray parameters = new VarArrayClass();

            object sev = null;

            try
            {
                // Populate the variant array with parameter values.
                parameters.Add(FileGDBPath + "\\LotIds");
                parameters.Add(FileGDBPath + "\\LotIds_BufferArray");
                parameters.Add("100 Feet");

                // Execute the tool.
                result = gp.Execute("Buffer_analysis", parameters, null);

                // Print geoprocessring messages.
                Console.WriteLine(gp.GetMessages(ref sev));
            }

            catch (Exception ex)
            {
                // Print a generic exception message.
                Console.WriteLine(ex.Message);
                // Print geoprocessing execution error messages.
                Console.WriteLine(gp.GetMessages(ref sev));
            }

        }

        private void iToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Initialize the geoprocessor.
            IGeoProcessor2 gp = new GeoProcessorClass();
            gp.OverwriteOutput = true;
            // Add the BestPath toolbox.
            gp.AddToolbox(tbxPath);

            // Generate the array of parameters.
            IVariantArray parameters = new VarArrayClass();
            parameters.Add(FileGDBPath + "\\LotIds");
            parameters.Add("100 Feet");
            parameters.Add(FileGDBPath + "\\LotIds_BufferCustomArray1");
            

            object sev = null;
            try
            {
                // Execute the model tool by name.
                gp.Execute("Model2", parameters, null);

                Console.WriteLine(gp.GetMessages(ref sev));
            }
            catch (Exception ex)
            {
                // Print geoprocessing execution error messages.
                Console.WriteLine(gp.GetMessages(ref sev));
            }

        }

        private void managedToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create the geoprocessor. 
            Geoprocessor GP = new Geoprocessor();
            GP.OverwriteOutput = true;
            // Create the tool process object.
            ESRI.ArcGIS.AnalysisTools.Buffer bufferTool = new
                ESRI.ArcGIS.AnalysisTools.Buffer();

            // Set parameter values.
            bufferTool.in_features = FileGDBPath + "\\LotIds";
            bufferTool.out_feature_class = FileGDBPath + "\\LotIds_BufferSystem";
            bufferTool.buffer_distance_or_field = "100 Feet";

            object sev = null;
            try
            {
                // Execute the tool.
                GP.Execute(bufferTool, null);

                Console.WriteLine(GP.GetMessages(ref sev));
            }
            catch (Exception ex)
            {
                // Print geoprocessing execution error messages.
                Console.WriteLine(GP.GetMessages(ref sev));
            }

        }


        private void managed方式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Initialize the geoprocessor.
            Geoprocessor GP = new Geoprocessor();
            GP.OverwriteOutput = true;
            // Add the BestPath toolbox.
            GP.AddToolbox(tbxPath);

            // Generate the array of parameters.
            IVariantArray parameters = new VarArrayClass();
            parameters.Add(FileGDBPath + "\\LotIds");
            parameters.Add("100 Feet");
            parameters.Add(FileGDBPath + "\\LotIds_BufferCustomArray2");

            object sev = null;
            try
            {
                // Execute the model tool by name.
                GP.Execute("Model2", parameters, null);

                Console.WriteLine(GP.GetMessages(ref sev));
            }
            catch (Exception ex)
            {
                // Print geoprocessing execution error messages.
                Console.WriteLine(GP.GetMessages(ref sev));
                //for (int i = 0; i < GP.MessageCount; i++)
                //    Console.WriteLine(GP.GetMessage(i));
            }

        }

        private void 获取报错信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geoprocessor GP = new Geoprocessor();
            GP.OverwriteOutput = true;
            // Create the tool process object.
            ESRI.ArcGIS.AnalysisTools.Buffer bufferTool = new
                ESRI.ArcGIS.AnalysisTools.Buffer();

            // Set parameter values.
            //bufferTool.in_features = FileGDBPath + "\\LotIds";
            bufferTool.in_features = FileGDBPath + "\\line";
            //bufferTool.out_feature_class = FileGDBPath + "\\LotIds_BufferSystem";
            bufferTool.out_feature_class = FileGDBPath + "\\line_Buffer";
            bufferTool.buffer_distance_or_field = "100 Feet";
            bufferTool.line_side = "LEFT";
            bufferTool.line_end_type = "FLAT";
            try
            {
                IGeoProcessorResult2 result = GP.Execute(bufferTool, null) as IGeoProcessorResult2;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GP Error");
            }
            finally
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < GP.MessageCount; i++)
                    sb.AppendLine(GP.GetMessage(i));
                if (sb.Capacity > 0) MessageBox.Show(sb.ToString(), "GP Messages");
            }
        }       



        private void 后台GPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            gp.ToolExecuting += new EventHandler<ESRI.ArcGIS.Geoprocessor.ToolExecutingEventArgs>(gpToolExecuting);

            gp.ProgressChanged += new EventHandler<ESRI.ArcGIS.Geoprocessor.ProgressChangedEventArgs>(gpProgressChanged);
            //Register to receive the geoprocessor event when the tools have completed execution.
            gp.ToolExecuted += new EventHandler<ESRI.ArcGIS.Geoprocessor.ToolExecutedEventArgs>(gpToolExecuted);

            // Create a variant array to hold the parameter values.
            IVariantArray parameters = new VarArrayClass();

            object sev = null;

            // Populate the variant array with parameter values.
            parameters.Add(FileGDBPath + "\\testPoint_10w");
            parameters.Add(FileGDBPath + "\\testPoint_10w_Copy");

            IGeoProcessorResult2 gpResult = gp.Execute("CopyFeatures_management", parameters,null) as
                IGeoProcessorResult2;
        }

        public void gpToolExecuting(object sender, ToolExecutingEventArgs e)
        {
            IGeoProcessorResult2 result = e.GPResult as IGeoProcessorResult2;
            //Determine if this is the tool to handle this event.
            if (result.Process.Tool.Name.Equals("CopyFeatures_management") && result.GetInput(0)
                .GetAsText().Equals(FileGDBPath + "\\testPoint_10w") && result.GetOutput(0)
                .GetAsText().Equals(FileGDBPath + "\\testPoint_10w_Copy"))
            {
                //Application specific code.
            }
        }

        public void gpProgressChanged(object sender, ESRI.ArcGIS.Geoprocessor.ProgressChangedEventArgs e)
        {
            //System.Windows.Forms.ProgressBar progressBar = progressBar1;
            IGeoProcessorResult2 gpResult = (IGeoProcessorResult2)e.GPResult;
            switch (e.ProgressChangedType)
            {
                case (ProgressChangedType.Show):
                    //The tool that is running reports progress or has stopped reporting progress; make the 
                    // progress bar visible if appropriate. 
                    //progressBar.Visible = e.Show;
                    break;
                case (ProgressChangedType.Message):
                    //The application does not use these, since a tool being used reports percentage progress.
                    break;
                case (ProgressChangedType.Percentage): 
                    //progressBar.Value = (int)
                    //e.ProgressPercentage;
                    break;
                default:
                    throw new ApplicationException(
                        "unexpected ProgressChangedEventsArgs.ProgressChangedType");
                    break;
            }
        }

        public void gpToolExecuted(object sender, ToolExecutedEventArgs e)
        {
            IGeoProcessorResult2 result = e.GPResult as IGeoProcessorResult2;
            if (result.Status.Equals(esriJobStatus.esriJobSucceeded))
            {
                //Check that there are no information or warning messages.
                if (result.MaxSeverity == 0)
                {
                    //Get the return value.
                    object returnValue = result.ReturnValue;
                    //Application specific code, 
                    //for example, find the layer to which this return value corresponds.
                }
                else
                {
                    //Application specific code.
                }
             
            }
            else
            {
                //Get all messages.
                IGPMessages msgs = result.GetResultMessages();
                for (int i = 0; i < result.MessageCount; i++)
                {
                    IGPMessage2 msg = msgs.GetMessage(i) as IGPMessage2;
                    //Application specific code.
                }
            }
        }

        private void 前后台GP对比ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //前台执行GP
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
           
            // Create a variant array to hold the parameter values.
            IVariantArray parameters = new VarArrayClass();

            // Populate the variant array with parameter values.
            parameters.Add(FileGDBPath + "\\testPoint_100w");
            parameters.Add(FileGDBPath + "\\testPoint_100w_CopyPre");

            try
            {
                IGeoProcessorResult2 result = gp.ExecuteAsync("CopyFeatures_management", parameters) as IGeoProcessorResult2;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GP Error");
            }
            finally
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < gp.MessageCount; i++)
                    sb.AppendLine(gp.GetMessage(i));
                if (sb.Capacity > 0) MessageBox.Show(sb.ToString(), "GP Messages");
            }
           
            //后台执行GP
            // Create a variant array to hold the parameter values.
            IVariantArray parameters1 = new VarArrayClass();

            // Populate the variant array with parameter values.
            parameters1.Add(FileGDBPath + "\\testPoint_100w");
            parameters1.Add(FileGDBPath + "\\testPoint_100w_CopyBack");

            try
            {
                IGeoProcessorResult2 result = gp.Execute("CopyFeatures_management", parameters1, null) as IGeoProcessorResult2;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GP Error");
            }
            finally
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < gp.MessageCount; i++)
                    sb.AppendLine(gp.GetMessage(i));
                if (sb.Capacity > 0) MessageBox.Show(sb.ToString(), "GP Messages");
            }

        }


    }
}
