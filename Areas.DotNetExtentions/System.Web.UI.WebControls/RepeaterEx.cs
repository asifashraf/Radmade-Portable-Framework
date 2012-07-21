using System.Collections.Generic;
using System.Web.UI.WebControls;

    public static class RepeaterEx
    {
		#region Methods (1) 

		// Public Methods (1) 

        public static void Bind<T>(this Repeater rp,
            IEnumerable<T> list)
        {
            rp.DataSource = list;
            rp.DataBind();
        }

		#endregion Methods 

}
