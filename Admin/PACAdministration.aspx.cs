﻿using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Admin
{
    public partial class PACAdministration : Common.BasePage {

        private const string MOD_NAME = "Admin.PACAdministration.";
        private const string BLANK_CELL = "&nbsp;";

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

        private void CloseAndResolve(string dialogId)
        {
            string script = string.Format(@"closeAndResolve('{0}')", dialogId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, script, true);
        }

        private void ResetPage() {

            const string METHOD_NAME = "ResetPage";

            try {
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
            int crop_year = Convert.ToInt16(ddlCropYear.Text);
            string shid = txtSHID.Text;
            downloadContracts.Style["display"] = "none";

            List<Contract> contractList = BeetDataContract.GetContracts(shid, crop_year, 0);

            if (contractList != null && contractList.Count > 0)
            {
                TableHeaderRow headerRow = new TableHeaderRow();

                TableHeaderCell h1 = new TableHeaderCell();
                h1.Text ="Contract No";
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
                downloadContracts.Style["display"] = "static";

                TableHeaderRow headerRow = new TableHeaderRow();

                TableHeaderCell hshid = new TableHeaderCell();
                hshid.Text = "SHID";
                headerRow.Cells.Add(hshid);

                TableHeaderCell h1 = new TableHeaderCell();
                h1.Text ="Sort";
                headerRow.Cells.Add(h1);

                TableHeaderCell h2 = new TableHeaderCell();
                h2.Text = "Individual";
                headerRow.Cells.Add(h2);

                TableHeaderCell h3 = new TableHeaderCell();
                h3.Text = "Individual Id";
                h3.Attributes.Add("style", "display: none");
                headerRow.Cells.Add(h3);

                TableHeaderCell h4 = new TableHeaderCell();
                h4.Text = "Assigned Percentage";
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
                    TableRow row = new TableRow();

                    TableCell cellshid = new TableCell();
                    TextBox SHID = new TextBox();
                    SHID.ID = "SHID_" + i;
                    SHID.Width = 50;
                    SHID.MaxLength = 6;
                    SHID.Attributes.Add("runat", "server");
                    SHID.Attributes.Add("class", "shid");
                    SHID.Text = Convert.ToString(this.IndTable[i].SHID).Trim();
                    if (SHID.Text == "0")
                        SHID.Text = this.MySHID;
                    cellshid.Controls.Add(SHID);
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
                    string fName = this.IndTable[i].FullName;
                    if (fName == null || fName.Length == 0)
                    {
                        List<Individual> inds = PACData.GetPACIndividuals(this.IndTable[i].IndividualID, null);
                        if (inds != null && inds.Count > 0)
                            fName = ((Individual)inds[0]).FullName;
                    }
                    cell2.Text = fName;
                    row.Cells.Add(cell2);

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

        protected void btnResolveShid_Click(object sender, EventArgs e)
        {
            int crop_year = Convert.ToInt16(ddlCropYear.Text);
            string shid = txtSHID.Text;

            ResetPage();

            if (shid != null && shid.Length > 0)
            {
                List<ListAddressItem> addrList = BeetDataAddress.AddressGetInfo(Convert.ToInt32(shid), 0, crop_year);

                if (addrList.Count > 0)
                {
                    txtSHID.Text = shid;
                    this.MySHID = shid;

                    ListAddressItem item = addrList[0];

                    lblBusName.Text = item.BusName;

                    switch (Convert.ToString(item.AddressType))
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
                }

                uplShid.Update();

                PACAgreement pac = PACData.GetPACAgreement(shid, crop_year);
                if (pac != null)
                {
                    pACContibution.Text = Convert.ToString(pac.Contribution);
                    pACDate.Text = pac.PACDate;
                    indTableField.Value = pac.IndividualsString;
                    this.IndTable = pac.Individuals;
                }

                ReBuildTable();
                UpdatePACDetails.Update();
            }
        }

        protected void btnAddrOk_Click(object sender, EventArgs e)
        {
            Common.AppHelper.HideWarning(addrWarning);
            
            // If we have a selected address, use the shid for our main page.
            if (lstAddressName.SelectedItem != null) {

                int crop_year = Convert.ToInt16(ddlCropYear.Text);
                string shid = txtAddrSHID.Text;

                ResetPage();

                txtSHID.Text = shid;
                lblBusName.Text = txtAddrBName.Text;
                this.MySHID = shid;

                switch (Convert.ToString(txtAddrType.Value))
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
                    pACContibution.Text = Convert.ToString(pac.Contribution);
                    pACDate.Text = pac.PACDate;
                    indTableField.Value = pac.IndividualsString;
                    this.IndTable = pac.Individuals;
                }

                ReBuildTable();
                UpdatePACDetails.Update();
            }

            CloseAndResolve("AddressFinder");
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

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.MySHID != null && this.MySHID.Length > 0)
            {
                btnResolveShid_Click(sender, e);
            }
        }

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
                    txtAddrType.Value = Convert.ToString(item.AddressType);
                }
            }
            catch (System.Exception ex)
            {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                ((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
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
            }
        }

        protected void btnIndAdd_Click(object sender, EventArgs e)
        {
            pacMessages.InnerText = "";

            if (newIndividualName != null && newIndividualName.Text.Length > 1)
            {
                int userId = PACData.SaveIndividual(new Individual(0, newIndividualName.Text));
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

        protected void Individual_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}