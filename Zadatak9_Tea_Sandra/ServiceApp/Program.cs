using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Contracts;
using System.Security;
using System.Threading;

namespace ServiceApp
{
	public class Program
	{
		static void Main(string[] args)
		{
            MyAuditBehavior myAuditBehavior = new MyAuditBehavior();

			NetTcpBinding binding = new NetTcpBinding();
            //setovanje windows autentifikacije
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			string address = "net.tcp://localhost:9999/SecurityService";



             Admin admin = new Admin("Milan", "Djokic", "Milance", SecurityService.Hash("sifra"));
             MojaBaza.admini.Add("Milance", admin);
             Admin admin1 = new Admin("Tea", "Ilic", "Teica", SecurityService.Hash("sifra"));
             MojaBaza.admini.Add("Teica", admin1);

             Korisnik korisnik = new Korisnik("Prosingoje", "Prosincic", "Proske", SecurityService.Hash("sifra"), "7777", 15750);
             MojaBaza.korisnici.Add("Proske", korisnik);

            Korisnik korisnik1 = new Korisnik("Marko", "Kovac", "markic", SecurityService.Hash("sifra"), "71237", 300000);
            MojaBaza.korisnici.Add("markic", korisnik1);

            Radnik radnik = new Radnik("Ana", "Bugarski", "Anci", SecurityService.Hash("sifra"));
             MojaBaza.radnici.Add("Anci", radnik);

           
            ServiceHost host = new ServiceHost(typeof(SecurityService));
			host.AddServiceEndpoint(typeof(Contracts.ISecurityService), binding, address);

			host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
			host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });


            MojaBaza.UcitajIzTxtProvizije();
			host.Open();

            

            Thread thread = new Thread(SecurityService.MesecnoSkidanjeProvizije);
            thread.Start();

			Console.WriteLine("Servis je pokrenut...");
			Console.ReadLine();

            SecurityService.ZavrsiThread = true;
            thread.Join();
            host.Close();
		}

        
	}
}
