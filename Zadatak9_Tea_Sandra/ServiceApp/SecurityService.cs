using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Security.Principal;

namespace ServiceApp
{
    public class SecurityService : Contracts.ISecurityService
    {
        public static bool ZavrsiThread { get; set; } = false;

        public void Ispis()
        {
            Console.WriteLine("------------------BAZA--------------------");
            foreach (var k in MojaBaza.korisnici.Keys)
            {
                Console.WriteLine("U bazi postoje korisnici sa korisnickim imenom: " + k + "\n");
            }

            Console.WriteLine("-------------------------------------");
            foreach (var r in MojaBaza.radnici.Keys)
            {
                Console.WriteLine("U bazi postoje radnici sa korisnickim imenom: " + r + "\n");
            }

            Console.WriteLine("-------------------------------------");
            foreach (var a in MojaBaza.admini.Keys)
            {
                Console.WriteLine("U bazi postoje admini sa korisnickim imenom: " + a + "\n");
            }
        }
        
        public bool DodajKorisnika(string ime, string prezime, string sifra, string korIme, string brojRacuna, double stanje)
        {
            IIdentity id = Thread.CurrentPrincipal.Identity;           
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);

            if (!principal.IsInRole("Radnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            Console.WriteLine("Dodavanje korisnika od strane radnika...\n");

            if (!MojaBaza.korisnici.ContainsKey(korIme))
            {
                MojaBaza.korisnici.Add(korIme, new Korisnik(ime, prezime, korIme, Hash(sifra), brojRacuna, stanje));
                Console.WriteLine("Uspesno dodavanje korisnika \nIme : " + MojaBaza.korisnici[korIme].Ime + "\nPrezime: " + MojaBaza.korisnici[korIme].Prezime + "\nKorisnicko ime: " + MojaBaza.korisnici[korIme].KorisnickoIme);
                return true;
            }
            else
            {
                Console.WriteLine("Vec postoji korisnik sa zadatim korisnickim imenom " + MojaBaza.korisnici[korIme].KorisnickoIme + "\n");
                return false;
            }
            
        }

        public bool DodajRadnika(string ime, string prezime, string korIme, string sifra)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;          
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);

