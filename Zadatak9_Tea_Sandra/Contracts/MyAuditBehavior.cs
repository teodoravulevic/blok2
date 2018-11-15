using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Contracts
{
    public sealed class MyAuditBehavior
    {

        static MyAuditBehavior()
        {
            if (!EventLog.SourceExists("Banka"))                                                  
            {
                EventLog.CreateEventSource("Banka", "BankaLog");
            }
            MyLogger = new EventLog("BankaLog", Environment.MachineName, "Banka");
        }

        private static EventLog MyLogger { get; set; }

        public static void LogNeuspesnaAutorizacija()       
        {
            MyLogger.WriteEntry($"Neuspesna autorizacija.", EventLogEntryType.Warning);
        }

        public static void LogUspesnaAutorizacija()    
        {
            MyLogger.WriteEntry($"Uspesna autorizacija", EventLogEntryType.Information);
        }

        public static void LogUspesnaTransakcija()     
        {
            MyLogger.WriteEntry($"Uspesna transakcija.", EventLogEntryType.Information);
        }
        public static void LogNeuspesnaTransakcija()
        {
            MyLogger.WriteEntry($"Neuspesna transakcija.", EventLogEntryType.Warning);
        }

        public static void LogUspesnoMesecnoSkidanjeProvizije()
        {
            MyLogger.WriteEntry($"Uspesno mesecno skidanje provizije sa racuna.", EventLogEntryType.Information);
        }
        public static void LogNeuspesnoMesecnoSkidanjeProvizije()
        {
            MyLogger.WriteEntry($"Neuspesno mesecno skidanje provizije sa racuna.", EventLogEntryType.Warning);
        }

    }
}
