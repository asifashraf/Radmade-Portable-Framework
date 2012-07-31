using System.Text;

    public static class StringBuilderX
    {
		#region Methods (4) 

		// Public Methods (4) 

        public static void AddLine(this StringBuilder sb)
        {
            sb.Append("".NewLine());
        }

        public static void AddTab(this StringBuilder sb)
        {
            sb.Append("\t");
        }

        //public static void Append(this StringBuilder sb, string text, params object[] parameters)
        //{
        //    string[] arrStrings = new string[parameters.Length];
        //    for (int i = 0; i < parameters.Length; i++)
        //    {
        //        arrStrings[i] = parameters[i].ToString();
        //    }

        //    sb.Append(string.Format(text, arrStrings));
        //    arrStrings = null;
        //    text = null;
        //    parameters = null;
        //}

        //public static void AppendLine(this StringBuilder sb, string text, params object[] parameters)
        //{
        ////    string[] arrStrings = new string[parameters.Length];
        ////    for (int i = 0; i < parameters.Length; i++)
        ////    {
        ////        arrStrings[i] = parameters[i].ToString();
        ////    }
        //    sb.AppendLine(string.Format(text, parameters));
        //    //arrStrings = null;
        //    //text = null;
        //    //parameters = null;
        //    parameters = null;
        //}

		#endregion Methods 
    }

