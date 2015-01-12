using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WSCIEMP.Common {
    /// <summary>
    /// Summary description for Helpers
    /// </summary>
    public static class UILib {

        private const string MOD_NAME = "Common.UILib.";

        public static void SetInputControlHighlight(Control container, string className, bool onlyTextBoxes) {

            const string METHOD_NAME = "SetInputControlHighlight";

            try {
                foreach (Control ctl in container.Controls) {

                    if ((onlyTextBoxes && ctl is TextBox) ||
                        (!onlyTextBoxes && (ctl is TextBox || ctl is DropDownList ||
                        ctl is ListBox || ctl is CheckBox || ctl is RadioButton ||
                        ctl is RadioButtonList || ctl is CheckBoxList))) {

                        WebControl wctl = ctl as WebControl;
                        if (wctl.CssClass.Length == 0) {
                            wctl.Attributes.Add("onfocus", string.Format("this.className = '{0}';", className));
                            wctl.Attributes.Add("onblur", "this.className = '';");
                        }
                    } else {
                        if (ctl.Controls.Count > 0) {
                            SetInputControlHighlight(ctl, className, onlyTextBoxes);
                        }
                    }
                }
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static string GetDropDownText(System.Web.UI.WebControls.DropDownList ddl) {

            string choice = "";
            foreach (System.Web.UI.WebControls.ListItem item in ddl.Items) {
                if (item.Selected) {
                    choice = item.Text;
                    break;
                }
            }
            return choice;
        }

        public static string GetListText(System.Web.UI.WebControls.ListBox lst, string seperator) {

            string choice = "";
            foreach (System.Web.UI.WebControls.ListItem item in lst.Items) {
                if (item.Selected) {
                    if (choice.Length > 0) {
                        choice += seperator;
                    }
                    choice += item.Text;
                }
            }
            return choice;
        }

        public static string GetDropDownValue(System.Web.UI.WebControls.DropDownList ddl) {

            string choice = "";
            foreach (System.Web.UI.WebControls.ListItem item in ddl.Items) {
                if (item.Selected) {
                    choice = item.Value;
                    break;
                }
            }
            return choice;
        }

        public static string GetListValues(System.Web.UI.WebControls.ListBox lst) {

            string choice = "";
            foreach (System.Web.UI.WebControls.ListItem item in lst.Items) {
                if (item.Selected) {
                    if (choice.Length > 0) {
                        choice += "," + item.Value;
                    } else {
                        choice += item.Value;
                    }
                }
            }
            return choice;
        }

        public static bool SelectDropDown(System.Web.UI.WebControls.DropDownList ddl, int index) {

            int selectedIndex = -1;
            for (int i = 0; i < ddl.Items.Count; i++) {

                System.Web.UI.WebControls.ListItem item = ddl.Items[i];

                if (i == index) {
                    item.Selected = true;
                    selectedIndex = ddl.Items.IndexOf(item);
                } else {
                    item.Selected = false;
                }
            }

            ddl.SelectedIndex = selectedIndex;
            return true;
        }

        public static bool SelectListIndex(System.Web.UI.WebControls.ListBox lst, string[] indexList) {

            for (int i = 0; i < lst.Items.Count; i++) { lst.Items[i].Selected = false; }

            foreach (string s in indexList) {
                if (s.Length > 0) {
                    System.Web.UI.WebControls.ListItem item = lst.Items[Convert.ToInt32(s)];
                    item.Selected = true;
                }
            }

            return true;
        }

        public static bool SelectDropDown(System.Web.UI.WebControls.DropDownList ddl, string text) {

            int selectedIndex = 0;
            bool status = true;
            bool found = false;

            foreach (System.Web.UI.WebControls.ListItem item in ddl.Items) {
                if (item.Text == text) {
                    item.Selected = true;
                    selectedIndex = ddl.Items.IndexOf(item);
                    found = true;
                } else {
                    item.Selected = false;
                }
            }

            // text was not found, add it to the list box.
            if (!found && text.Length > 0) {
                ddl.Items.Add(text);
                ddl.Items[ddl.Items.Count - 1].Selected = true;
                status = false;
                selectedIndex = ddl.Items.Count - 1;
            }

            ddl.SelectedIndex = selectedIndex;
            return status;
        }

        public static bool SelectDropDownValue(System.Web.UI.WebControls.DropDownList ddl, string selectedValue) {

            int selectedIndex = 0;
            bool status = true;
            bool found = false;

            foreach (System.Web.UI.WebControls.ListItem item in ddl.Items) {
                if (item.Value == selectedValue) {
                    item.Selected = true;
                    selectedIndex = ddl.Items.IndexOf(item);
                    found = true;
                } else {
                    item.Selected = false;
                }
            }

            if (found) {
                ddl.SelectedIndex = selectedIndex;
            }
            return status;
        }

        public static bool SelectListText(System.Web.UI.WebControls.ListBox lst, string[] textList) {

            bool status = true;
            bool found = false;

            for (int i = 0; i < lst.Items.Count; i++) { lst.Items[i].Selected = false; }
            foreach (string text in textList) {

                found = false;
                foreach (System.Web.UI.WebControls.ListItem item in lst.Items) {
                    if (item.Text == text) {
                        item.Selected = true;
                        found = true;
                    }
                }

                // Any text not found in the list, are added to the list and 
                // this resets the status.
                if (!found && text.Length > 0) {
                    lst.Items.Add(text);
                    lst.Items[lst.Items.Count - 1].Selected = true;
                    status = false;
                }
            }

            return status;
        }

        public static void FillYesNo(System.Web.UI.WebControls.DropDownList ddl, string defaultText) {

            string itemText = "";
            int selectedIndex = 0;

            ddl.Items.Clear();
            for (int i = 0; i <= 1; i++) {

                if (i == 0) {
                    itemText = "No";
                } else {
                    itemText = "Yes";
                }
                ddl.Items.Add(itemText);

                if (itemText == defaultText) {
                    ddl.Items[i].Selected = true;
                    selectedIndex = i;
                }
            }

            ddl.SelectedIndex = selectedIndex;
        }

        public static void FillYesNoBlank(System.Web.UI.WebControls.DropDownList ddl, string defaultText) {

            string itemText = "";
            int selectedIndex = 0;

            ddl.Items.Clear();
            ddl.Items.Add("");
            for (int i = 0; i <= 1; i++) {

                if (i == 0) {
                    itemText = "No";
                } else {
                    itemText = "Yes";
                }
                ddl.Items.Add(itemText);

                if (itemText == defaultText) {
                    ddl.Items[i].Selected = true;
                    selectedIndex = i;
                }
            }

            ddl.SelectedIndex = selectedIndex;
        }
    }
}