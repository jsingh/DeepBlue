using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using System.IO;

namespace DeepBlue.ImportData {
    public class Util {

		public static void Log(string log) {
			try {
				if (System.IO.File.Exists(Globals.ConsoleLogFile) == false) {
					log = "<div style='background-color:black'>";
					using (TextWriter tw = new StreamWriter(Globals.ConsoleLogFile, true)) {
						tw.WriteLine(log + Environment.NewLine + Environment.NewLine);
						tw.Flush();
						tw.Close();
					}
				}
				Console.WriteLine(log);
				log = "<font style='color:" + Console.ForegroundColor.ToString() + "'>" + log + "</font><br/>";
				using (TextWriter tw = new StreamWriter(Globals.ConsoleLogFile, true)) {
					tw.WriteLine(log + Environment.NewLine + Environment.NewLine);
					tw.Flush();
					tw.Close();
				}
			}
			catch { }
		}

        public static bool ParseAddress(string address, out string[] parts) {
            parts = new string[3];
            if (string.IsNullOrEmpty(address)) {
                try {
                    int lastIndex = 0;
                    lastIndex = address.LastIndexOf(",");
                    parts[0] = address.Substring(0, lastIndex);
                    address = address.Substring(lastIndex + 1);
                    string[] subParts = address.Trim().Split(' ');
                    if (subParts.Length == 1) {
                        parts[1] = subParts[0];
                    } else if (subParts.Length == 2) {
                        parts[1] = subParts[0];
                        parts[2] = subParts[1];
                    }
                } catch {
                    return false;
                }
            } else {
                return false;
            }
            return true;
        }

        public static void SetAddress(string address, Address addr) {
            addr.Country = Globals.DefaultCountryID;
            addr.City = Globals.DefaultCity;
            addr.State = Globals.DefaultStateID;
            addr.PostalCode = Globals.DefaultZip;
            try {
                string[] parts = new string[3];
                if (ParseAddress(address, out parts)) {
                    addr.City = parts[0];
                    addr.PostalCode = parts[2];
                    addr.State = Globals.States.Where(x => x.Abbr == parts[1].ToUpper().Trim()).First().StateID;
                }
            } catch {

            }
        }

        public static void WriteError(string error) {
			using (TextWriter tw = new StreamWriter(Globals.ConsoleErrorLogFile, true)) {
				tw.WriteLine(Environment.NewLine + error);
				tw.Flush();
				tw.Close();
			}
            Console.ForegroundColor = ConsoleColor.Red;
            Util.Log(error);
            Console.ResetColor();
        }

        public static void WriteNewEntry(string error) {
			using (TextWriter tw = new StreamWriter(Globals.ConsoleSuccessLogFile, true)) {
				tw.WriteLine(Environment.NewLine + error);
				tw.Flush();
				tw.Close();
			}
            Console.ForegroundColor = ConsoleColor.Green;
            Util.Log(error);
            Console.ResetColor();
        }

        public static void WriteWarning(string error) {
			using (TextWriter tw = new StreamWriter(Globals.ConsoleWarningLogFile, true)) {
				tw.WriteLine(Environment.NewLine + error);
				tw.Flush();
				tw.Close();
			}
			Console.ForegroundColor = ConsoleColor.Yellow;
            Util.Log(error);
            Console.ResetColor();
        }

    }
}
