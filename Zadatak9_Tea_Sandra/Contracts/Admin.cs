using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Contracts
{
    public class Admin
    {
        private string ime;
        private string prezime;
        private string korisnickoIme;
        private string sifra;
     
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

        public Admin(string i, string p, string k, string s)
        {
            Ime = i;
            Prezime = p;
            KorisnickoIme = k;
            Sifra = s;
        }
    }
}
