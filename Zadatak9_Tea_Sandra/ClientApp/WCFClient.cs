using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using ISecurityService = Contracts.ISecurityService;
using System.Security;

namespace ClientApp
{
	public class WCFClient : ChannelFactory<Contracts.ISecurityService>, ISecurityService, IDisposable
	{
        Contracts.ISecurityService factory;

		public WCFClient(NetTcpBinding binding, EndpointAddress address)
			: base(binding, address)
		{
			factory = this.CreateChannel();
		}
	
		public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			this.Close();
		}

        public void DodajKorisnika(string ime, string prezime, string sifra, string korIme, string brojRacuna, double stanje)
        {
            try
            {
                factory.DodajKorisnika(ime, prezime, sifra, korIme, brojRacuna, stanje);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public void DodajRadnika(string ime, string prezime, string korIme, string sifra)
        {
            try
            {
                factory.DodajRadnika(ime, prezime, korIme, sifra);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public void Ispis()
        {
            try
            {
                factory.Ispis();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public string IspisiPoruke(string korIme)
        {
            try
            {
                string poruka = factory.IspisiPoruke(korIme);
                return poruka;
            }
            catch(Exception e)
            {
                return "";
             
            }
        }

        public void IspisZaPromenuTransakcije(string kor)
        {
            factory.IspisZaPromenuTransakcije(kor);
        }

        public bool Isplata(string korIme, double novac)
        {
            try
            {
                return factory.Isplata(korIme, novac);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
           
        }

        public bool KorisnikTraziDozvoljeniMinus(string korIme)
        {
            try
            {
            return factory.KorisnikTraziDozvoljeniMinus(korIme);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void MenjajKorisnika(string ime, string prezime, string sifra, string korIme)
        {
            try
            {
            factory.MenjajKorisnika(ime, prezime, sifra, korIme);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public void MenjajParametreProvizijama(double minimalnaPotrosnja, double sumaKojuDobijaKorisnikZaDozvMinus, double provizijaZaIznoseDo3000, double provizijaZaIsnosePreko3000, double provizijaZaMomTransfer, int skalabilneSekunde, double provizijaZaMesecnoOdrzavanje, int brojac)
        {
            try
            {
            factory.MenjajParametreProvizijama(minimalnaPotrosnja, sumaKojuDobijaKorisnikZaDozvMinus, provizijaZaIznoseDo3000, provizijaZaIsnosePreko3000, provizijaZaMomTransfer, skalabilneSekunde, provizijaZaMesecnoOdrzavanje, brojac);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public void MenjajPostojecegRadnika(string ime, string prezime, string korIme, string sifra)
        {
            try
            {
            factory.MenjajPostojecegRadnika(ime, prezime, korIme, sifra);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public bool MenjajTransakcijuKorisnika(string korIme, double pare, int brojTransakcije)
        {
            try
            {
                return factory.MenjajTransakcijuKorisnika(korIme, pare, brojTransakcije);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        public void ObrisiKorisnika(string korIme)
        {
            try
            {
            factory.ObrisiKorisnika(korIme);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public void ObrisiRadnika(string korIme)
        {
            try
            {
                factory.ObrisiRadnika(korIme);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public bool ObrisiTransakcijuKorisnika(string korIme, int brojTransakcije)
        {
            try
            {
                return factory.ObrisiTransakcijuKorisnika(korIme, brojTransakcije);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool UlogujSe(string korIme, string sifra)
        {
            try
            {
            return factory.UlogujSe(korIme, sifra);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Uplata(string korIme, string korImeUplatioc, double novac, int izbor)
        {
            try
            {
            return factory.Uplata(korIme, korImeUplatioc, novac, izbor);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        

        bool ISecurityService.DodajKorisnika(string ime, string prezime, string sifra, string korIme, string brojRacuna, double stanje)
        {
            try
            {
            return factory.DodajKorisnika(ime, prezime, sifra, korIme, brojRacuna, stanje);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        bool ISecurityService.DodajRadnika(string ime, string prezime, string korIme, string sifra)
        {
            try
            {
                return factory.DodajRadnika(ime, prezime, korIme, sifra);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        bool ISecurityService.MenjajKorisnika(string ime, string prezime, string sifra, string korIme)
        {
            try
            {
                return factory.MenjajKorisnika(ime, prezime, sifra, korIme);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        bool ISecurityService.MenjajPostojecegRadnika(string ime, string prezime, string korIme, string sifra)
        {
            try
            {
            return factory.MenjajPostojecegRadnika(ime, prezime, korIme, sifra);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        bool ISecurityService.ObrisiKorisnika(string korIme)
        {
            try
            {
            return factory.ObrisiKorisnika(korIme);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        bool ISecurityService.ObrisiRadnika(string korIme)
        {
            try
            {
                return factory.ObrisiRadnika(korIme);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
