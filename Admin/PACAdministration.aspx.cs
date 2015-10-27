using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;
using WSCIEMP.Common;
using System.Globalization;
using System.Text;
using System.Configuration;
using System.Data;

namespace WSCIEMP.Admin
{
    public partial class PACAdministration : Common.BasePage {

        private const string MOD_NAME = "Admin.PACAdministration.";
        private const string BLANK_CELL = "&nbsp;";
        public string getUserName { get { return System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().Split(@"\".ToCharArray())[1]; } }

        private string MySHID
        {
            get { return (String)ViewState["mySHID"]; }
            set {
                ViewState["mySHID"] = value;
                if (value != null && value.Length > 0)
                {
                    PACDetails.Style["display"] = "static";
                }
                else
                {
                    PACDetails.Style["display"] = "none";
                }
                if (ddlCropYear.SelectedIndex == 0)
                {
                    PACDetailsActions.Style["display"] = "static";
                }
                else
                {
                    PACDetailsActions.Style["display"] = "none";
                }
            }
        }

        private List<Individual> IndTable
        {
            get { return (List<Individual>)ViewState["indTable"]; }
            set { ViewState["indTable"] = value; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            const string METHOD_NAME = "Page_Load";

            try {
                Common.AppHelper.HideWarning(addrWarning);
                Common.AppHelper.HideWarning(indWarning);

                if (!Page.IsPostBack)
                {
                    this.MySHID = null;
                    this.IndTable = new List<Individual>();
                    WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                }

                txtSHID.Attributes.Add("onkeypress", "CheckEnterKey(event);");
            }
            catch (System.Exception ex)
            {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                ((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
        }

        
        // PAC Search
        
        protected void btnResolveShid_Click(object sender, EventArgs e)
        {
            int crop_year = Convert.ToInt16(ddlCropYear.Text);
            string shid = txtSHID.Text;

            LoadPAC(crop_year, shid);
            CloseAndResolve("AddressFinder");
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.MySHID != null && this.MySHID.Length > 0)
            {
                btnResolveShid_Click(sender, e);
            }
        }
        
        
        // Address Search
        
        protected void btnAddrOk_Click(object sender, EventArgs e)
        {
            // If we have a selected address, use the shid for our main page.
            if (lstAddressName.SelectedItem != null)
            {
                int crop_year = Convert.ToInt16(ddlCropYear.Text);
                string shid = txtAddrSHID.Text;
                if (hdnFinderType.Value == "AddressFinder")
                {
                    LoadPAC(crop_year, shid);
                }
                else
                {
                    var ind = new Individual
                    {
                        FullName = txtAddrFName.Text + " " + txtAddrLName.Text,
                        SHID = Convert.ToInt32(shid),
                        Sort = Convert.ToChar("1"),
                        Percentage = 100,
                        IndividualID = 0,
                        Email = txtEmail.Text
                    };
                    ind.IndividualID = PACData.SaveIndividual(ind);
                    IndTable.Add(ind);
                    ReBuildTable();
                    UpdatePACDetails.Update();
                }
                CloseAndResolve("AddressFinder");
            }
        }

        protected void btnAddrFind_Click(object sender, EventArgs e)
        {
            const string METHOD_NAME = "btnAddrFind_Click";

            pacMessages.InnerText = "";

            try
            {
                Common.AppHelper.HideWarning(addrWarning);

                lstAddressName.Items.Clear();

                string searchTerm = txtSearchString.Text.TrimEnd();
                int searchType = 0;

                if (radTypeSHID.Checked)
                {
                    searchType = 1;
                }
                else
                {
                    if (radTypeBusname.Checked)
                    {
                        searchType = 2;
                    }
                    else
                    {
                        searchType = 3;
                    }
                }

                if (searchTerm.Length > 0)
                {

                    if (!searchTerm.Contains("*"))
                    {
                        searchTerm = searchTerm + "%";
                    }
                    else
                    {
                        searchTerm = searchTerm.Replace("*", "%");
                    }
                }

                int cropYear = Convert.ToInt16(ddlCropYear.Text);;
                List<ListAddressItem> addrList = BeetDataAddress.AddressFindByTerm(searchTerm, cropYear, searchType);
                lstAddressName.DataSource = addrList;
                lstAddressName.DataBind();
            }
            catch (System.Exception ex)
            {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                ((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
        }

        
        // Signer Search

        protected void lstAddressName_SelectedIndexChanged(object sender, EventArgs e)
        {
            const string METHOD_NAME = "lstAddressName_SelectedIndexChanged";
            try
            {
                Common.AppHelper.HideWarning(addrWarning);

                int cropYear = Convert.ToInt16(ddlCropYear.Text);
                ListItem selItem = lstAddressName.SelectedItem;
                List<ListAddressItem> addrList = BeetDataAddress.AddressGetInfo(Convert.ToInt32(selItem.Value), 0, cropYear);

                if (addrList.Count > 0)
                {
                    ListAddressItem item = addrList[0];
                    txtAddrSHID.Text = item.SHID;
                    chkAddrSubscriber.Checked = item.IsSubscriber;
                    txtAddrFName.Text = item.FirstName;
                    txtAddrLName.Text = item.LastName;
                    txtAddrBName.Text = item.BusName;
                    txtAddrAddress.Text = item.AdrLine1;
                    txtAddrAddressLine2.Text = item.AdrLine2;
                    txtAddrCity.Text = item.CityName;
                    txtAddrState.Text = item.StateName;
                    txtAddrZip.Text = item.PostalCode;
                    txtAddrPhoneNo.Text = item.PhoneNo;
                    txtEmail.Text = item.Email;
                    txtAddrType.Value = Convert.ToString(item.AddressType);
                }
            }
            catch (System.Exception ex)
            {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                ((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
        }

        protected void btnIndAdd_Click(object sender, EventArgs e)
        {
            pacMessages.InnerText = "";
            int userId = 0;
            if (newIndividualName != null && newIndividualName.Text.Length > 1)
            {
                var i = new Individual
                {
                    IndividualID = 0,
                    FullName = newIndividualName.Text,
                    Email = newIndividualEmail.Text,
                    SHID = Convert.ToInt32(txtSHID.Text)
                };
                userId = PACData.SaveIndividual(i);
                if (userId > 0)
                {
                    Individual user = PACData.GetPACIndividuals(userId, null)[0];
                    if (user != null)
                    {
                        this.IndTable.Add(user);
                        ReBuildTable();
                        UpdatePACDetails.Update();
                        CloseAndResolve("PACIndividuals");
                    }
                }
            }
        }

        protected void btnIndFind_Click(object sender, EventArgs e)
        {
            const string METHOD_NAME = "btnIndFind_Click";
            try
            {

                Common.AppHelper.HideWarning(indWarning);

                IndividualsListBox.Items.Clear();

                string searchTerm = IndName.Text.TrimEnd();

                if (searchTerm.Length > 0)
                {
                    if (!searchTerm.Contains("*"))
                    {
                        searchTerm = "%" + searchTerm + "%";
                    }
                    else
                    {
                        searchTerm = searchTerm.Replace("*", "%");
                    }
                }

                List<Individual> indList = PACData.GetPACIndividuals(0, searchTerm);
                IndividualsListBox.DataSource = indList;
                IndividualsListBox.DataBind();

            }
            catch (System.Exception ex)
            {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                ((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", indWarning);
            }
        }

        protected void btnIndSel_Click(object sender, EventArgs e)
        {
            UpdateIndTable();

            ListItem selItem = IndividualsListBox.SelectedItem;

            Boolean unique = true;
            for (int i = 0; i < this.IndTable.Count; i++)
            {
                if (this.IndTable[i].IndividualID == Convert.ToInt16(selItem.Value))
                {
                    unique = false;
                }
            }

            if (unique)
            {
                this.IndTable.Add(new Individual(Convert.ToInt16(selItem.Value), selItem.Text, 0, false, DateTime.Now, ' '));
                ReBuildTable();
                UpdatePACDetails.Update();
            }

            CloseAndResolve("PACIndividuals");
        }


        // PAC Form

        protected void btnDownloadPACAgreement_Click(object server, EventArgs e)
        {
            var METHOD_NAME = "btnDownloadPACAgreement_Click";
            var qs = new NameValueCollection();
            if (lblAddressType.Text == "Corporation")
            {
                pacMessages.InnerText = "";

                string searchTerm = txtSHID.Text.Trim();
                int searchType = 1;
                int cropYear = Convert.ToInt16(ddlCropYear.Text);
                List<ListAddressItem> addrList = BeetDataAddress.AddressFindByTerm(searchTerm, cropYear, searchType);
                var address = new StringBuilder();
                address.AppendFormat("{0}, {1}, {2}, {3}", addrList[0].AdrLine1, addrList[0].AdrLine2, addrList[0].CityName, addrList[0].StateName);
                var phone = addrList[0].PhoneNo ?? "";

                var pac = PACData.GetPACAgreement(txtSHID.Text, Convert.ToInt16(ddlCropYear.Text));
                var inds = PACData.GetPACIndividuals(pac.Individuals[0].IndividualID, null);
                var i = new Individual();
                var signerFirstName = inds[0].FullName.Split(" ".ToCharArray())[0];
                var signerLastName = inds[0].FullName.Split(" ".ToCharArray())[1];

                var date = DateTime.Now;
                var mfi = new DateTimeFormatInfo();
                var strMonthName = mfi.GetMonthName(date.Month).ToString();

                qs.Add("Filename", "PACDuesCorp");
                qs.Add("CORPORATION NAME", Server.UrlEncode(lblBusName.Text));
                qs.Add("CorporationName", Server.UrlEncode(lblBusName.Text));
                qs.Add("LastNameFirstName", signerLastName + ", " + signerFirstName);
                //qs.Add("Dated", DateTime.Now.ToString("MM/dd/yyyy"));
                qs.Add("CentsPerTonDevlivered", pACContibution.Text);
                qs.Add("TwoDigitCents", (pACContibution.Text.Length == 1) ? "0" + pACContibution.Text : pACContibution.Text);
                qs.Add("Address", address.ToString());
                qs.Add("PHONE", phone);
                qs.Add("Text1", DateTime.Now.Year.ToString());
                qs.Add("Year2", DateTime.Now.Year.ToString());

                try
                {
                    qs.Add("Individual1", (PACData.GetPACIndividuals(pac.Individuals[0].IndividualID, null)[0].FullName));
                    qs.Add("IndividualPercentage1", pac.Individuals[0].Percentage.ToString());
                    qs.Add("Individual2", (PACData.GetPACIndividuals(pac.Individuals[1].IndividualID, null)[0].FullName));
                    qs.Add("IndividualPercentage2", pac.Individuals[1].Percentage.ToString());
                    qs.Add("Individual3", (PACData.GetPACIndividuals(pac.Individuals[2].IndividualID, null)[0].FullName));
                    qs.Add("IndividualPercentage3", pac.Individuals[2].Percentage.ToString());
                    qs.Add("Individual4", (PACData.GetPACIndividuals(pac.Individuals[3].IndividualID, null)[0].FullName));
                    qs.Add("IndividualPercentage4", pac.Individuals[3].Percentage.ToString());
                }
                catch(Exception ex)
                {
                    Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                    ((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", indWarning);
                }
            }
            else
            {
                var pac = PACData.GetPACAgreement(txtSHID.Text, Convert.ToInt16(ddlCropYear.Text));
                var inds = PACData.GetPACIndividuals(pac.Individuals[0].IndividualID, null);
                var i = new Individual();

                var date = DateTime.Now;
                var mfi = new DateTimeFormatInfo();
                var strMonthName = mfi.GetMonthName(date.Month).ToString();

                var ds = WSCContract.GetContracts(txtSHID.Text, 2014, ConfigurationManager.ConnectionStrings["BeetConn"].ToString());
                var strContractIds = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                    strContractIds += dr[0] + ", ";
                if (strContractIds.Length > 2)
                    strContractIds = strContractIds.Substring(0, strContractIds.Length - 2);

                qs.Add("Filename", "PACDuesNonCorp");
                qs.Add("CurrentTwoDigitYear", date.ToString("yy"));
                qs.Add("CurrentDayMonth", mfi.GetMonthName(date.Month).ToString() + " " + date.Day);
                qs.Add("SumOfMoneyPerTon", pACContibution.Text);
                qs.Add("CropYear1", DateTime.Now.Year.ToString());
                qs.Add("SomeBullshit", DateTime.Now.Year.ToString());
                qs.Add("PrintShareholderName", ((Individual)inds[0]).FullName);
                qs.Add("ContractNumber1", strContractIds);
            }

            Response.Redirect("~/Downloads/Downloader.aspx" + qs.ToQueryString());
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.MySHID != null && this.MySHID.Length > 0)
            {
                UpdateIndTable();

                if (pACDate.Text == null || pACDate.Text.Length == 0)
                    pACDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

                if (pACContibution.Text == null || pACContibution.Text.Length == 0)
                    pACContibution.Text = "0";

                if (this.IndTable != null && this.MySHID != null && this.MySHID.Length > 0)
                {
                    PACAgreement pacAgreement = new PACAgreement(this.MySHID, Convert.ToDouble(pACContibution.Text), pACDate.Text, Convert.ToInt16(ddlCropYear.Text), this.IndTable);
                    PACData.SavePACAgreement(pacAgreement);
                }

                btnResolveShid_Click(sender, e);
                pacMessages.InnerText = "PAC Agreement Saved";
                btnDownloadPACAgreement.Enabled = true;
            }
        }

       
        // Private

        private void LoadPAC(int crop_year, string shid)
        {
            var METHOD_NAME = "LoadPAC";
            Common.AppHelper.HideWarning(addrWarning);

            ResetPage();

            txtSHID.Text = shid;
            lblBusName.Text = txtAddrBName.Text;
            this.MySHID = shid;

            try
            {

                var addresses = BeetDataAddress.AddressGetInfo(Convert.ToInt32(shid), 0, crop_year);


                var addr = addresses[0];



                switch (Convert.ToString(addr.AddressType))
                {
                    case "1":
                        lblAddressType.Text = "Individual";
                        break;
                    case "2":
                        lblAddressType.Text = "Corporation";
                        break;
                    default:
                        lblAddressType.Text = "Other";
                        break;
                }

                uplShid.Update();

                PACAgreement pac = PACData.GetPACAgreement(shid, crop_year);
                if (pac != null)
                {
                    btnDownloadPACAgreement.Enabled = true;
                    pACContibution.Text = Convert.ToString(pac.Contribution);
                    pACDate.Text = pac.PACDate;

                    if (pac.Individuals.Count == 0)
                    {
                        pac.Individuals.Add(CreateAndFetchIndividual(crop_year, shid));
                    }
                    indTableField.Value = pac.IndividualsString;
                    this.IndTable = pac.Individuals;
                }
                else
                {
                    btnDownloadPACAgreement.Enabled = false;
                    IndTable.Add(CreateAndFetchIndividual(crop_year, shid));
                }
                UpdatePACDetails.Visible = true;
                ReBuildTable();
                UpdatePACDetails.Update();
            }
            catch(Exception ex)
            {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                ((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", indWarning);
                pacMessages.InnerHtml = "<font style='color: red;'>Invalid SHID / Crop Year</font>";
                //pacMessages.InnerHtml = ex.Message + "<br />" + ex.StackTrace;
                UpdatePACDetails.Visible = false;
                return;
            }
        }

        private void ResetPage()
        {

            const string METHOD_NAME = "ResetPage";

            try
            {
                this.MySHID = "";
                lblBusName.Text = "";
                lblAddressType.Text = "";

                pACContibution.Text = "";
                pACDate.Text = "";

                this.IndTable = new List<Individual>();
                indTableField.Value = "";

                pacMessages.InnerText = "";
            }
            catch (Exception ex)
            {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void UpdateIndTable()
        {
            string summaryData = indTableField.Value;

            this.IndTable = new List<Individual>();

            if (summaryData != null && summaryData.Length > 1)
            {
                string[] lines = summaryData.Split('~');
                for (int l = 0; l < lines.Length; l++)
                {
                    if (lines[l] != null && lines[l].Length > 1)
                    {
                        string[] items = lines[l].Split('|');
                        Individual tempHolder = new Individual();
                        tempHolder.IndividualID = Convert.ToInt32(items[1]);
                        if (items[0] != null && items[0].Length > 0)
                            tempHolder.Sort = Convert.ToChar(items[0]);
                        else
                            tempHolder.Sort = ' ';
                        tempHolder.Percentage = Convert.ToDouble(items[2]);
                        tempHolder.Signed = ("true" == items[3]);
                        if (items.Length > 4 && items[4].Length > 1)
                            tempHolder.SignedDate = Convert.ToDateTime(items[4]);
                        if (items.Length > 5 && items[5].Length > 1)
                            tempHolder.SHID = Convert.ToInt32(items[5]);
                        this.IndTable.Add(tempHolder);
                    }
                }
            }
        }

        private void ReBuildTable()
        {
            const string METHOD_NAME = "ReBuildTable";

            try
            {
                int crop_year = Convert.ToInt16(ddlCropYear.Text);
                string shid = txtSHID.Text;

                List<Contract> contractList = BeetDataContract.GetContracts(shid, crop_year, 0);

                if (contractList != null && contractList.Count > 0)
                {
                    TableHeaderRow headerRow = new TableHeaderRow();

                    TableHeaderCell h1 = new TableHeaderCell();
                    h1.Text = "Contract No";
                    headerRow.Cells.Add(h1);

                    TableHeaderCell h2 = new TableHeaderCell();
                    h2.Text = "PAC Dues";
                    headerRow.Cells.Add(h2);

                    contractTable.Rows.Add(headerRow);

                    for (int i = 0; i < contractList.Count; i++)
                    {
                        TableRow row = new TableRow();

                        TableCell cell1 = new TableCell();
                        cell1.Text = Convert.ToString(contractList[i].contractNo);
                        row.Cells.Add(cell1);

                        TableCell cell2 = new TableCell();
                        cell2.Text = Convert.ToString(contractList[i].pacDues);
                        row.Cells.Add(cell2);

                        contractTable.Rows.Add(row);
                    }
                }

                if (IndTable.Count > 0)
                {

                    TableHeaderRow headerRow = new TableHeaderRow();

                    TableHeaderCell hshid = new TableHeaderCell();
                    hshid.Text = "SHID";
                    headerRow.Cells.Add(hshid);

                    TableHeaderCell h1 = new TableHeaderCell();
                    h1.Text = "Sort";
                    headerRow.Cells.Add(h1);

                    TableHeaderCell h2 = new TableHeaderCell();
                    h2.Text = "Individual";
                    headerRow.Cells.Add(h2);

                    TableHeaderCell hEmail = new TableHeaderCell();
                    hEmail.Text = "Email";
                    headerRow.Cells.Add(hEmail);

                    TableHeaderCell h3 = new TableHeaderCell();
                    h3.Text = "Individual Id";
                    h3.Attributes.Add("style", "display: none");
                    headerRow.Cells.Add(h3);

                    TableHeaderCell h4 = new TableHeaderCell();
                    h4.Text = "Percent";
                    headerRow.Cells.Add(h4);

                    TableHeaderCell h5 = new TableHeaderCell();
                    h5.Text = "Signature Date";
                    headerRow.Cells.Add(h5);

                    TableHeaderCell h6 = new TableHeaderCell();
                    h6.Text = "Remove";
                    headerRow.Cells.Add(h6);

                    indTable.Rows.Add(headerRow);

                    for (int i = 0; i < this.IndTable.Count; i++)
                    {

                        var ind = PACData.GetPACIndividuals(this.IndTable[i].IndividualID, null)[0];

                        TableRow row = new TableRow();

                        TableCell cellshid = new TableCell();
                        cellshid.Text = GetShid(ind.SHID.ToString());
                        row.Cells.Add(cellshid);

                        TableCell cell1 = new TableCell();
                        TextBox Sort = new TextBox();
                        Sort.ID = "Sort_" + i;
                        Sort.Width = 15;
                        Sort.MaxLength = 1;
                        Sort.Attributes.Add("runat", "server");
                        Sort.Attributes.Add("class", "sort");
                        Sort.Text = Convert.ToString(this.IndTable[i].Sort).Trim();
                        cell1.Controls.Add(Sort);
                        row.Cells.Add(cell1);

                        TableCell cell2 = new TableCell();
                        cell2.Text = ind.FullName;
                        cell2.Width = 150;
                        row.Cells.Add(cell2);

                        TableCell cellEmail = new TableCell();
                        cellEmail.Text = ind.Email;
                        cellEmail.Width = 150;
                        row.Cells.Add(cellEmail);

                        TableCell cell3 = new TableCell();
                        HtmlInputHidden IndId = new HtmlInputHidden();
                        IndId.ID = "IndividualID_" + i;
                        IndId.Value = Convert.ToString(this.IndTable[i].IndividualID);
                        IndId.Attributes.Add("class", "individualid");
                        cell3.Controls.Add(IndId);
                        cell3.Attributes.Add("style", "display: none");
                        row.Cells.Add(cell3);

                        TableCell cell4 = new TableCell();
                        TextBox Percentage = new TextBox();
                        Percentage.ID = "Percentage_" + i;
                        Percentage.Width = 25;
                        Percentage.MaxLength = 5;
                        Percentage.Attributes.Add("runat", "server");
                        Percentage.Attributes.Add("class", "percentage");
                        Percentage.Text = Convert.ToString(this.IndTable[i].Percentage);
                        cell4.Controls.Add(Percentage);
                        row.Cells.Add(cell4);

                        /*
                        TableCell cell4 = new TableCell();
                        HtmlInputCheckBox Signed = new HtmlInputCheckBox();
                        Signed.ID = "Signed_" + i;
                        Signed.Checked = this.IndTable[i].Signed;
                        Signed.Attributes.Add("runat", "server");
                        Signed.Attributes.Add("class", "signed");
                        cell4.Controls.Add(Signed);
                        row.Cells.Add(cell4);
                        */

                        TableCell cell5 = new TableCell();
                        HtmlInputText SignedDate = new HtmlInputText();
                        SignedDate.ID = "SignedDate_" + i;
                        SignedDate.Size = 10;
                        if (this.IndTable[i].Signed)
                            SignedDate.Value = this.IndTable[i].SignedDate.ToString("MM/dd/yyyy");
                        SignedDate.Attributes.Add("class", "signedDate");
                        SignedDate.Attributes.Add("onclick", "JavaScript:LaunchDatePicker(this);");
                        cell5.Controls.Add(SignedDate);
                        row.Cells.Add(cell5);

                        TableCell cell6 = new TableCell();
                        cell6.Attributes.Add("style", "text-align: center;");
                        HtmlAnchor anchor = new HtmlAnchor();
                        anchor.HRef = "javascript:void(0);";
                        anchor.InnerText = "X";
                        anchor.Attributes.Add("onclick", "DeleteIndRow(this);");
                        cell6.Controls.Add(anchor);
                        row.Cells.Add(cell6);

                        indTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }

        }

        private void CloseAndResolve(string dialogId)
        {
            string script = string.Format(@"closeAndResolve('{0}')", dialogId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, script, true);
        }

        private Individual CreateAndFetchIndividual(int crop_year, string shid)
        {
            int cropYear = Convert.ToInt16(ddlCropYear.Text);
            List<ListAddressItem> addrList = BeetDataAddress.AddressFindByTerm(shid, cropYear, 1);
            var x = addrList[0];
            var ind = new Individual
            {
                FullName = x.FirstName + " " + x.LastName,
                SHID = Convert.ToInt32(shid),
                Sort = Convert.ToChar("1"),
                Percentage = 100,
                IndividualID = 0,
                Email = x.Email
            };
            ind.IndividualID = PACData.SaveIndividual(ind);
            return ind;
        }

        private string GetShid(string shid)
        {
            if (!ShidExists(shid))
                return shid;

            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            foreach (char c in alphabet)
                if (!ShidExists(shid + c.ToString().ToUpper()))
                    return shid + c.ToString().ToUpper();
            return shid;
        }

        private bool ShidExists(string s)
        {
            var result = false;
            //foreach(Individual i in ind
            this.IndTable.ForEach(i => result = (i.SHID.ToString() == s || result) ? true : false);
            return result;
        }
    }
}