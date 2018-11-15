using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security;
using Contracts;

namespace ClientApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			string address = "net.tcp://localhost:9999/SecurityService";

            //WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address)));


            using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address))))
            {
                Console.WriteLine("Unesite korisnicko ime i sifru!");
                string tempKorisnickoIme = Console.ReadLine();
                string tempLozinka = Console.ReadLine().Trim();

                proxy.UlogujSe(tempKorisnickoIme, tempLozinka);

                Console.WriteLine("Klijent je povezan");
                Console.WriteLine(proxy.IspisiPoruke(tempKorisnickoIme));

                while (true)
                {
                    Console.WriteLine("*****MENI*****");
                    Console.WriteLine("Izaberite opciju:\n");
                    Console.WriteLine("***UKOLIKO STE ADMIN MOZETE IZABRATI NEKU OD OVIH OPCIJA***");

                    Console.WriteLine("1. Dodaj radnika...\n");
                    Console.WriteLine("2. Menjaj radnike...\n");
                    Console.WriteLine("3. Obrisi radnika...\n");
                    Console.WriteLine("4. Menjaj parametre provizijama...\n");

                    Console.WriteLine("***UKOLIKO STE RADNIK MOZETE IZABRATI NEKU OD OVIH OPCIJA***");

                    Console.WriteLine("5. Dodaj korisnika...\n");
                    Console.WriteLine("6. Menjaj korisnika...\n");
                    Console.WriteLine("7. Obrisi korisnika...\n");
                    Console.WriteLine("8. Menjaj transakciju korisnika...\n");
                    Console.WriteLine("9. Obrisi transakciju korisnika...\n");
                    Console.WriteLine("10. Zatrazi dozvoljeni minus...\n");

                    Console.WriteLine("***UKOLIKO STE KORISINIK MOZETE IZABRATI NEKU OD OVIH OPCIJA***");
                    Console.WriteLine("11. Isplata...\n");
                    Console.WriteLine("12. Uplata...\n");
                    proxy.Ispis();

                    string tempSaKonzoleOpcija = Console.ReadLine();
                    if (tempSaKonzoleOpcija == "1")
                    {
                        Console.WriteLine("Unesite ime radnika...\n");
                        string imeRadnikaSaKonzole = Console.ReadLine();
                        Console.WriteLine("Unesite prezime radnika...\n");
                        string prezimeRadnikaSaKonzole = Console.ReadLine();
                        Console.WriteLine("Unesite korisnicko ime radnika...\n");
                        string korImeSaKonzole = Console.ReadLine();
                        Console.WriteLine("Unesite sifru radnika...\n");
                        string sifraSaKonzole = Console.ReadLine();

                        proxy.DodajRadnika(imeRadnikaSaKonzole, prezimeRadnikaSaKonzole, korImeSaKonzole, sifraSaKonzole);
                    }
                    else if (tempSaKonzoleOpcija == "2")
                    {

                        Console.WriteLine("Unesite korisnicko ime radnika kome zelite da menjate podatke...\n");
                        string ki = Console.ReadLine();
                        Console.WriteLine("Unesite novo izmenjeno ime radnika ili staro ukoliko niste menjali ime...\n");
                        string ime = Console.ReadLine();
                        Console.WriteLine("Unesite novo izmenjeno prezime radnika ili staro ukoliko niste menjali prezime...\n");
                        string prezime = Console.ReadLine();
                        Console.WriteLine("Unesite novu izmenjenu sifru radnika ili staru ukoliko niste menjali sifru...\n");
                        string si = Console.ReadLine();

                        proxy.MenjajPostojecegRadnika(ime, prezime, ki, si);

                    }
                    else if (tempSaKonzoleOpcija == "3")
                    {
                        Console.WriteLine("Unesite korisnickon ime radnika kojeg zelite da obrisete...\n");
                        string temp = Console.ReadLine();
                        proxy.ObrisiRadnika(temp);
                    }
                    else if (tempSaKonzoleOpcija == "4")
                    {
                        Console.WriteLine("Unesite novu vrednost za vrednost minimalne potrosnje kako bi korisniku bio odobren minus..\n");
                        double mp = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Unesite novu vrednost parametra za racunanje minimalne sume koju korisnik dobija za dozvoljeni minus...\n");
                        double ms = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Unesite vrednost provizije za iznose do 3000...\n");
                        double do3000 = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Unesite vrednost provizije za iznose preko 3000...\n");
                        double preko3000 = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Unesite vrednost provizije za momentalni transfer...\n");
                        double momTr = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Unesite na koliko sekundi se skida provizija za mesecno odrzavanje...\n");
                        int ss = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Unesite paramatar koji govori koliko je provizija za mesecno odrzavanje...\n");
                        double mo = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Unesite brojac za mesecno skidanje provizije...\n");
                        int b = Int32.Parse(Console.ReadLine());
                        proxy.MenjajParametreProvizijama(mp, ms, do3000, preko3000, momTr, ss, mo, b);
                    }
                    else if (tempSaKonzoleOpcija == "5")
                    {
                        Console.WriteLine("Unesite ime korisnika...\n");
                        string imeKorisnikaSaKonzole = Console.ReadLine();
                        Console.WriteLine("Unesite prezime korisnika...\n");
                        string prezimeKorisnikaSaKonzole = Console.ReadLine();
                        Console.WriteLine("Unesite korisnicko ime korisnika...\n");
                        string korImeSaKonzole = Console.ReadLine();
                        Console.WriteLine("Unesite sifru korisnika...\n");
                        string sifraSaKonzole = Console.ReadLine();
                        Console.WriteLine("Unesite broj racuna korisnika...\n");
                        string brRac = Console.ReadLine();
                        Console.WriteLine("Unesite trenutno stanje korisnika...\n");
                        double stanje = Double.Parse(Console.ReadLine());
                        proxy.DodajKorisnika(imeKorisnikaSaKonzole, prezimeKorisnikaSaKonzole, sifraSaKonzole, korImeSaKonzole, brRac, stanje);

                    }
                    else if (tempSaKonzoleOpcija == "6")
                    {
                        Console.WriteLine("Unesite korisnicko ime korisnika kome zelite da menjate podatke...\n");
                        string ki = Console.ReadLine();
                        Console.WriteLine("Unesite novo izmenjeno ime korisnika ili staro ukoliko niste menjali ime...\n");
                        string ime = Console.ReadLine();
                        Console.WriteLine("Unesite novo izmenjeno prezime korisnika ili staro ukoliko niste menjali prezime...\n");
                        string prezime = Console.ReadLine();
                        Console.WriteLine("Unesite novu izmenjenu sifru korisnika ili staru ukoliko niste menjali sifru...\n");
                        string si = Console.ReadLine();

                        proxy.MenjajKorisnika(ime, prezime, si, ki);
                    }
                    else if (tempSaKonzoleOpcija == "7")
                    {
                        Console.WriteLine("Unesite korisnicko ime korisnika kojeg zelite da obrisete...\n");
                        string temp = Console.ReadLine();
                        proxy.ObrisiKorisnika(temp);
                    }
                    else if (tempSaKonzoleOpcija == "8")
                    {
                        Console.WriteLine("Unesite korisnicko ime korisnika kojem zelite da izmenite transakciju..\n");
                        string ki = Console.ReadLine();

                        proxy.IspisZaPromenuTransakcije(ki);

                        Console.WriteLine("Unesite novi iznos novca transakcije...\n");
                        double p = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Unesite broj transakcije korisnika...\n");
                        int bt = Int32.Parse(Console.ReadLine());

                        proxy.MenjajTransakcijuKorisnika(ki, p, bt);
                        proxy.IspisZaPromenuTransakcije(ki);

                    }
                    else if (tempSaKonzoleOpcija == "9")
                    {
                        Console.WriteLine("Unesite korisnicko ime korisnika ciju transakciju zelite  da obrisete...\n");
                        string ki = Console.ReadLine();
                        Console.WriteLine("Unesite broj transakcije koju zelite da obrisete");
                        int bt = Int32.Parse(Console.ReadLine());

                        proxy.ObrisiTransakcijuKorisnika(ki, bt);
                    }
                    else if (tempSaKonzoleOpcija == "10")
                    {
                        Console.WriteLine("Unesite korisnicko ime korisnika koji zeli da trazi dozvoljeni minus...\n");
                        string ki = Console.ReadLine();

                        proxy.KorisnikTraziDozvoljeniMinus(ki);
                    }
                    else if (tempSaKonzoleOpcija == "11")
                    {
                        Console.WriteLine("Unesite korisnicko ime korisnika koji zeli da isplati novac...\n");
                        string ki = Console.ReadLine();
                        Console.WriteLine("Unesite koliku svotu novca korisnik zeli da isplati...\n");
                        double n = Double.Parse(Console.ReadLine());

                        proxy.Isplata(ki, n);

                    }
                    else if (tempSaKonzoleOpcija == "12")
                    {
                        Console.WriteLine("Unesite korisnicko ime korisnika koji zeli da izvrsi uplatu...\n");
                        string ki = Console.ReadLine();
                        Console.WriteLine("Unesite korisnicko ime korisnika kojem zelite da uplatite novac...\n ");
                        string ku = Console.ReadLine();
                        Console.WriteLine("Unesite koliku svotu novca zelite da uplatite...\n");
                        double n = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Vrsta uplate!\n");
                        Console.WriteLine("1-uplata drugoj osobi koja je korisnik sistema...\n");
                        Console.WriteLine("2-uplata drugoj osobi koja nije korisnik sistema(manje ili tacno 3000,00 RSD)...\n");
                        Console.WriteLine("3-uplata drugoj osobi koja nije korisnik sistema(vise od 3000,00 RSD)...\n");
                        Console.WriteLine("4-momentalni transfer...\n");
                        int temp = Int32.Parse(Console.ReadLine());
                        proxy.Uplata(ku, ki, n, temp);
                        Console.WriteLine("Uplata se izvrsava...\n");
                    }
                    else
                    {
                        Console.WriteLine("OPCIJA NE POSTOJI!!!\n");
                    }

                }


            }

               // Console.ReadLine();
            
		}
	}
}
