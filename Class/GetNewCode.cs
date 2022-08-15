using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicApp.Class
{
    public static class GetNewCode
    {
        public static string GetNewPatienttCode()
        {
            string MaxCode;
            using (var db = new DAL.DBDataContext())
                MaxCode = db.Patients.OrderByDescending(x => x.ID).Select(x => x.PatientCode ?? "0").FirstOrDefault();
            return GetNetNumberInString(MaxCode);
        }
        private static string GetNetNumberInString(string Code)
        {
            if (Code?.Length == 0 || Code == null)
                return "1";
            string str1 = "";
            foreach (char c in Code)
            {
                str1 = char.IsDigit(c) ? str1 + c.ToString() : "";
            }
            if (str1?.Length == 0)
                return Code + "1";
            string str2 = str1.Insert(0, "1");
            str2 = (Convert.ToInt64(str2) + 1).ToString();
            string str3 = str2[0] == '1' ? str2.Remove(0, 1) : str2.Remove(0, 1).Insert(0, "1");
            int index = Code.LastIndexOf(str1);
            Code = Code.Remove(index);
            Code = Code.Insert(index, str3);
            return Code;
        }
    }
}