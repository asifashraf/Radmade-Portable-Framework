using System;
using System.Web.UI.WebControls;
using System.Web.UI;


    public static class GridViewX
    {
		#region Methods (4) 

		// Public Methods (4) 

        public static Control GetColumnControl(this GridView grid, int rowIndex, int cellIndex)
        {
            GridViewRow row = grid.Rows[rowIndex];
            return row.Cells[cellIndex].Controls[0];            
        }

        public static object GetColumnValue(this GridView grid, int rowIndex, int cellIndex)
        {
            Control ctrl = grid.GetColumnControl(rowIndex, cellIndex);
            Type t = ctrl.GetType();
            if (t == typeof(TextBox))
            {
                return ((TextBox)ctrl).Text;
            }
            if (t == typeof(CheckBox))
            {
                return ((CheckBox)ctrl).Checked;
            }
            if (t == typeof(Label))
            {
                return ((Label)ctrl).Text;
            }
            if (t == typeof(DropDownList))
            {
                return ((DropDownList)ctrl).SelectedItem.Value;
            }
            throw new Exception("{0} type is not handled in GetColumnValue Method. Do that first.");
        }

        public static object GetCurrentPrimaryKey(this GridView grid,
            string whichPrimaryKey, int rowIndex)
        {
            return grid.DataKeys[rowIndex][whichPrimaryKey].ToString();
        }

        public static void HandleGridEditingEvent(this GridView grid, int newEditIndex)
        {
            grid.EditIndex = newEditIndex;
        }

		#endregion Methods 
    }

