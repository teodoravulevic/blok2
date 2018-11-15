using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace ServiceApp
{
    public class CustomPrincipal : IPrincipal
    {
        private WindowsIdentity windowsIdentity;
        private HashSet<permisions> _permisions;

        public CustomPrincipal(WindowsIdentity windowsIdentity)
        {
            this.windowsIdentity = windowsIdentity;
            _permisions = new HashSet<permisions>();

            var _groups = this.windowsIdentity.Groups;

            foreach (var item in _groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)item.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));

                var temp = name.ToString();                                            
                if (name.Value.Contains("\\"))
                    temp = name.ToString().Split('\\')[1];

                if (temp == "Admiri")                                                  
                {
                    if (!_permisions.Contains(permisions.Admiri))
                        _permisions.Add(permisions.Admiri);
                }

                else if (temp == "Radnik")
                {
                    if (!_permisions.Contains(permisions.Radnik))
                        _permisions.Add(permisions.Radnik);
                }
                else if (temp == "Korisnik")
                {
                    if (!_permisions.Contains(permisions.Korisnik))
                        _permisions.Add(permisions.Korisnik);
                }
            }

        }

        public IIdentity Identity { get { return windowsIdentity; } }

        public bool IsInRole(string role)
        {

            switch (role)                                                               
            {
                case "Admiri":
                    if (_permisions.Contains(permisions.Admiri))
                        return true;
                    else
                        return false;
                case "Radnik":
                    if (_permisions.Contains(permisions.Radnik))
                        return true;
                    else
                        return false;
                case "Korisnik":
                    if (_permisions.Contains(permisions.Korisnik))
                        return true;
                    else
                        return false;
                default:
                    return false;
            }

        }
    }
}