            if (!principal.IsInRole("Admiri"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            Console.WriteLine("Dodavanje radnika od strane admina...\n");

            if (!MojaBaza.radnici.ContainsKey(korIme))
            {
                MojaBaza.radnici.Add(korIme, new Radnik(ime, prezime, korIme, Hash(sifra)));
                Console.WriteLine("Uspesno dodavanje radnika \nIme: " + MojaBaza.radnici[korIme].Ime + "\nPrezime: " + MojaBaza.radnici[korIme].Prezime + "\nSifra: " + MojaBaza.radnici[korIme].Sifra + "\nKorisnicko ime: " + MojaBaza.radnici[korIme].KorisnickoIme);

                return true;
            }
            else
            {
                Console.WriteLine("Vec postoji radnik sa zadatim korisnickim imenom " + MojaBaza.radnici[korIme].KorisnickoIme + "\n");
                return false;
            }
            
        }

        public bool Isplata(string korIme, double novac)
        {
            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Korisnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (!MojaBaza.korisnici.ContainsKey(korIme))
            {
                Console.WriteLine("Ne postoji korisnik na ciji racun radnik treba da isplati pare...\n");
                MyAuditBehavior.LogNeuspesnaTransakcija();
                return false;
            }
            else
            {
                if (MojaBaza.korisnici[korIme].StanjeRacuna >= novac)
                {
                    MojaBaza.korisnici[korIme].StanjeRacuna -= novac;
                    MojaBaza.korisnici[korIme].PrethodneTransakcije.Add(MojaBaza.korisnici[korIme].BrTr, novac);
                    MojaBaza.korisnici[korIme].BrTr++;
                    MojaBaza.korisnici[korIme].Isplate += novac;
                    Console.WriteLine("Stanje na racunu korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " nakon isplate je: " + MojaBaza.korisnici[korIme].StanjeRacuna);
                    MyAuditBehavior.LogUspesnaTransakcija();
                    return true;
                }
                else
                {
                    Console.WriteLine("Korisnik " + korIme + " nema dovoljno novca za isplatu.\n");
                    MyAuditBehavior.LogNeuspesnaTransakcija();
                    return false;
                }
            }
            
        }

        public bool KorisnikTraziDozvoljeniMinus(string korIme)
        {
            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);
            if (!principal.IsInRole("Radnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            Console.WriteLine("Korisnik " + MojaBaza.korisnici[korIme].KorisnickoIme + " trazi dozvoljeni minus...");
            //dodajemo u listu zahteva za dozvoljenim minusom
            MojaBaza.listaZahtevaZaDozvoljenimMinusom.Add(korIme);

            foreach(string var in MojaBaza.listaZahtevaZaDozvoljenimMinusom)
            {
                Console.WriteLine("U listi se nalazi zahtev korisnika sa korisnickim imenom: " + var);
            }

            if (MojaBaza.korisnici[korIme].Isplate >= 100000)
            {
                Console.WriteLine("Radnik je odobrio zahtev za dozvoljeni minus korisniku sa korisnickim imenom " + korIme);
                MojaBaza.korisnici[korIme].DozvoljeniMinus = 0.1 * MojaBaza.korisnici[korIme].Isplate;
                Console.WriteLine("Stanje korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " nakon ispate je: " + MojaBaza.korisnici[korIme].DozvoljeniMinus.ToString());
                return true;
            }

            else
            {
                Console.WriteLine("Radnik je odobrio zahtev za dozvoljeni minus korisniku sa korisnickim imenom " + korIme);
                //uklanjamo iz reda zahteva
                MojaBaza.listaZahtevaZaDozvoljenimMinusom.Remove(korIme);
                return false;
            }
        }

        public bool MenjajKorisnika(string ime, string prezime, string sifra, string korIme)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Radnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (MojaBaza.korisnici.ContainsKey(korIme))
            {
                MojaBaza.korisnici[korIme].Ime = ime;
                MojaBaza.korisnici[korIme].Prezime = prezime;
                MojaBaza.korisnici[korIme].Sifra = Hash(sifra);
                Console.WriteLine("Uspesno menjanje korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " od strane radnika...\n");
                Console.WriteLine("Novi podaci korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + "su: \nIme: " + MojaBaza.korisnici[korIme].Ime + "\nPrezime: " + MojaBaza.korisnici[korIme].Prezime + "\nSifra: " + MojaBaza.korisnici[korIme].Sifra);
                return true;
            }
            else
            {
                Console.WriteLine("Ne postoji korisnik sa zadatim korisnickim imenom " + MojaBaza.korisnici[korIme].KorisnickoIme + " za menjanje...\n");
                return false;
            }
        }

        public bool MenjajPostojecegRadnika(string ime, string prezime, string korIme, string sifra)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Admiri"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (MojaBaza.radnici.ContainsKey(korIme))
            {
                MojaBaza.radnici[korIme].Ime = ime;
                MojaBaza.radnici[korIme].Prezime = prezime;
                MojaBaza.radnici[korIme].Sifra = Hash(sifra);
                Console.WriteLine("Uspesno menjanje radnika " + MojaBaza.radnici[korIme].KorisnickoIme + " od strane admina...\n");
                Console.WriteLine("Novi podaci korisnika " + MojaBaza.radnici[korIme].KorisnickoIme + "su: \nIme: " + MojaBaza.radnici[korIme].Ime + "\nPrezime: " + MojaBaza.radnici[korIme].Prezime + "\nSifra: " + MojaBaza.radnici[korIme].Sifra);
                return true;
            }
            else
            {
                Console.WriteLine("Ne postoji radnik sa zadatim korisnickim imenom " + MojaBaza.radnici[korIme].KorisnickoIme + " za menjanje...\n");
                return false;
            }
            
        }

        public bool ObrisiKorisnika(string korIme)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Radnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (MojaBaza.korisnici.ContainsKey(korIme))
            {
                Console.WriteLine("Uspesno brisanje korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + "\n");
                MojaBaza.korisnici.Remove(korIme);
                
                return true;
            }
            else
            {
                Console.WriteLine("Ne postoji korisnik sa zadatim korisnickim imenom " + MojaBaza.korisnici[korIme].KorisnickoIme+ "\n");
                return false;
            }
        }

        public bool ObrisiRadnika(string korIme)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Admiri"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (MojaBaza.radnici.ContainsKey(korIme))
            {
                Console.WriteLine("Uspesno brisanje radnika " + MojaBaza.radnici[korIme].KorisnickoIme + "\n");
                MojaBaza.radnici.Remove(korIme);
                
                return true;
            }
            else
            {
                Console.WriteLine("Ne postoji radnik sa zadatim korisnickim imenom " + MojaBaza.radnici[korIme].KorisnickoIme + "\n");
                return false;
            }
        }

        public bool Uplata(string korIme,string korImeUplatioc,double novac, int izbor)
        {
            //string temp = "";

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Korisnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (!MojaBaza.korisnici.ContainsKey(korIme))
            {
                Console.WriteLine("Ne postoji korisnik na ciji racun radnik treba da uplati pare...\n");
                MyAuditBehavior.LogNeuspesnaTransakcija();
                return false;
            }
            else
            {
                /*
                MojaBaza.korisnici[korIme].StanjeRacuna += novac;
                MojaBaza.korisnici[korIme].PrethodneTransakcije.Add(MojaBaza.korisnici[korIme].BrTr++, novac);
                Console.WriteLine("Stanje na racunu korisnika nakon uplate je: " + MojaBaza.korisnici[korIme].StanjeRacuna);
                return true;*/
                
                switch (izbor)
                {
                    case 1:
                        MojaBaza.korisnici[korIme].StanjeRacuna += novac;
                        MojaBaza.korisnici[korIme].PrethodneTransakcije.Add(MojaBaza.korisnici[korIme].BrTr, novac);
                        MojaBaza.korisnici[korIme].BrTr++;
                        Console.WriteLine("Stanje na racunu korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " nakon uplate je: " + MojaBaza.korisnici[korIme].StanjeRacuna);
                        MyAuditBehavior.LogUspesnaTransakcija();
                        break;
                    case 2:

                        if (MojaBaza.korisnici[korImeUplatioc].StanjeRacuna > 100)
                        {
                            MojaBaza.korisnici[korIme].StanjeRacuna += novac;
                            MojaBaza.korisnici[korImeUplatioc].StanjeRacuna -= 100;
                            MojaBaza.korisnici[korIme].PrethodneTransakcije.Add(MojaBaza.korisnici[korIme].BrTr, novac);
                            MojaBaza.korisnici[korIme].BrTr++;
                            MyAuditBehavior.LogUspesnaTransakcija();
                            Console.WriteLine("Stanje na racunu korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " nakon uplate je: " + MojaBaza.korisnici[korIme].StanjeRacuna);
                        }
                        else
                        {
                            Console.WriteLine("Nemoguce uplatiti novac");
                            MyAuditBehavior.LogNeuspesnaTransakcija();
                            return false;
                        }
                        break;
                    case 3:
                        if (MojaBaza.korisnici[korImeUplatioc].StanjeRacuna > 0.3 * novac)
                        {
                            MojaBaza.korisnici[korIme].StanjeRacuna += novac;
                            MojaBaza.korisnici[korImeUplatioc].StanjeRacuna -= 0.3 * novac;
                            // Console.WriteLine("Stanje na racunu korisnika nakon uplate je: " + MojaBaza.korisnici[korIme].StanjeRacuna);
                            MojaBaza.korisnici[korIme].PrethodneTransakcije.Add(MojaBaza.korisnici[korIme].BrTr, novac);
                            MojaBaza.korisnici[korIme].BrTr++;
                            Console.WriteLine("Stanje na racunu korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " nakon uplate je: " + MojaBaza.korisnici[korIme].StanjeRacuna);
                            MyAuditBehavior.LogUspesnaTransakcija();
                        }
                        else
                        {
                            Console.WriteLine("Nemoguce uplatiti novac");
                            MyAuditBehavior.LogNeuspesnaTransakcija();
                            return false;
                        }
                        break;
                    case 4:
                        if (MojaBaza.korisnici[korImeUplatioc].StanjeRacuna > 250)
                        {
                            MojaBaza.korisnici[korIme].StanjeRacuna += novac;
                            MojaBaza.korisnici[korImeUplatioc].StanjeRacuna -= 250;
                            MojaBaza.korisnici[korIme].PrethodneTransakcije.Add(MojaBaza.korisnici[korIme].BrTr, novac);
                            MojaBaza.korisnici[korIme].BrTr++;
                            Console.WriteLine("Stanje na racunu korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " nakon uplate je: " + MojaBaza.korisnici[korIme].StanjeRacuna);
                            MyAuditBehavior.LogUspesnaTransakcija();
                        }
                        else
                        {
                            Console.WriteLine("Nemoguce uplatiti novac");
                            MyAuditBehavior.LogNeuspesnaTransakcija();
                            return false;
                        }
                            break;
                }
                return true;
            }
        }

        public bool MenjajTransakcijuKorisnika(string korIme, double pare, int brojTransakcije)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Radnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (!MojaBaza.korisnici.ContainsKey(korIme))
            {
                Console.WriteLine("Ne postoji korisnik sa zadatim korisnickim imenom " + MojaBaza.korisnici[korIme].KorisnickoIme + "\n");
                return false;
            }

            else
            {
                if (MojaBaza.korisnici[korIme].PrethodneTransakcije.Keys.Contains(brojTransakcije))
                {
                    MojaBaza.korisnici[korIme].PrethodneTransakcije[brojTransakcije] = pare;
                    Console.WriteLine("Transakcija korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " je uspesno promenjena");
                    return true;
                }
                else
                {
                    Console.WriteLine("U listi korisnickih transakcija ne postoji transakcija sa tim brojem...\n");
                    return false;
                }
            }
        }

        public bool ObrisiTransakcijuKorisnika(string korIme, int brojTransakcije)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Radnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return false;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (!MojaBaza.korisnici.ContainsKey(korIme))
            {
                Console.WriteLine("Ne postoji korisnik sa zadatim korisnickim imenom " + MojaBaza.korisnici[korIme].KorisnickoIme + "\n");
                return false;
            }
            else
            {
                if (MojaBaza.korisnici[korIme].PrethodneTransakcije.Keys.Contains(brojTransakcije))
                {
                    MojaBaza.korisnici[korIme].PrethodneTransakcije.Remove(brojTransakcije);
                    return true;
                }
                else
                {
                    Console.WriteLine("U listi korisnickih transakcija ne postoji transakcija sa tim brojem...\n");
                    return false;
                }
            }
        }

        public static void MesecnoSkidanjeProvizije()
        {
            while (!ZavrsiThread)
            {
                foreach(var korisnik in MojaBaza.korisnici)
                {
                    if(korisnik.Value.StanjeRacuna < MojaBaza.provizijaZaMesecnoOdrzavanje)
                    {
                        if (korisnik.Value.BrojacZaOpomenu == MojaBaza.brojac)
                            continue;

                        korisnik.Value.BrojacZaOpomenu++;
                        
                        if(korisnik.Value.BrojacZaOpomenu == MojaBaza.brojac)
                        {
                            UpisUTxt.UpisOpomene(korisnik.Value.KorisnickoIme);
                            MyAuditBehavior.LogNeuspesnoMesecnoSkidanjeProvizije();
                        }
                    }
                    korisnik.Value.StanjeRacuna -= MojaBaza.provizijaZaMesecnoOdrzavanje;
                    MyAuditBehavior.LogUspesnoMesecnoSkidanjeProvizije();
                }

                Thread.Sleep(MojaBaza.skalabilneSekunde*1000);
            }

            return;

            #region comment
            ////provera da li postoji korisnik sa tim korisnickim imenom
            //if (MojaBaza.korisnici.ContainsKey(korIme))
            //{
            //    if (MojaBaza.korisnici[korIme].StanjeRacuna >= MojaBaza.provizijaZaMesecnoOdrzavanje)
            //    {
            //        MojaBaza.korisnici[korIme].StanjeRacuna -= 200;
            //        Console.WriteLine("Preostalo stanje na racunu korisnika " + MojaBaza.korisnici[korIme].KorisnickoIme + " je: " + MojaBaza.korisnici[korIme].StanjeRacuna + "\n");
            //    }
            //    else if (MojaBaza.korisnici[korIme].StanjeRacuna < 200 && MojaBaza.korisnici[korIme].BrojacZaOpomenu < 2)
            //    {
            //        Console.WriteLine("Korisnik " + MojaBaza.korisnici[korIme].KorisnickoIme +  " nema dovoljno sredstava za skidanje sa racuna(200, 00 RSD)");
            //        MojaBaza.korisnici[korIme].BrojacZaOpomenu++;
            //    }
            //    else if (MojaBaza.korisnici[korIme].StanjeRacuna < 200 && MojaBaza.korisnici[korIme].BrojacZaOpomenu == 2)
            //    {
            //        Console.WriteLine("Korisnik " + MojaBaza.korisnici[korIme].KorisnickoIme +  " nema dovoljno sredstava za skidanje sa racuna i salje mu se opomena!\n");
            //        UpisUTxt.UpisOpomene(korIme);
            //        UpisUTxt.IspisiPoruke(korIme);
            //    }
            //    else if(MojaBaza.korisnici[korIme].StanjeRacuna < 200 && MojaBaza.korisnici[korIme].BrojacZaOpomenu > 2)
            //    {
            //        Console.WriteLine("Opomena je vec poslata...\n");
            //        UpisUTxt.IspisiPoruke(korIme);
            //    }

            //}
            //else
            //    Console.WriteLine("Ne postoji korisnik sa zadatim korisnickim imenom " + MojaBaza.korisnici[korIme].KorisnickoIme + " za skidanje mesecne provizije...\n");

            #endregion
        }

        public void MenjajParametreProvizijama(double minimalnaPotrosnja, double sumaKojuDobijaKorisnikZaDozvMinus,double  provizijaZaIznoseDo3000,
                                        double provizijaZaIsnosePreko3000, double provizijaZaMomTransfer, int skalabilneSekunde, double provizijaZaMesecnoOdrzavanje, int brojac)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Admiri"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return;
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            File.Delete("Provizije.txt");
            string textZaUpis = minimalnaPotrosnja + ";" + sumaKojuDobijaKorisnikZaDozvMinus + ";" + provizijaZaIznoseDo3000 + ";" + provizijaZaIsnosePreko3000 + ";" + provizijaZaMomTransfer + ";" + skalabilneSekunde + ";" + provizijaZaMesecnoOdrzavanje + ";" + brojac;
            File.WriteAllText("Provizije.txt", textZaUpis);                                    
        }

        public bool UlogujSe(string korIme, string sifra)
        {
            if (MojaBaza.admini.ContainsKey(korIme))
            {
                if (MojaBaza.admini[korIme].Sifra == Hash(sifra))
                {
                    Console.WriteLine("Admin sa korisnickim imenom: " + korIme + " je uspesno ulogovan...\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Pogresna lozinka...\n");
                    return false;
                }
            }
            else if (MojaBaza.korisnici.ContainsKey(korIme))
            {
                if (MojaBaza.korisnici[korIme].Sifra == Hash(sifra))
                {
                    Console.WriteLine("Korisnik sa korisnickim imenom: " + korIme + " je uspesno ulogovan...\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Pogresna lozinka...\n");
                    return false;
                }
            }
            else if (MojaBaza.radnici.ContainsKey(korIme))
            {
                if (MojaBaza.radnici[korIme].Sifra == Hash(sifra))
                {
                    Console.WriteLine("Radnik sa korisnickim imenom: " + korIme + " je uspesno ulogovan...\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Pogresna lozinka...\n");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Ne postoji korisnicko ime " + korIme + " u bazi podataka...\n");
                return false;
            }
        }

        public static string Hash(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)));
            }
        }

        public string IspisiPoruke(string korIme)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity wID = id as WindowsIdentity;

            CustomPrincipal principal = new CustomPrincipal(wID);


            if (!principal.IsInRole("Korisnik"))
            {
                MyAuditBehavior.LogNeuspesnaAutorizacija();
                Console.WriteLine("Nemate permisiju..\n");
                return "";
            }

            MyAuditBehavior.LogUspesnaAutorizacija();

            if (MojaBaza.korisnici[korIme].BrojacZaOpomenu == MojaBaza.brojac)
            {
                return "Dragi korisnice" + korIme + " u opomeni ste";
            }
            else
            {
                return "Nemate poruka";
            }
        }

        public void IspisZaPromenuTransakcije(string kor)
        {
            Console.WriteLine("Ispis transakcija korisnika:\n");
            foreach (var v in MojaBaza.korisnici[kor].PrethodneTransakcije)
            {
                Console.WriteLine("Transakcija broj: " + v.Key + " , vrednost transakcije =  " + v.Value);
            }
        }
    }
}
