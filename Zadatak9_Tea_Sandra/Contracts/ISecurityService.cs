using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;

namespace Contracts
{ 
    [ServiceContract]
    public interface ISecurityService
    {
        [OperationContract]
        void Ispis();
        //ovo radi admin
        [OperationContract]
        bool DodajRadnika(string ime, string prezime, string korIme, string sifra);

        [OperationContract]
        void MenjajParametreProvizijama(double minimalnaPotrosnja, double sumaKojuDobijaKorisnikZaDozvMinus, double provizijaZaIznoseDo3000,
                                        double provizijaZaIsnosePreko3000, double provizijaZaMomTransfer, int skalabilneSekunde, double provizijaZaMesecnoOdrzavanje, int brojac);


        [OperationContract]
        bool MenjajPostojecegRadnika(string ime, string prezime, string korIme, string sifra);

        [OperationContract]
        bool UlogujSe(string korIme, string sifra);

        [OperationContract]
        bool ObrisiRadnika(string korIme);

        //ovo radi radnik
        [OperationContract]
        bool DodajKorisnika(string ime, string prezime, string sifra, string korIme, string brojRacuna, double stanje);

        [OperationContract]
        bool MenjajKorisnika(string ime, string prezime, string sifra, string korIme);

        [OperationContract]
        bool ObrisiKorisnika(string korIme);

        [OperationContract]
        bool KorisnikTraziDozvoljeniMinus(string korIme);

        [OperationContract]
        bool Uplata(string korIme, string korImeUplatioc, double novac, int izbor);

        [OperationContract]
        bool Isplata(string korIme, double novac);

        [OperationContract]
        bool MenjajTransakcijuKorisnika(string korIme, double pare, int brojTransakcije);

        [OperationContract]
        bool ObrisiTransakcijuKorisnika(string korIme, int brojTransakcije);

        [OperationContract]
        string IspisiPoruke(string korIme);

        [OperationContract]
        void IspisZaPromenuTransakcije(string kor);
    }
}
