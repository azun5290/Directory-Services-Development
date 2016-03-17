/*=====================================================================
  File:     DomainConfigData.cs

  Summary:  Section for retrieving information about the current
            domain using the Domain class and the GetCurrentDomain
            method.
---------------------------------------------------------------------
                        ** SAMPLE CODE **
*/

using System;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]

namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{
    public partial class FromDomain
    {
        public static void GetConfigData()
        {

            Console.WriteLine("<--DOMAIN INFORMATION-->\n");

            Domain domain;

            try
            {
                // bind to the current domain
                domain = Domain.GetCurrentDomain();
            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Current domain: {0}\n", domain.Name);


            // get the dn of the current domain by using GetDirectoryEntry
            string domainDn = domain.GetDirectoryEntry()
                                    .Properties["distinguishedName"]
                                    .Value.ToString();

            Console.WriteLine("The distinguishedName of the current domain is: {0}",
                                domainDn);

            // get domain's parent domain
            Console.Write("\nParent domain: ");
            Domain parentDomain = domain.Parent;
            if (parentDomain == null)
            {
                Console.WriteLine("The current domain is the root of the domain tree.");
            }
            else
            {
                Console.WriteLine(domain.Parent);
            }

            // all child domains
            Console.Write("\nChild domains:");
            foreach (Domain childDomain in domain.Children)
            {
                Console.WriteLine(childDomain.Name);
            }

            // all domain controllers within the domain
            Console.WriteLine("\nDomain controllers in the domain:");
            foreach (DomainController dc in domain.DomainControllers)
            {
                Console.WriteLine(dc.Name);
            }

            // Find the PDC
            Console.WriteLine("\nPDC: {0}", domain.PdcRoleOwner);

        }
    }
}
