using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Contracts
{
    public class Korisnik
    {
        private int brTr;
        private string ime;
        private string prezime;
        private string korisnickoIme;
        private string sifra;
        private string brojRacunaKorisnika;
        private double dozvoljeniMinus;
        private double stanjeRacuna;
        private Dictionary<int, double> prethodneTransakcije;
        private double isplate;
        private double uplate;
        private int brojacZaOpomenu;

        public int BrojacZaOpomenu
        {
            get { return brojacZaOpomenu; }
            set { brojacZaOpomenu = value; }
        }

        public double Isplate
        {
            get { return isplate; }
            set { isplate = value; }
        }

        public int BrTr
        {
            get { return brTr; }
            set { brTr = value; }
        }

        public double Uplate
        {
            get { return uplate; }
            set { uplate = value; }
        }

        public Dictionary<int, double> PrethodneTransakcije
        {
            get { return prethodneTransakcije; }
            set { prethodneTransakcije = value; }
        }

        public string BrojRacunaKorisnika
        {
            get { return brojRacunaKorisnika; }
            set { brojRacunaKorisnika = value; }
        }

        public double DozvoljeniMinus
        {
            get { return dozvoljeniMinus; }
            set { dozvoljeniMinus = value; }
        }

        public double StanjeRacuna
        {
            get { return stanjeRacuna; }
            set { stanjeRacuna = value; }
        }

        public string Ime
        {
            get { return ime; }
            set { ime = value; }
        }

        public string Prezime
        {
            get { return prezime; }
            set { prezime = value; }
        }

        public string KorisnickoIme
        {
            get { return korisnickoIme; }
            set { korisnickoIme = value; }
        }

        public string Sifra
        {
            get { return sifra; }
            set { sifra = value; }
        }

        public Korisnik(string i, string p, string k, string s, string b, double sR)
        {
            Ime = i;
            Prezime = p;
            KorisnickoIme = k;
            Sifra = s;
            BrojRacunaKorisnika = b;
            StanjeRacuna = sR;
            brTr = 0;
            brojacZaOpomenu = 0;
            prethodneTransakcije = new Dictionary<int, double>();
            dozvoljeniMinus = 0;
            uplate = 0;
            isplate = 0;
        }
    }
}
