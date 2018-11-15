using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServiceApp
{
    public class MojaBaza
    {
        public static Dictionary<string, Admin> admini = new Dictionary<string, Admin>();
        public static Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        public static Dictionary<string, Radnik> radnici = new Dictionary<string, Radnik>();
        public static List<string> listaZahtevaZaDozvoljenimMinusom = new List<string>();

        public static double minimalnaPotrosnja;
        public static double sumaKojuDobijaKorisnikZaDozvMinus;
        public static double provizijaZaIznoseDo3000;
        public static double provizijaZaIsnosePreko3000;
        public static double provizijaZaMomTransfer;
        public static int skalabilneSekunde;
        public static double provizijaZaMesecnoOdrzavanje;
        public static int brojac;

        public static void UcitajIzTxtProvizije()
        {
            string text = System.IO.File.ReadAllText("Provizije.txt");
            string[] tokens = text.Split(';');
            minimalnaPotrosnja = Double.Parse(tokens[0]);
            sumaKojuDobijaKorisnikZaDozvMinus = Double.Parse(tokens[1]);
            provizijaZaIznoseDo3000 = Double.Parse(tokens[2]);
            provizijaZaIsnosePreko3000 = Double.Parse(tokens[3]);
            provizijaZaMomTransfer = Double.Parse(tokens[4]);
            skalabilneSekunde = Int32.Parse(tokens[5]);
            provizijaZaMesecnoOdrzavanje = Double.Parse(tokens[6]);
            brojac = Int32.Parse(tokens[7]);
        }
    }
}
