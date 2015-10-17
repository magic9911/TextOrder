using System;
using System.Reflection;
using GoldStar.Lib.Helper;

namespace GoldStar.Lib.Helper {

    /// <summary>
    /// Class that can bring Infomations to.
    /// </summary>
    public class Info {
        private static AssemblyInfo myAsmInfo = new AssemblyInfo(Assembly.GetEntryAssembly());
        //private static Assembly assembly = Assembly.GetExecutingAssembly();

        public static string getProductName() {
            return myAsmInfo.ProductTitle;
        }

        public static string getVersion() {
            return Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();
        }

        public static string getAppFolder() {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}