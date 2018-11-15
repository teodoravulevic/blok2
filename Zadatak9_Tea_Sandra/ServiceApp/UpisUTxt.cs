using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServiceApp
{
    public static class UpisUTxt
    {
        public static void UpisOpomene(string korIme)
        {
            string[] lines = { MojaBaza.korisnici[korIme].BrojRacunaKorisnika, MojaBaza.korisnici[korIme].Ime, MojaBaza.korisnici[korIme].Prezime,
                MojaBaza.korisnici[korIme].KorisnickoIme, MojaBaza.korisnici[korIme].DozvoljeniMinus.ToString(), MojaBaza.korisnici[korIme].StanjeRacuna.ToString()};

            File.WriteAllLines("Opomene.txt", lines);
        }
    }
}
