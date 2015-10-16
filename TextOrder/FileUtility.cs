using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextOrder {
    public class FileUtility {

        private FileUtility() {

        }

        public static string ReadFile(string path) {
            return File.ReadAllText(path);
        }

        public static bool WriteFile(string path, string contents) {
            try {
                File.WriteAllText(path, contents);
                return true;
            } catch (Exception ex) {

                return false;
            }
            
        }
    }
}
