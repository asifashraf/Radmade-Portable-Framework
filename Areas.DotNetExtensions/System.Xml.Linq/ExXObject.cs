using System.Xml.Linq;
using RadApi.Exts.DotNetExts.Support.Enums;


public static class ExXObject
    {
		#region Methods (1) 

		// Public Methods (1) 

        public static XDeclaration GetXDeclaration(this XObject doc,
            XVersion version, XEncoding encoding, bool standAlone)
        {
            return new XDeclaration(version.GetStringValue(), encoding.GetStringValue(), true.ToYesNo(standAlone, TextNotation.Lower));
        }

		#endregion Methods 
    }

