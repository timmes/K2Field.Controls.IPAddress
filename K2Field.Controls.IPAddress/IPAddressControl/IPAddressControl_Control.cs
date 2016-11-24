using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

//SourceCode.Forms.Controls.Web.SDK.dll, located in the GAC of the smartforms server or in the bin folder of the K2 Designer web site
using SourceCode.Forms.Controls.Web;
using SourceCode.Forms.Controls.Web.SDK;
using SourceCode.Forms.Controls.Web.SDK.Attributes;

//adds the client-side .js file as a web resource
[assembly: WebResource("K2Field.Controls.IPAddress.IPAddressControl.IPAddressControl_Script.js", "text/javascript", PerformSubstitution = true)]
//adds the client-side style sheet as a web resource
[assembly: WebResource("K2Field.Controls.IPAddress.IPAddressControl.IPAddressControl_Stylesheet.css", "text/css", PerformSubstitution = true)]

namespace K2Field.Controls.IPAddress.IPAddressControl
{
    //specifies the location of the embedded definition xml file for the control
    [ControlTypeDefinition("K2Field.Controls.IPAddress.IPAddressControl.IPAddressControl_Definition.xml")]
    //specifies the location of the embedded client-side javascript file for the control
    [ClientScript("K2Field.Controls.IPAddress.IPAddressControl.IPAddressControl_Script.js")]
    //specifies the location of the embedded client-side stylesheet for the control
    [ClientCss("K2Field.Controls.IPAddress.IPAddressControl.IPAddressControl_Stylesheet.css")]
    //specifies the location of the client-side resource file for the control. You will need to add a resource file to the project properties
    //[ClientResources("K2Field.Controls.IPAddress.Resources.[ResrouceFileName]")]
    public class Control : BaseControl
    {
        #region Control Properties
        public bool IsVisible
        {
            get
            {
                return this.GetOption<bool>("isvisible", true);
            }
            set
            {
                this.SetOption<bool>("isvisible", value, true);
            }
        }
        public bool IsEnabled
        {
            get
            {
                return this.GetOption<bool>("isenabled", true);
            }
            set
            {
                this.SetOption<bool>("isenabled", value, true);
            }
        }
        public string Text { get; set; }
        public string Value
        {
            get { return this.Attributes["value"]; }
            set { this.Attributes["value"] = value; }
        }
        public bool OutputDebugInfo { get; set; }

        #region IDs
        public string ControlID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public override string ClientID
        {
            get
            {
                return base.ID;
            }
        }

        public override string UniqueID
        {
            get
            {
                return base.ID;
            }
        }
        #endregion

        #endregion

        #region Contructor
        public Control() : base("span") //TODO: if needed, inherit from a HTML type like div or input
        {

        }
        #endregion

        #region Control Methods
        protected override void CreateChildControls()
        {
            base.EnsureChildControls();

            //Perform state-specific operations
            switch (base.State)
            {
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Designtime:
                    //assign a temp unique Id for the control
                    this.ID = Guid.NewGuid().ToString();
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Preview:
                    //do any Preview-time manipulation here
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime:
                    //do any runtime manipulation here
                    this.Attributes.Add("enabled", this.IsEnabled.ToString());
                    this.Attributes.Add("visible", this.IsVisible.ToString());
                    break;
            }

            //if outputting the debug info for the control, add the literal control to the controls collection
            //if (OutputDebugInfo)
            //{
            //    this.Controls.Add(AddLabelControlWithControlProperties());
            //}

            // Get the IP address
            this.Text = GetIPAddress();

            // Call base implementation last
            base.CreateChildControls();
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            var propertiesLabel = new HtmlGenericControl("span");
            //populate the value of the label control with the properties of your custom control
            if (base.State == SourceCode.Forms.Controls.Web.Shared.ControlState.Designtime ||
                base.State == SourceCode.Forms.Controls.Web.Shared.ControlState.Preview
                )
            {
                propertiesLabel.ID = this.ID + "_propertiesLabel";
                propertiesLabel.InnerText = "(" + this.GetType().FullName + " - Design Time)";
            }
            if (base.State == SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime)
            {
                propertiesLabel.ID = this.ID + "_propertiesLabel";
                if (OutputDebugInfo)
                {
                    propertiesLabel.InnerText = "(" + this.GetType().FullName + " - Runtime) " +
                                           System.Environment.NewLine + "Control Text: " + this.Text +
                                           System.Environment.NewLine + "Control Value: " + this.Value +
                                           System.Environment.NewLine + "Control Id: " + this.ID +
                                           System.Environment.NewLine + "Enabled: " + this.IsEnabled +
                                           System.Environment.NewLine + "Visible: " + this.IsVisible;
                }
                else
                {
                    propertiesLabel.InnerText = this.Text;
                }

                    
            }
            propertiesLabel.RenderControl(writer);
        }
        #endregion


        #region Helper methods

        /// <summary>
        /// this helper method outputs a label control with various properties for the Smartforms control
        /// it is intended for development and debugging purposes so that you can output the various properties of your custom control
        /// Feel free to add code and properties to the output element
        /// </summary>
        private LiteralControl AddLabelControlWithControlProperties()
        {
            SourceCode.Forms.Controls.Web.Label propertiesLabel = new SourceCode.Forms.Controls.Web.Label();
            //populate the value of the label control with the properties of your custom control
            switch (base.State)
            {
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Designtime:
                    this.ID = Guid.NewGuid().ToString() + "_propertiesLabel";
                    propertiesLabel.Text = "(" + this.GetType().FullName + " - Design Time)";
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Preview:
                    propertiesLabel.Text = "(" + this.GetType().FullName + " - Preview)";
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime:
                    propertiesLabel.ID = this.ID + "_propertiesLabel";
                    propertiesLabel.Text = "(" + this.GetType().FullName + " - Runtime) " +
                        System.Environment.NewLine + "Control Text: " + this.Text +
                        System.Environment.NewLine + "Control Value: " + this.Value +
                        System.Environment.NewLine + "Control Id: " + this.ID +
                        System.Environment.NewLine + "Enabled: " + this.IsEnabled +
                        System.Environment.NewLine + "Visible: " + this.IsVisible;
                    break;
            }

            //after defining the control, get the control's HTML so we can inject it into the Div placeholder
            string controlHTML = "";

            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                propertiesLabel.RenderControl(writer);
                controlHTML = writer.InnerWriter.ToString();
            }

            LiteralControl controlProperties = new LiteralControl("<div>" + controlHTML + "</div>");

            return controlProperties;
        }

        /// <summary>
        /// Returns the current clients IP address
        /// </summary>
        /// <returns></returns>
        private string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        #endregion
    }
}
